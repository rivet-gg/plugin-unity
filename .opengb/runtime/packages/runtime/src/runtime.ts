import { ModuleContextParams, ScriptContext } from "./context.ts";
import { Context, RouteContext, TestContext } from "./context.ts";
import { PgPoolDummy, Postgres, PrismaClientDummy } from "./postgres.ts";
import { TraceEntryType } from "./trace.ts";
import { newTrace } from "./trace.ts";
import { RegistryCallMap } from "./proxy.ts";
import { ActorDriver } from "./actor/driver.ts";
import { ActorBase } from "./actor/actor.ts";
import { QualifiedPathPair } from "../../path_resolver/src/mod.ts";
import { ContextParams } from "./mod.ts";
import { errorToLogEntries, log, LOGGER_CONFIG } from "./logger.ts";
import { Environment } from "./environment.ts";

interface ParseSuccessResult<Output = unknown> {
	success: true;
	data: Output;
	error?: never;
}

interface ParseErrorResult {
	success: false;
	error: Error;
	data?: never;
}

interface ValidationSchema {
	safeParseAsync: <Output = unknown>(
		data: unknown,
	) => Promise<
		ParseSuccessResult<Output> | ParseErrorResult
	>;
}

export interface Config {
	runtime: BuildRuntime;
	modules: Record<string, Module>;
	cors?: CorsConfig;
	db: {
		createPgPool: (url: URL) => PgPoolDummy;
	};
}

/**
 * Which runtime to target when building.
 */
export enum BuildRuntime {
	Deno,
	Cloudflare,
}

export interface Module {
	storageAlias: string;
	scripts: Record<string, Script>;
	actors: Record<string, Actor>;
	routes: Record<string, Route>;
	errors: Record<string, ErrorConfig>;
	db?: {
		/** Name of the Postgres schema the tables live in. */
		schema: string;
		createPrismaClient: (pool: PgPoolDummy, schema: string) => PrismaClientDummy;
	};
	dependencies: Set<string>;
	userConfig: unknown;
}

export interface CorsConfig {
	origins: Set<string>;
}

export interface Script {
	// deno-lint-ignore no-explicit-any
	run: ScriptRun<any, any, any, any, any>;
	requestSchema: ValidationSchema;
	responseSchema: ValidationSchema;
	public: boolean;
}

export interface RouteBase {
	// deno-lint-ignore no-explicit-any
	run: RouteRun<any, any, any>;
	methods: Set<string>;
}

export interface PrefixRoute extends RouteBase {
	pathPrefix: string;
}

export interface ExactRoute extends RouteBase {
	path: string;
}

export type Route = ExactRoute | PrefixRoute;

export type ScriptRun<Req, Res, UserConfigT, DatabaseT, DatabaseSchemaT> = (
	ctx: ScriptContext<{
		dependenciesSnake: any;
		dependenciesCamel: any;
		actorsSnake: any;
		actorsCamel: any;
		userConfig: UserConfigT;
		database: DatabaseT;
		databaseSchema: DatabaseSchemaT;
	}>,
	req: Req,
) => Promise<Res>;

export interface Actor {
	// This monstrosity is to allow passing the constructor a subclass of ActorBase.
	actor: new (
		...args: ConstructorParameters<typeof ActorBase<unknown, unknown>>
	) => ActorBase<unknown, unknown>;
	storageAlias: string;
}

export type RouteRun<UserConfigT, DatabaseT, DatabaseSchemaT> = (
	ctx: RouteContext<{
		dependenciesSnake: any;
		dependenciesCamel: any;
		actorsSnake: any;
		actorsCamel: any;
		userConfig: UserConfigT;
		database: DatabaseT;
		databaseSchema: DatabaseSchemaT;
	}>,
	req: Request,
) => Promise<Response>;

export interface ErrorConfig {
	description?: string;
}

export class Runtime<Params extends ContextParams> {
	public postgres: Postgres;

	public hostname: string;
	public port: number;
	public publicEndpoint: string;

	public constructor(
		public readonly env: Environment,
		public readonly config: Config,
		public actorDriver: ActorDriver,
		private dependencyCaseConversionMap: RegistryCallMap,
		private actorCaseConversionMap: RegistryCallMap,
	) {
		// Read config
		this.hostname = env.get("OPENGB_HOSTNAME") ?? "127.0.0.1";
		this.port = parseInt(env.get("OPENGB_PORT") ?? "6420");
		this.publicEndpoint = env.get("OPENGB_PUBLIC_ENDPOINT") ?? `http://${this.hostname}:${this.port}`;

		// Configure logger
		LOGGER_CONFIG.enableSpreadObject = env.get("_OPENGB_LOG_SPILT_OBJECT") == "1";
		LOGGER_CONFIG.enableErrorStack = env.get("_OPENGB_LOG_ERROR_STACK") == "1";
		if (env.get("OPENGB_TERM_COLOR") === "never") {
			LOGGER_CONFIG.enableColor = false;
		} else if (env.get("OPENGB_TERM_COLOR") === "always") {
			LOGGER_CONFIG.enableColor = true;
		} else if (env.get("NO_COLOR") != undefined && env.get("NO_COLOR") != "") {
			// https://no-color.org/
			LOGGER_CONFIG.enableColor = false;
		} else {
			LOGGER_CONFIG.enableColor = globalThis.Deno?.stdout?.isTerminal() == true;
		}

		// Create database
		this.postgres = new Postgres();
	}

	private async shutdown() {
		await this.postgres.shutdown();
	}

