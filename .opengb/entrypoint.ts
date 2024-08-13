// This file is auto-generated by the Open Game Backend (https://opengb.dev) build system.
//
// Do not edit this file directly.
//
// Generated at 2024-07-29T07:22:45.889Z
import { Runtime } from "/Users/nathan/rivet/plugin-unity/.opengb/runtime/packages/runtime/src/mod.ts";
import { dependencyCaseConversionMap } from "/Users/nathan/rivet/plugin-unity/.opengb/dependencyCaseConversion.ts";
import { actorCaseConversionMap } from "/Users/nathan/rivet/plugin-unity/.opengb/actorCaseConversion.ts";
import type { DependenciesCamel, DependenciesSnake } from "./dependencies.d.ts";
import type { ActorsCamel, ActorsSnake } from "./actors.d.ts";
import config from "./runtime_config.ts";
import { handleRequest } from "/Users/nathan/rivet/plugin-unity/.opengb/runtime/packages/runtime/src/server.ts";
import { ActorDriver } from "/Users/nathan/rivet/plugin-unity/.opengb/runtime/packages/runtime/src/actor/drivers/memory/driver.ts";
import { PathResolver } from "/Users/nathan/rivet/plugin-unity/.opengb/runtime/packages/path_resolver/src/mod.ts";
import { log } from "/Users/nathan/rivet/plugin-unity/.opengb/runtime/packages/runtime/src/logger.ts";

const runtime = new Runtime<{
	dependenciesSnake: DependenciesSnake;
	dependenciesCamel: DependenciesCamel;
	actorsSnake: ActorsSnake;
	actorsCamel: ActorsCamel;
}>(
	Deno.env,
	config,
	new ActorDriver(
		Deno.env,
		config,
		dependencyCaseConversionMap,
		actorCaseConversionMap,
	),
	dependencyCaseConversionMap,
	actorCaseConversionMap,
);

const resolver = new PathResolver(runtime.routePaths());

await Deno.serve(
	{
		hostname: runtime.hostname,
		port: runtime.port,
		onListen: () => {
			log(
				"info",
				"server started",
				["hostname", runtime.hostname],
				["port", runtime.port],
				["endpoint", runtime.publicEndpoint],
			);
		},
	},
	(req: Request, reqMeta: Deno.ServeHandlerInfo): Promise<Response> => {
		return handleRequest(
			runtime,
			req,
			{ remoteAddress: reqMeta.remoteAddr.hostname },
			resolver,
		);
	},
).finished;