	public createRootContext(
		traceEntryType: TraceEntryType,
	): Context<{
		dependenciesSnake: Params["dependenciesSnake"];
		dependenciesCamel: Params["dependenciesCamel"];
		actorsSnake: Params["actorsSnake"];
		actorsCamel: Params["actorsCamel"];
	}> {
		return new Context(
			this,
			newTrace(traceEntryType, this.config.runtime),
			this.dependencyCaseConversionMap,
			this.actorCaseConversionMap,
		);
	}

	public createRouteContext(
		ctx: Context<Params>,
		moduleName: string,
		routeName: string,
	) {
		// FIXME: This is a pretty terrible hack. We should find a better way to
		// do this, probably with a "public" underscore function.
		const ctxWithPublicGRC = ctx as unknown as { getRouteContext: Context<Params>["getRouteContext"] };
		return ctxWithPublicGRC.getRouteContext(moduleName, routeName);
	}

	public createRootRouteContext(
		traceEntryType: TraceEntryType,
		moduleName: string,
		routeName: string,
	): RouteContext<{
		"dependenciesSnake": Params["dependenciesSnake"];
		"dependenciesCamel": Params["dependenciesCamel"];
		"actorsSnake": Params["actorsSnake"];
		"actorsCamel": Params["actorsCamel"];
		"userConfig": any;
		"database": PrismaClientDummy | undefined;
		"databaseSchema": any;
	}> {
		const module = this.config.modules[moduleName];
		if (!module) throw new Error(`Module not found: ${moduleName}`);

		return new RouteContext(
			this,
			newTrace(traceEntryType),
			moduleName,
			this.postgres.getOrCreatePrismaClient(this.env, this.config, module),
			module.db?.schema,
			routeName,
			this.dependencyCaseConversionMap,
			this.actorCaseConversionMap,
		);
	}

	/**
	 * Registers a module test with the Deno runtime.
	 */
	public static test<Params extends ModuleContextParams>(
		config: Config,
		actorDriver: ActorDriver,
		moduleName: string,
		testName: string,
		fn: (
			ctx: TestContext<Params>,
		) => Promise<void>,
		dependencyCaseConversionMap: RegistryCallMap,
		actorDependencyCaseConversionMap: RegistryCallMap,
	) {
		Deno.test({
			name: testName,

			// TODO: https://github.com/rivet-gg/opengb-engine/issues/35
			sanitizeOps: false,
			sanitizeResources: false,

			async fn() {
				const runtime = new Runtime<Params>(
					Deno.env,
					config,
					actorDriver,
					dependencyCaseConversionMap,
					actorDependencyCaseConversionMap,
				);

				// Build context
				const module = config.modules[moduleName]!;
				const ctx = new TestContext<Params>(
					runtime,
					newTrace({
						test: { module: moduleName, name: testName },
					}),
					moduleName,
					runtime.postgres.getOrCreatePrismaClient(runtime.env, runtime.config, module),
					module.db?.schema,
					dependencyCaseConversionMap,
					actorDependencyCaseConversionMap,
				);

				// Run test
				try {
					await ctx.runBlock(async () => {
						await fn(ctx);
					});
				} catch (cause) {
					log(
						"error",
						"failed to execute test",
						["module", moduleName],
						["test", testName],
						...errorToLogEntries("cause", cause),
					);
					throw cause;
				} finally {
					await runtime.shutdown();
				}
			},
		});
	}

	/**
	 * Only runs on a CORS preflight request— returns a response with the
	 * appropriate CORS headers & status.
	 *
	 * @param req The preflight OPTIONS request
	 * @returns The full response to the preflight request
	 */
	public corsPreflight(req: Request): Response {
		const origin = req.headers.get("Origin");
		if (origin) {
			const normalizedOrigin = new URL(origin).origin;
			if (this.config.cors) {
				if (this.config.cors.origins.has(normalizedOrigin)) {
					return new Response(undefined, {
						status: 204,
						headers: {
							...this.corsHeaders(req),
							"Vary": "Origin",
						},
					});
				}
			}
		}

		// Origin is not allowed/no origin header on preflight
		return new Response(
			JSON.stringify({
				"message": "CORS origin not allowed. See https://opengb.dev/docs/cors",
			}),
			{
				status: 403,
				headers: {
					"Vary": "Origin",
				},
			},
		);
	}

	public corsHeaders(req: Request): Record<string, string> {
		const origin = req.headers.get("Origin");

		// Don't set CORS headers if there's no origin (e.g. a server-side
		// request)
		if (!origin) return {};

		// If the origin is allowed, return the appropriate headers.
		// Otherwise, return a non-matching cors header (empty object).
		if (this.config.cors?.origins.has(origin)) {
			return {
				"Access-Control-Allow-Origin": new URL(origin).origin,
				"Access-Control-Allow-Methods": "*",
				"Access-Control-Allow-Headers": "*",
			};
		} else {
			return {};
		}
	}

	public corsAllowed(req: Request): boolean {
		const origin = req.headers.get("Origin");

		if (!origin) return true;
		return this.config.cors?.origins.has(origin) ?? false;
	}

	public routePaths(): QualifiedPathPair[] {
		const paths: QualifiedPathPair[] = [];
		for (const [moduleName, module] of Object.entries(this.config.modules)) {
			for (const [routeName, route] of Object.entries(module.routes)) {
				if ("path" in route) {
					paths.push({
						module: moduleName,
						route: routeName,
						path: { path: route.path, isPrefix: false },
					});
				} else {
					paths.push({
						module: moduleName,
						route: routeName,
						path: { path: route.pathPrefix, isPrefix: true },
					});
				}
			}
		}
		return paths;
	}
}

export type PathPair = { path: string; isPrefix: boolean };
