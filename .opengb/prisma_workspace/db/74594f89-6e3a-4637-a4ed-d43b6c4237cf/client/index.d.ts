
/**
 * Client
**/

// @deno-types="./runtime/library.d.ts"
import * as runtime from './runtime/library.js';
import $Types = runtime.Types // general types
import $Public = runtime.Types.Public
import $Utils = runtime.Types.Utils
import $Extensions = runtime.Types.Extensions
import $Result = runtime.Types.Result

export type PrismaPromise<T> = $Public.PrismaPromise<T>


/**
 * Model TokenBuckets
 * 
 */
export type TokenBuckets = $Result.DefaultSelection<Prisma.$TokenBucketsPayload>

/**
 * ##  Prisma Client ʲˢ
 * 
 * Type-safe database client for TypeScript & Node.js
 * @example
 * ```
 * const prisma = new PrismaClient()
 * // Fetch zero or more TokenBuckets
 * const tokenBuckets = await prisma.tokenBuckets.findMany()
 * ```
 *
 * 
 * Read more in our [docs](https://www.prisma.io/docs/reference/tools-and-interfaces/prisma-client).
 */
export class PrismaClient<
  ClientOptions extends Prisma.PrismaClientOptions = Prisma.PrismaClientOptions,
  U = 'log' extends keyof ClientOptions ? ClientOptions['log'] extends Array<Prisma.LogLevel | Prisma.LogDefinition> ? Prisma.GetEvents<ClientOptions['log']> : never : never,
  ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs
> {
  [K: symbol]: { types: Prisma.TypeMap<ExtArgs>['other'] }

    /**
   * ##  Prisma Client ʲˢ
   * 
   * Type-safe database client for TypeScript & Node.js
   * @example
   * ```
   * const prisma = new PrismaClient()
   * // Fetch zero or more TokenBuckets
   * const tokenBuckets = await prisma.tokenBuckets.findMany()
   * ```
   *
   * 
   * Read more in our [docs](https://www.prisma.io/docs/reference/tools-and-interfaces/prisma-client).
   */

  constructor(optionsArg ?: Prisma.Subset<ClientOptions, Prisma.PrismaClientOptions>);
  $on<V extends U>(eventType: V, callback: (event: V extends 'query' ? Prisma.QueryEvent : Prisma.LogEvent) => void): void;

  /**
   * Connect with the database
   */
  $connect(): $Utils.JsPromise<void>;

  /**
   * Disconnect from the database
   */
  $disconnect(): $Utils.JsPromise<void>;

  /**
   * Add a middleware
   * @deprecated since 4.16.0. For new code, prefer client extensions instead.
   * @see https://pris.ly/d/extensions
   */
  $use(cb: Prisma.Middleware): void

/**
   * Executes a prepared raw query and returns the number of affected rows.
   * @example
   * ```
   * const result = await prisma.$executeRaw`UPDATE User SET cool = ${true} WHERE email = ${'user@email.com'};`
   * ```
   * 
   * Read more in our [docs](https://www.prisma.io/docs/reference/tools-and-interfaces/prisma-client/raw-database-access).
   */
  $executeRaw<T = unknown>(query: TemplateStringsArray | Prisma.Sql, ...values: any[]): Prisma.PrismaPromise<number>;

  /**
   * Executes a raw query and returns the number of affected rows.
   * Susceptible to SQL injections, see documentation.
   * @example
   * ```
   * const result = await prisma.$executeRawUnsafe('UPDATE User SET cool = $1 WHERE email = $2 ;', true, 'user@email.com')
   * ```
   * 
   * Read more in our [docs](https://www.prisma.io/docs/reference/tools-and-interfaces/prisma-client/raw-database-access).
   */
  $executeRawUnsafe<T = unknown>(query: string, ...values: any[]): Prisma.PrismaPromise<number>;

  /**
   * Performs a prepared raw query and returns the `SELECT` data.
   * @example
   * ```
   * const result = await prisma.$queryRaw`SELECT * FROM User WHERE id = ${1} OR email = ${'user@email.com'};`
   * ```
   * 
   * Read more in our [docs](https://www.prisma.io/docs/reference/tools-and-interfaces/prisma-client/raw-database-access).
   */
  $queryRaw<T = unknown>(query: TemplateStringsArray | Prisma.Sql, ...values: any[]): Prisma.PrismaPromise<T>;

  /**
   * Performs a raw query and returns the `SELECT` data.
   * Susceptible to SQL injections, see documentation.
   * @example
   * ```
   * const result = await prisma.$queryRawUnsafe('SELECT * FROM User WHERE id = $1 OR email = $2;', 1, 'user@email.com')
   * ```
   * 
   * Read more in our [docs](https://www.prisma.io/docs/reference/tools-and-interfaces/prisma-client/raw-database-access).
   */
  $queryRawUnsafe<T = unknown>(query: string, ...values: any[]): Prisma.PrismaPromise<T>;

  /**
   * Allows the running of a sequence of read/write operations that are guaranteed to either succeed or fail as a whole.
   * @example
   * ```
   * const [george, bob, alice] = await prisma.$transaction([
   *   prisma.user.create({ data: { name: 'George' } }),
   *   prisma.user.create({ data: { name: 'Bob' } }),
   *   prisma.user.create({ data: { name: 'Alice' } }),
   * ])
   * ```
   * 
   * Read more in our [docs](https://www.prisma.io/docs/concepts/components/prisma-client/transactions).
   */
  $transaction<P extends Prisma.PrismaPromise<any>[]>(arg: [...P], options?: { isolationLevel?: Prisma.TransactionIsolationLevel }): $Utils.JsPromise<runtime.Types.Utils.UnwrapTuple<P>>

  $transaction<R>(fn: (prisma: Omit<PrismaClient, runtime.ITXClientDenyList>) => $Utils.JsPromise<R>, options?: { maxWait?: number, timeout?: number, isolationLevel?: Prisma.TransactionIsolationLevel }): $Utils.JsPromise<R>


  $extends: $Extensions.ExtendsHook<"extends", Prisma.TypeMapCb, ExtArgs>

      /**
   * `prisma.tokenBuckets`: Exposes CRUD operations for the **TokenBuckets** model.
    * Example usage:
    * ```ts
    * // Fetch zero or more TokenBuckets
    * const tokenBuckets = await prisma.tokenBuckets.findMany()
    * ```
    */
  get tokenBuckets(): Prisma.TokenBucketsDelegate<ExtArgs>;
}

export namespace Prisma {
  export import DMMF = runtime.DMMF

  export type PrismaPromise<T> = $Public.PrismaPromise<T>

  /**
   * Validator
   */
  export import validator = runtime.Public.validator

  /**
   * Prisma Errors
   */
  export import PrismaClientKnownRequestError = runtime.PrismaClientKnownRequestError
  export import PrismaClientUnknownRequestError = runtime.PrismaClientUnknownRequestError
  export import PrismaClientRustPanicError = runtime.PrismaClientRustPanicError
  export import PrismaClientInitializationError = runtime.PrismaClientInitializationError
  export import PrismaClientValidationError = runtime.PrismaClientValidationError
  export import NotFoundError = runtime.NotFoundError

  /**
   * Re-export of sql-template-tag
   */
  export import sql = runtime.sqltag
  export import empty = runtime.empty
  export import join = runtime.join
  export import raw = runtime.raw
  export import Sql = runtime.Sql

  /**
   * Decimal.js
   */
  export import Decimal = runtime.Decimal

  export type DecimalJsLike = runtime.DecimalJsLike

  /**
   * Metrics 
   */
  export type Metrics = runtime.Metrics
  export type Metric<T> = runtime.Metric<T>
  export type MetricHistogram = runtime.MetricHistogram
  export type MetricHistogramBucket = runtime.MetricHistogramBucket

  /**
  * Extensions
  */
  export import Extension = $Extensions.UserArgs
  export import getExtensionContext = runtime.Extensions.getExtensionContext
  export import Args = $Public.Args
  export import Payload = $Public.Payload
  export import Result = $Public.Result
  export import Exact = $Public.Exact

  /**
   * Prisma Client JS version: 5.17.0
   * Query Engine version: 393aa359c9ad4a4bb28630fb5613f9c281cde053
   */
  export type PrismaVersion = {
    client: string
  }

  export const prismaVersion: PrismaVersion 

  /**
   * Utility Types
   */

  /**
   * From https://github.com/sindresorhus/type-fest/
   * Matches a JSON object.
   * This type can be useful to enforce some input to be JSON-compatible or as a super-type to be extended from. 
   */
  export type JsonObject = {[Key in string]?: JsonValue}

  /**
   * From https://github.com/sindresorhus/type-fest/
   * Matches a JSON array.
   */
  export interface JsonArray extends Array<JsonValue> {}

  /**
   * From https://github.com/sindresorhus/type-fest/
   * Matches any valid JSON value.
   */
  export type JsonValue = string | number | boolean | JsonObject | JsonArray | null

  /**
   * Matches a JSON object.
   * Unlike `JsonObject`, this type allows undefined and read-only properties.
   */
  export type InputJsonObject = {readonly [Key in string]?: InputJsonValue | null}

  /**
   * Matches a JSON array.
   * Unlike `JsonArray`, readonly arrays are assignable to this type.
   */
  export interface InputJsonArray extends ReadonlyArray<InputJsonValue | null> {}

  /**
   * Matches any valid value that can be used as an input for operations like
   * create and update as the value of a JSON field. Unlike `JsonValue`, this
   * type allows read-only arrays and read-only object properties and disallows
   * `null` at the top level.
   *
   * `null` cannot be used as the value of a JSON field because its meaning
   * would be ambiguous. Use `Prisma.JsonNull` to store the JSON null value or
   * `Prisma.DbNull` to clear the JSON value and set the field to the database
   * NULL value instead.
   *
   * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-by-null-values
   */
  export type InputJsonValue = string | number | boolean | InputJsonObject | InputJsonArray | { toJSON(): unknown }

  /**
   * Types of the values used to represent different kinds of `null` values when working with JSON fields.
   * 
   * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-on-a-json-field
   */
  namespace NullTypes {
    /**
    * Type of `Prisma.DbNull`.
    * 
    * You cannot use other instances of this class. Please use the `Prisma.DbNull` value.
    * 
    * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-on-a-json-field
    */
    class DbNull {
      private DbNull: never
      private constructor()
    }

    /**
    * Type of `Prisma.JsonNull`.
    * 
    * You cannot use other instances of this class. Please use the `Prisma.JsonNull` value.
    * 
    * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-on-a-json-field
    */
    class JsonNull {
      private JsonNull: never
      private constructor()
    }

    /**
    * Type of `Prisma.AnyNull`.
    * 
    * You cannot use other instances of this class. Please use the `Prisma.AnyNull` value.
    * 
    * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-on-a-json-field
    */
    class AnyNull {
      private AnyNull: never
      private constructor()
    }
  }

  /**
   * Helper for filtering JSON entries that have `null` on the database (empty on the db)
   * 
   * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-on-a-json-field
   */
  export const DbNull: NullTypes.DbNull

  /**
   * Helper for filtering JSON entries that have JSON `null` values (not empty on the db)
   * 
   * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-on-a-json-field
   */
  export const JsonNull: NullTypes.JsonNull

  /**
   * Helper for filtering JSON entries that are `Prisma.DbNull` or `Prisma.JsonNull`
   * 
   * @see https://www.prisma.io/docs/concepts/components/prisma-client/working-with-fields/working-with-json-fields#filtering-on-a-json-field
   */
  export const AnyNull: NullTypes.AnyNull

  type SelectAndInclude = {
    select: any
    include: any
  }

  type SelectAndOmit = {
    select: any
    omit: any
  }

  /**
   * Get the type of the value, that the Promise holds.
   */
  export type PromiseType<T extends PromiseLike<any>> = T extends PromiseLike<infer U> ? U : T;

  /**
   * Get the return type of a function which returns a Promise.
   */
  export type PromiseReturnType<T extends (...args: any) => $Utils.JsPromise<any>> = PromiseType<ReturnType<T>>

  /**
   * From T, pick a set of properties whose keys are in the union K
   */
  type Prisma__Pick<T, K extends keyof T> = {
      [P in K]: T[P];
  };


  export type Enumerable<T> = T | Array<T>;

  export type RequiredKeys<T> = {
    [K in keyof T]-?: {} extends Prisma__Pick<T, K> ? never : K
  }[keyof T]

  export type TruthyKeys<T> = keyof {
    [K in keyof T as T[K] extends false | undefined | null ? never : K]: K
  }

  export type TrueKeys<T> = TruthyKeys<Prisma__Pick<T, RequiredKeys<T>>>

  /**
   * Subset
   * @desc From `T` pick properties that exist in `U`. Simple version of Intersection
   */
  export type Subset<T, U> = {
    [key in keyof T]: key extends keyof U ? T[key] : never;
  };

  /**
   * SelectSubset
   * @desc From `T` pick properties that exist in `U`. Simple version of Intersection.
   * Additionally, it validates, if both select and include are present. If the case, it errors.
   */
  export type SelectSubset<T, U> = {
    [key in keyof T]: key extends keyof U ? T[key] : never
  } &
    (T extends SelectAndInclude
      ? 'Please either choose `select` or `include`.'
      : T extends SelectAndOmit
        ? 'Please either choose `select` or `omit`.'
        : {})

  /**
   * Subset + Intersection
   * @desc From `T` pick properties that exist in `U` and intersect `K`
   */
  export type SubsetIntersection<T, U, K> = {
    [key in keyof T]: key extends keyof U ? T[key] : never
  } &
    K

  type Without<T, U> = { [P in Exclude<keyof T, keyof U>]?: never };

  /**
   * XOR is needed to have a real mutually exclusive union type
   * https://stackoverflow.com/questions/42123407/does-typescript-support-mutually-exclusive-types
   */
  type XOR<T, U> =
    T extends object ?
    U extends object ?
      (Without<T, U> & U) | (Without<U, T> & T)
    : U : T


  /**
   * Is T a Record?
   */
  type IsObject<T extends any> = T extends Array<any>
  ? False
  : T extends Date
  ? False
  : T extends Uint8Array
  ? False
  : T extends BigInt
  ? False
  : T extends object
  ? True
  : False


  /**
   * If it's T[], return T
   */
  export type UnEnumerate<T extends unknown> = T extends Array<infer U> ? U : T

  /**
   * From ts-toolbelt
   */

  type __Either<O extends object, K extends Key> = Omit<O, K> &
    {
      // Merge all but K
      [P in K]: Prisma__Pick<O, P & keyof O> // With K possibilities
    }[K]

  type EitherStrict<O extends object, K extends Key> = Strict<__Either<O, K>>

  type EitherLoose<O extends object, K extends Key> = ComputeRaw<__Either<O, K>>

  type _Either<
    O extends object,
    K extends Key,
    strict extends Boolean
  > = {
    1: EitherStrict<O, K>
    0: EitherLoose<O, K>
  }[strict]

  type Either<
    O extends object,
    K extends Key,
    strict extends Boolean = 1
  > = O extends unknown ? _Either<O, K, strict> : never

  export type Union = any

  type PatchUndefined<O extends object, O1 extends object> = {
    [K in keyof O]: O[K] extends undefined ? At<O1, K> : O[K]
  } & {}

  /** Helper Types for "Merge" **/
  export type IntersectOf<U extends Union> = (
    U extends unknown ? (k: U) => void : never
  ) extends (k: infer I) => void
    ? I
    : never

  export type Overwrite<O extends object, O1 extends object> = {
      [K in keyof O]: K extends keyof O1 ? O1[K] : O[K];
  } & {};

  type _Merge<U extends object> = IntersectOf<Overwrite<U, {
      [K in keyof U]-?: At<U, K>;
  }>>;

  type Key = string | number | symbol;
  type AtBasic<O extends object, K extends Key> = K extends keyof O ? O[K] : never;
  type AtStrict<O extends object, K extends Key> = O[K & keyof O];
  type AtLoose<O extends object, K extends Key> = O extends unknown ? AtStrict<O, K> : never;
  export type At<O extends object, K extends Key, strict extends Boolean = 1> = {
      1: AtStrict<O, K>;
      0: AtLoose<O, K>;
  }[strict];

  export type ComputeRaw<A extends any> = A extends Function ? A : {
    [K in keyof A]: A[K];
  } & {};

  export type OptionalFlat<O> = {
    [K in keyof O]?: O[K];
  } & {};

  type _Record<K extends keyof any, T> = {
    [P in K]: T;
  };

  // cause typescript not to expand types and preserve names
  type NoExpand<T> = T extends unknown ? T : never;

  // this type assumes the passed object is entirely optional
  type AtLeast<O extends object, K extends string> = NoExpand<
    O extends unknown
    ? | (K extends keyof O ? { [P in K]: O[P] } & O : O)
      | {[P in keyof O as P extends K ? K : never]-?: O[P]} & O
    : never>;

  type _Strict<U, _U = U> = U extends unknown ? U & OptionalFlat<_Record<Exclude<Keys<_U>, keyof U>, never>> : never;

  export type Strict<U extends object> = ComputeRaw<_Strict<U>>;
  /** End Helper Types for "Merge" **/

  export type Merge<U extends object> = ComputeRaw<_Merge<Strict<U>>>;

  /**
  A [[Boolean]]
  */
  export type Boolean = True | False

  // /**
  // 1
  // */
  export type True = 1

  /**
  0
  */
  export type False = 0

  export type Not<B extends Boolean> = {
    0: 1
    1: 0
  }[B]

  export type Extends<A1 extends any, A2 extends any> = [A1] extends [never]
    ? 0 // anything `never` is false
    : A1 extends A2
    ? 1
    : 0

  export type Has<U extends Union, U1 extends Union> = Not<
    Extends<Exclude<U1, U>, U1>
  >

  export type Or<B1 extends Boolean, B2 extends Boolean> = {
    0: {
      0: 0
      1: 1
    }
    1: {
      0: 1
      1: 1
    }
  }[B1][B2]

  export type Keys<U extends Union> = U extends unknown ? keyof U : never

  type Cast<A, B> = A extends B ? A : B;

  export const type: unique symbol;



  /**
   * Used by group by
   */

  export type GetScalarType<T, O> = O extends object ? {
    [P in keyof T]: P extends keyof O
      ? O[P]
      : never
  } : never

  type FieldPaths<
    T,
    U = Omit<T, '_avg' | '_sum' | '_count' | '_min' | '_max'>
  > = IsObject<T> extends True ? U : T

  type GetHavingFields<T> = {
    [K in keyof T]: Or<
      Or<Extends<'OR', K>, Extends<'AND', K>>,
      Extends<'NOT', K>
    > extends True
      ? // infer is only needed to not hit TS limit
        // based on the brilliant idea of Pierre-Antoine Mills
        // https://github.com/microsoft/TypeScript/issues/30188#issuecomment-478938437
        T[K] extends infer TK
        ? GetHavingFields<UnEnumerate<TK> extends object ? Merge<UnEnumerate<TK>> : never>
        : never
      : {} extends FieldPaths<T[K]>
      ? never
      : K
  }[keyof T]

  /**
   * Convert tuple to union
   */
  type _TupleToUnion<T> = T extends (infer E)[] ? E : never
  type TupleToUnion<K extends readonly any[]> = _TupleToUnion<K>
  type MaybeTupleToUnion<T> = T extends any[] ? TupleToUnion<T> : T

  /**
   * Like `Pick`, but additionally can also accept an array of keys
   */
  type PickEnumerable<T, K extends Enumerable<keyof T> | keyof T> = Prisma__Pick<T, MaybeTupleToUnion<K>>

  /**
   * Exclude all keys with underscores
   */
  type ExcludeUnderscoreKeys<T extends string> = T extends `_${string}` ? never : T


  export type FieldRef<Model, FieldType> = runtime.FieldRef<Model, FieldType>

  type FieldRefInputType<Model, FieldType> = Model extends never ? never : FieldRef<Model, FieldType>


  export const ModelName: {
    TokenBuckets: 'TokenBuckets'
  };

  export type ModelName = (typeof ModelName)[keyof typeof ModelName]


  export type Datasources = {
    db?: Datasource
  }

  interface TypeMapCb extends $Utils.Fn<{extArgs: $Extensions.InternalArgs, clientOptions: PrismaClientOptions }, $Utils.Record<string, any>> {
    returns: Prisma.TypeMap<this['params']['extArgs'], this['params']['clientOptions']>
  }

  export type TypeMap<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs, ClientOptions = {}> = {
    meta: {
      modelProps: "tokenBuckets"
      txIsolationLevel: Prisma.TransactionIsolationLevel
    }
    model: {
      TokenBuckets: {
        payload: Prisma.$TokenBucketsPayload<ExtArgs>
        fields: Prisma.TokenBucketsFieldRefs
        operations: {
          findUnique: {
            args: Prisma.TokenBucketsFindUniqueArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload> | null
          }
          findUniqueOrThrow: {
            args: Prisma.TokenBucketsFindUniqueOrThrowArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>
          }
          findFirst: {
            args: Prisma.TokenBucketsFindFirstArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload> | null
          }
          findFirstOrThrow: {
            args: Prisma.TokenBucketsFindFirstOrThrowArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>
          }
          findMany: {
            args: Prisma.TokenBucketsFindManyArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>[]
          }
          create: {
            args: Prisma.TokenBucketsCreateArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>
          }
          createMany: {
            args: Prisma.TokenBucketsCreateManyArgs<ExtArgs>
            result: BatchPayload
          }
          createManyAndReturn: {
            args: Prisma.TokenBucketsCreateManyAndReturnArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>[]
          }
          delete: {
            args: Prisma.TokenBucketsDeleteArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>
          }
          update: {
            args: Prisma.TokenBucketsUpdateArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>
          }
          deleteMany: {
            args: Prisma.TokenBucketsDeleteManyArgs<ExtArgs>
            result: BatchPayload
          }
          updateMany: {
            args: Prisma.TokenBucketsUpdateManyArgs<ExtArgs>
            result: BatchPayload
          }
          upsert: {
            args: Prisma.TokenBucketsUpsertArgs<ExtArgs>
            result: $Utils.PayloadToResult<Prisma.$TokenBucketsPayload>
          }
          aggregate: {
            args: Prisma.TokenBucketsAggregateArgs<ExtArgs>
            result: $Utils.Optional<AggregateTokenBuckets>
          }
          groupBy: {
            args: Prisma.TokenBucketsGroupByArgs<ExtArgs>
            result: $Utils.Optional<TokenBucketsGroupByOutputType>[]
          }
          count: {
            args: Prisma.TokenBucketsCountArgs<ExtArgs>
            result: $Utils.Optional<TokenBucketsCountAggregateOutputType> | number
          }
        }
      }
    }
  } & {
    other: {
      payload: any
      operations: {
        $executeRawUnsafe: {
          args: [query: string, ...values: any[]],
          result: any
        }
        $executeRaw: {
          args: [query: TemplateStringsArray | Prisma.Sql, ...values: any[]],
          result: any
        }
        $queryRawUnsafe: {
          args: [query: string, ...values: any[]],
          result: any
        }
        $queryRaw: {
          args: [query: TemplateStringsArray | Prisma.Sql, ...values: any[]],
          result: any
        }
      }
    }
  }
  export const defineExtension: $Extensions.ExtendsHook<"define", Prisma.TypeMapCb, $Extensions.DefaultArgs>
  export type DefaultPrismaClient = PrismaClient
  export type ErrorFormat = 'pretty' | 'colorless' | 'minimal'
  export interface PrismaClientOptions {
    /**
     * Overwrites the datasource url from your schema.prisma file
     */
    datasources?: Datasources
    /**
     * Overwrites the datasource url from your schema.prisma file
     */
    datasourceUrl?: string
    /**
     * @default "colorless"
     */
    errorFormat?: ErrorFormat
    /**
     * @example
     * ```
     * // Defaults to stdout
     * log: ['query', 'info', 'warn', 'error']
     * 
     * // Emit as events
     * log: [
     *   { emit: 'stdout', level: 'query' },
     *   { emit: 'stdout', level: 'info' },
     *   { emit: 'stdout', level: 'warn' }
     *   { emit: 'stdout', level: 'error' }
     * ]
     * ```
     * Read more in our [docs](https://www.prisma.io/docs/reference/tools-and-interfaces/prisma-client/logging#the-log-option).
     */
    log?: (LogLevel | LogDefinition)[]
    /**
     * The default values for transactionOptions
     * maxWait ?= 2000
     * timeout ?= 5000
     */
    transactionOptions?: {
      maxWait?: number
      timeout?: number
      isolationLevel?: Prisma.TransactionIsolationLevel
    }
    /**
     * Instance of a Driver Adapter, e.g., like one provided by `@prisma/adapter-planetscale`
     */
    adapter?: runtime.DriverAdapter | null
  }


  /* Types for Logging */
  export type LogLevel = 'info' | 'query' | 'warn' | 'error'
  export type LogDefinition = {
    level: LogLevel
    emit: 'stdout' | 'event'
  }

  export type GetLogType<T extends LogLevel | LogDefinition> = T extends LogDefinition ? T['emit'] extends 'event' ? T['level'] : never : never
  export type GetEvents<T extends any> = T extends Array<LogLevel | LogDefinition> ?
    GetLogType<T[0]> | GetLogType<T[1]> | GetLogType<T[2]> | GetLogType<T[3]>
    : never

  export type QueryEvent = {
    timestamp: Date
    query: string
    params: string
    duration: number
    target: string
  }

  export type LogEvent = {
    timestamp: Date
    message: string
    target: string
  }
  /* End Types for Logging */


  export type PrismaAction =
    | 'findUnique'
    | 'findUniqueOrThrow'
    | 'findMany'
    | 'findFirst'
    | 'findFirstOrThrow'
    | 'create'
    | 'createMany'
    | 'createManyAndReturn'
    | 'update'
    | 'updateMany'
    | 'upsert'
    | 'delete'
    | 'deleteMany'
    | 'executeRaw'
    | 'queryRaw'
    | 'aggregate'
    | 'count'
    | 'runCommandRaw'
    | 'findRaw'
    | 'groupBy'

  /**
   * These options are being passed into the middleware as "params"
   */
  export type MiddlewareParams = {
    model?: ModelName
    action: PrismaAction
    args: any
    dataPath: string[]
    runInTransaction: boolean
  }

  /**
   * The `T` type makes sure, that the `return proceed` is not forgotten in the middleware implementation
   */
  export type Middleware<T = any> = (
    params: MiddlewareParams,
    next: (params: MiddlewareParams) => $Utils.JsPromise<T>,
  ) => $Utils.JsPromise<T>

  // tested in getLogLevel.test.ts
  export function getLogLevel(log: Array<LogLevel | LogDefinition>): LogLevel | undefined;

  /**
   * `PrismaClient` proxy available in interactive transactions.
   */
  export type TransactionClient = Omit<Prisma.DefaultPrismaClient, runtime.ITXClientDenyList>

  export type Datasource = {
    url?: string
  }

  /**
   * Count Types
   */



  /**
   * Models
   */

  /**
   * Model TokenBuckets
   */

  export type AggregateTokenBuckets = {
    _count: TokenBucketsCountAggregateOutputType | null
    _avg: TokenBucketsAvgAggregateOutputType | null
    _sum: TokenBucketsSumAggregateOutputType | null
    _min: TokenBucketsMinAggregateOutputType | null
    _max: TokenBucketsMaxAggregateOutputType | null
  }

  export type TokenBucketsAvgAggregateOutputType = {
    tokens: number | null
  }

  export type TokenBucketsSumAggregateOutputType = {
    tokens: bigint | null
  }

  export type TokenBucketsMinAggregateOutputType = {
    type: string | null
    key: string | null
    tokens: bigint | null
    lastRefill: Date | null
  }

  export type TokenBucketsMaxAggregateOutputType = {
    type: string | null
    key: string | null
    tokens: bigint | null
    lastRefill: Date | null
  }

  export type TokenBucketsCountAggregateOutputType = {
    type: number
    key: number
    tokens: number
    lastRefill: number
    _all: number
  }


  export type TokenBucketsAvgAggregateInputType = {
    tokens?: true
  }

  export type TokenBucketsSumAggregateInputType = {
    tokens?: true
  }

  export type TokenBucketsMinAggregateInputType = {
    type?: true
    key?: true
    tokens?: true
    lastRefill?: true
  }

  export type TokenBucketsMaxAggregateInputType = {
    type?: true
    key?: true
    tokens?: true
    lastRefill?: true
  }

  export type TokenBucketsCountAggregateInputType = {
    type?: true
    key?: true
    tokens?: true
    lastRefill?: true
    _all?: true
  }

  export type TokenBucketsAggregateArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Filter which TokenBuckets to aggregate.
     */
    where?: TokenBucketsWhereInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/sorting Sorting Docs}
     * 
     * Determine the order of TokenBuckets to fetch.
     */
    orderBy?: TokenBucketsOrderByWithRelationInput | TokenBucketsOrderByWithRelationInput[]
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination#cursor-based-pagination Cursor Docs}
     * 
     * Sets the start position
     */
    cursor?: TokenBucketsWhereUniqueInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Take `±n` TokenBuckets from the position of the cursor.
     */
    take?: number
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Skip the first `n` TokenBuckets.
     */
    skip?: number
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/aggregations Aggregation Docs}
     * 
     * Count returned TokenBuckets
    **/
    _count?: true | TokenBucketsCountAggregateInputType
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/aggregations Aggregation Docs}
     * 
     * Select which fields to average
    **/
    _avg?: TokenBucketsAvgAggregateInputType
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/aggregations Aggregation Docs}
     * 
     * Select which fields to sum
    **/
    _sum?: TokenBucketsSumAggregateInputType
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/aggregations Aggregation Docs}
     * 
     * Select which fields to find the minimum value
    **/
    _min?: TokenBucketsMinAggregateInputType
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/aggregations Aggregation Docs}
     * 
     * Select which fields to find the maximum value
    **/
    _max?: TokenBucketsMaxAggregateInputType
  }

  export type GetTokenBucketsAggregateType<T extends TokenBucketsAggregateArgs> = {
        [P in keyof T & keyof AggregateTokenBuckets]: P extends '_count' | 'count'
      ? T[P] extends true
        ? number
        : GetScalarType<T[P], AggregateTokenBuckets[P]>
      : GetScalarType<T[P], AggregateTokenBuckets[P]>
  }




  export type TokenBucketsGroupByArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    where?: TokenBucketsWhereInput
    orderBy?: TokenBucketsOrderByWithAggregationInput | TokenBucketsOrderByWithAggregationInput[]
    by: TokenBucketsScalarFieldEnum[] | TokenBucketsScalarFieldEnum
    having?: TokenBucketsScalarWhereWithAggregatesInput
    take?: number
    skip?: number
    _count?: TokenBucketsCountAggregateInputType | true
    _avg?: TokenBucketsAvgAggregateInputType
    _sum?: TokenBucketsSumAggregateInputType
    _min?: TokenBucketsMinAggregateInputType
    _max?: TokenBucketsMaxAggregateInputType
  }

  export type TokenBucketsGroupByOutputType = {
    type: string
    key: string
    tokens: bigint
    lastRefill: Date
    _count: TokenBucketsCountAggregateOutputType | null
    _avg: TokenBucketsAvgAggregateOutputType | null
    _sum: TokenBucketsSumAggregateOutputType | null
    _min: TokenBucketsMinAggregateOutputType | null
    _max: TokenBucketsMaxAggregateOutputType | null
  }

  type GetTokenBucketsGroupByPayload<T extends TokenBucketsGroupByArgs> = Prisma.PrismaPromise<
    Array<
      PickEnumerable<TokenBucketsGroupByOutputType, T['by']> &
        {
          [P in ((keyof T) & (keyof TokenBucketsGroupByOutputType))]: P extends '_count'
            ? T[P] extends boolean
              ? number
              : GetScalarType<T[P], TokenBucketsGroupByOutputType[P]>
            : GetScalarType<T[P], TokenBucketsGroupByOutputType[P]>
        }
      >
    >


  export type TokenBucketsSelect<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = $Extensions.GetSelect<{
    type?: boolean
    key?: boolean
    tokens?: boolean
    lastRefill?: boolean
  }, ExtArgs["result"]["tokenBuckets"]>

  export type TokenBucketsSelectCreateManyAndReturn<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = $Extensions.GetSelect<{
    type?: boolean
    key?: boolean
    tokens?: boolean
    lastRefill?: boolean
  }, ExtArgs["result"]["tokenBuckets"]>

  export type TokenBucketsSelectScalar = {
    type?: boolean
    key?: boolean
    tokens?: boolean
    lastRefill?: boolean
  }


  export type $TokenBucketsPayload<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    name: "TokenBuckets"
    objects: {}
    scalars: $Extensions.GetPayloadResult<{
      type: string
      key: string
      tokens: bigint
      lastRefill: Date
    }, ExtArgs["result"]["tokenBuckets"]>
    composites: {}
  }

  type TokenBucketsGetPayload<S extends boolean | null | undefined | TokenBucketsDefaultArgs> = $Result.GetResult<Prisma.$TokenBucketsPayload, S>

  type TokenBucketsCountArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = 
    Omit<TokenBucketsFindManyArgs, 'select' | 'include' | 'distinct'> & {
      select?: TokenBucketsCountAggregateInputType | true
    }

  export interface TokenBucketsDelegate<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> {
    [K: symbol]: { types: Prisma.TypeMap<ExtArgs>['model']['TokenBuckets'], meta: { name: 'TokenBuckets' } }
    /**
     * Find zero or one TokenBuckets that matches the filter.
     * @param {TokenBucketsFindUniqueArgs} args - Arguments to find a TokenBuckets
     * @example
     * // Get one TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.findUnique({
     *   where: {
     *     // ... provide filter here
     *   }
     * })
     */
    findUnique<T extends TokenBucketsFindUniqueArgs>(args: SelectSubset<T, TokenBucketsFindUniqueArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "findUnique"> | null, null, ExtArgs>

    /**
     * Find one TokenBuckets that matches the filter or throw an error with `error.code='P2025'` 
     * if no matches were found.
     * @param {TokenBucketsFindUniqueOrThrowArgs} args - Arguments to find a TokenBuckets
     * @example
     * // Get one TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.findUniqueOrThrow({
     *   where: {
     *     // ... provide filter here
     *   }
     * })
     */
    findUniqueOrThrow<T extends TokenBucketsFindUniqueOrThrowArgs>(args: SelectSubset<T, TokenBucketsFindUniqueOrThrowArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "findUniqueOrThrow">, never, ExtArgs>

    /**
     * Find the first TokenBuckets that matches the filter.
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * @param {TokenBucketsFindFirstArgs} args - Arguments to find a TokenBuckets
     * @example
     * // Get one TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.findFirst({
     *   where: {
     *     // ... provide filter here
     *   }
     * })
     */
    findFirst<T extends TokenBucketsFindFirstArgs>(args?: SelectSubset<T, TokenBucketsFindFirstArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "findFirst"> | null, null, ExtArgs>

    /**
     * Find the first TokenBuckets that matches the filter or
     * throw `PrismaKnownClientError` with `P2025` code if no matches were found.
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * @param {TokenBucketsFindFirstOrThrowArgs} args - Arguments to find a TokenBuckets
     * @example
     * // Get one TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.findFirstOrThrow({
     *   where: {
     *     // ... provide filter here
     *   }
     * })
     */
    findFirstOrThrow<T extends TokenBucketsFindFirstOrThrowArgs>(args?: SelectSubset<T, TokenBucketsFindFirstOrThrowArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "findFirstOrThrow">, never, ExtArgs>

    /**
     * Find zero or more TokenBuckets that matches the filter.
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * @param {TokenBucketsFindManyArgs} args - Arguments to filter and select certain fields only.
     * @example
     * // Get all TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.findMany()
     * 
     * // Get first 10 TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.findMany({ take: 10 })
     * 
     * // Only select the `type`
     * const tokenBucketsWithTypeOnly = await prisma.tokenBuckets.findMany({ select: { type: true } })
     * 
     */
    findMany<T extends TokenBucketsFindManyArgs>(args?: SelectSubset<T, TokenBucketsFindManyArgs<ExtArgs>>): Prisma.PrismaPromise<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "findMany">>

    /**
     * Create a TokenBuckets.
     * @param {TokenBucketsCreateArgs} args - Arguments to create a TokenBuckets.
     * @example
     * // Create one TokenBuckets
     * const TokenBuckets = await prisma.tokenBuckets.create({
     *   data: {
     *     // ... data to create a TokenBuckets
     *   }
     * })
     * 
     */
    create<T extends TokenBucketsCreateArgs>(args: SelectSubset<T, TokenBucketsCreateArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "create">, never, ExtArgs>

    /**
     * Create many TokenBuckets.
     * @param {TokenBucketsCreateManyArgs} args - Arguments to create many TokenBuckets.
     * @example
     * // Create many TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.createMany({
     *   data: [
     *     // ... provide data here
     *   ]
     * })
     *     
     */
    createMany<T extends TokenBucketsCreateManyArgs>(args?: SelectSubset<T, TokenBucketsCreateManyArgs<ExtArgs>>): Prisma.PrismaPromise<BatchPayload>

    /**
     * Create many TokenBuckets and returns the data saved in the database.
     * @param {TokenBucketsCreateManyAndReturnArgs} args - Arguments to create many TokenBuckets.
     * @example
     * // Create many TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.createManyAndReturn({
     *   data: [
     *     // ... provide data here
     *   ]
     * })
     * 
     * // Create many TokenBuckets and only return the `type`
     * const tokenBucketsWithTypeOnly = await prisma.tokenBuckets.createManyAndReturn({ 
     *   select: { type: true },
     *   data: [
     *     // ... provide data here
     *   ]
     * })
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * 
     */
    createManyAndReturn<T extends TokenBucketsCreateManyAndReturnArgs>(args?: SelectSubset<T, TokenBucketsCreateManyAndReturnArgs<ExtArgs>>): Prisma.PrismaPromise<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "createManyAndReturn">>

    /**
     * Delete a TokenBuckets.
     * @param {TokenBucketsDeleteArgs} args - Arguments to delete one TokenBuckets.
     * @example
     * // Delete one TokenBuckets
     * const TokenBuckets = await prisma.tokenBuckets.delete({
     *   where: {
     *     // ... filter to delete one TokenBuckets
     *   }
     * })
     * 
     */
    delete<T extends TokenBucketsDeleteArgs>(args: SelectSubset<T, TokenBucketsDeleteArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "delete">, never, ExtArgs>

    /**
     * Update one TokenBuckets.
     * @param {TokenBucketsUpdateArgs} args - Arguments to update one TokenBuckets.
     * @example
     * // Update one TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.update({
     *   where: {
     *     // ... provide filter here
     *   },
     *   data: {
     *     // ... provide data here
     *   }
     * })
     * 
     */
    update<T extends TokenBucketsUpdateArgs>(args: SelectSubset<T, TokenBucketsUpdateArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "update">, never, ExtArgs>

    /**
     * Delete zero or more TokenBuckets.
     * @param {TokenBucketsDeleteManyArgs} args - Arguments to filter TokenBuckets to delete.
     * @example
     * // Delete a few TokenBuckets
     * const { count } = await prisma.tokenBuckets.deleteMany({
     *   where: {
     *     // ... provide filter here
     *   }
     * })
     * 
     */
    deleteMany<T extends TokenBucketsDeleteManyArgs>(args?: SelectSubset<T, TokenBucketsDeleteManyArgs<ExtArgs>>): Prisma.PrismaPromise<BatchPayload>

    /**
     * Update zero or more TokenBuckets.
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * @param {TokenBucketsUpdateManyArgs} args - Arguments to update one or more rows.
     * @example
     * // Update many TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.updateMany({
     *   where: {
     *     // ... provide filter here
     *   },
     *   data: {
     *     // ... provide data here
     *   }
     * })
     * 
     */
    updateMany<T extends TokenBucketsUpdateManyArgs>(args: SelectSubset<T, TokenBucketsUpdateManyArgs<ExtArgs>>): Prisma.PrismaPromise<BatchPayload>

    /**
     * Create or update one TokenBuckets.
     * @param {TokenBucketsUpsertArgs} args - Arguments to update or create a TokenBuckets.
     * @example
     * // Update or create a TokenBuckets
     * const tokenBuckets = await prisma.tokenBuckets.upsert({
     *   create: {
     *     // ... data to create a TokenBuckets
     *   },
     *   update: {
     *     // ... in case it already exists, update
     *   },
     *   where: {
     *     // ... the filter for the TokenBuckets we want to update
     *   }
     * })
     */
    upsert<T extends TokenBucketsUpsertArgs>(args: SelectSubset<T, TokenBucketsUpsertArgs<ExtArgs>>): Prisma__TokenBucketsClient<$Result.GetResult<Prisma.$TokenBucketsPayload<ExtArgs>, T, "upsert">, never, ExtArgs>


    /**
     * Count the number of TokenBuckets.
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * @param {TokenBucketsCountArgs} args - Arguments to filter TokenBuckets to count.
     * @example
     * // Count the number of TokenBuckets
     * const count = await prisma.tokenBuckets.count({
     *   where: {
     *     // ... the filter for the TokenBuckets we want to count
     *   }
     * })
    **/
    count<T extends TokenBucketsCountArgs>(
      args?: Subset<T, TokenBucketsCountArgs>,
    ): Prisma.PrismaPromise<
      T extends $Utils.Record<'select', any>
        ? T['select'] extends true
          ? number
          : GetScalarType<T['select'], TokenBucketsCountAggregateOutputType>
        : number
    >

    /**
     * Allows you to perform aggregations operations on a TokenBuckets.
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * @param {TokenBucketsAggregateArgs} args - Select which aggregations you would like to apply and on what fields.
     * @example
     * // Ordered by age ascending
     * // Where email contains prisma.io
     * // Limited to the 10 users
     * const aggregations = await prisma.user.aggregate({
     *   _avg: {
     *     age: true,
     *   },
     *   where: {
     *     email: {
     *       contains: "prisma.io",
     *     },
     *   },
     *   orderBy: {
     *     age: "asc",
     *   },
     *   take: 10,
     * })
    **/
    aggregate<T extends TokenBucketsAggregateArgs>(args: Subset<T, TokenBucketsAggregateArgs>): Prisma.PrismaPromise<GetTokenBucketsAggregateType<T>>

    /**
     * Group by TokenBuckets.
     * Note, that providing `undefined` is treated as the value not being there.
     * Read more here: https://pris.ly/d/null-undefined
     * @param {TokenBucketsGroupByArgs} args - Group by arguments.
     * @example
     * // Group by city, order by createdAt, get count
     * const result = await prisma.user.groupBy({
     *   by: ['city', 'createdAt'],
     *   orderBy: {
     *     createdAt: true
     *   },
     *   _count: {
     *     _all: true
     *   },
     * })
     * 
    **/
    groupBy<
      T extends TokenBucketsGroupByArgs,
      HasSelectOrTake extends Or<
        Extends<'skip', Keys<T>>,
        Extends<'take', Keys<T>>
      >,
      OrderByArg extends True extends HasSelectOrTake
        ? { orderBy: TokenBucketsGroupByArgs['orderBy'] }
        : { orderBy?: TokenBucketsGroupByArgs['orderBy'] },
      OrderFields extends ExcludeUnderscoreKeys<Keys<MaybeTupleToUnion<T['orderBy']>>>,
      ByFields extends MaybeTupleToUnion<T['by']>,
      ByValid extends Has<ByFields, OrderFields>,
      HavingFields extends GetHavingFields<T['having']>,
      HavingValid extends Has<ByFields, HavingFields>,
      ByEmpty extends T['by'] extends never[] ? True : False,
      InputErrors extends ByEmpty extends True
      ? `Error: "by" must not be empty.`
      : HavingValid extends False
      ? {
          [P in HavingFields]: P extends ByFields
            ? never
            : P extends string
            ? `Error: Field "${P}" used in "having" needs to be provided in "by".`
            : [
                Error,
                'Field ',
                P,
                ` in "having" needs to be provided in "by"`,
              ]
        }[HavingFields]
      : 'take' extends Keys<T>
      ? 'orderBy' extends Keys<T>
        ? ByValid extends True
          ? {}
          : {
              [P in OrderFields]: P extends ByFields
                ? never
                : `Error: Field "${P}" in "orderBy" needs to be provided in "by"`
            }[OrderFields]
        : 'Error: If you provide "take", you also need to provide "orderBy"'
      : 'skip' extends Keys<T>
      ? 'orderBy' extends Keys<T>
        ? ByValid extends True
          ? {}
          : {
              [P in OrderFields]: P extends ByFields
                ? never
                : `Error: Field "${P}" in "orderBy" needs to be provided in "by"`
            }[OrderFields]
        : 'Error: If you provide "skip", you also need to provide "orderBy"'
      : ByValid extends True
      ? {}
      : {
          [P in OrderFields]: P extends ByFields
            ? never
            : `Error: Field "${P}" in "orderBy" needs to be provided in "by"`
        }[OrderFields]
    >(args: SubsetIntersection<T, TokenBucketsGroupByArgs, OrderByArg> & InputErrors): {} extends InputErrors ? GetTokenBucketsGroupByPayload<T> : Prisma.PrismaPromise<InputErrors>
  /**
   * Fields of the TokenBuckets model
   */
  readonly fields: TokenBucketsFieldRefs;
  }

  /**
   * The delegate class that acts as a "Promise-like" for TokenBuckets.
   * Why is this prefixed with `Prisma__`?
   * Because we want to prevent naming conflicts as mentioned in
   * https://github.com/prisma/prisma-client-js/issues/707
   */
  export interface Prisma__TokenBucketsClient<T, Null = never, ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> extends Prisma.PrismaPromise<T> {
    readonly [Symbol.toStringTag]: "PrismaPromise"
    /**
     * Attaches callbacks for the resolution and/or rejection of the Promise.
     * @param onfulfilled The callback to execute when the Promise is resolved.
     * @param onrejected The callback to execute when the Promise is rejected.
     * @returns A Promise for the completion of which ever callback is executed.
     */
    then<TResult1 = T, TResult2 = never>(onfulfilled?: ((value: T) => TResult1 | PromiseLike<TResult1>) | undefined | null, onrejected?: ((reason: any) => TResult2 | PromiseLike<TResult2>) | undefined | null): $Utils.JsPromise<TResult1 | TResult2>
    /**
     * Attaches a callback for only the rejection of the Promise.
     * @param onrejected The callback to execute when the Promise is rejected.
     * @returns A Promise for the completion of the callback.
     */
    catch<TResult = never>(onrejected?: ((reason: any) => TResult | PromiseLike<TResult>) | undefined | null): $Utils.JsPromise<T | TResult>
    /**
     * Attaches a callback that is invoked when the Promise is settled (fulfilled or rejected). The
     * resolved value cannot be modified from the callback.
     * @param onfinally The callback to execute when the Promise is settled (fulfilled or rejected).
     * @returns A Promise for the completion of the callback.
     */
    finally(onfinally?: (() => void) | undefined | null): $Utils.JsPromise<T>
  }




  /**
   * Fields of the TokenBuckets model
   */ 
  interface TokenBucketsFieldRefs {
    readonly type: FieldRef<"TokenBuckets", 'String'>
    readonly key: FieldRef<"TokenBuckets", 'String'>
    readonly tokens: FieldRef<"TokenBuckets", 'BigInt'>
    readonly lastRefill: FieldRef<"TokenBuckets", 'DateTime'>
  }
    

  // Custom InputTypes
  /**
   * TokenBuckets findUnique
   */
  export type TokenBucketsFindUniqueArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * Filter, which TokenBuckets to fetch.
     */
    where: TokenBucketsWhereUniqueInput
  }

  /**
   * TokenBuckets findUniqueOrThrow
   */
  export type TokenBucketsFindUniqueOrThrowArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * Filter, which TokenBuckets to fetch.
     */
    where: TokenBucketsWhereUniqueInput
  }

  /**
   * TokenBuckets findFirst
   */
  export type TokenBucketsFindFirstArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * Filter, which TokenBuckets to fetch.
     */
    where?: TokenBucketsWhereInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/sorting Sorting Docs}
     * 
     * Determine the order of TokenBuckets to fetch.
     */
    orderBy?: TokenBucketsOrderByWithRelationInput | TokenBucketsOrderByWithRelationInput[]
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination#cursor-based-pagination Cursor Docs}
     * 
     * Sets the position for searching for TokenBuckets.
     */
    cursor?: TokenBucketsWhereUniqueInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Take `±n` TokenBuckets from the position of the cursor.
     */
    take?: number
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Skip the first `n` TokenBuckets.
     */
    skip?: number
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/distinct Distinct Docs}
     * 
     * Filter by unique combinations of TokenBuckets.
     */
    distinct?: TokenBucketsScalarFieldEnum | TokenBucketsScalarFieldEnum[]
  }

  /**
   * TokenBuckets findFirstOrThrow
   */
  export type TokenBucketsFindFirstOrThrowArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * Filter, which TokenBuckets to fetch.
     */
    where?: TokenBucketsWhereInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/sorting Sorting Docs}
     * 
     * Determine the order of TokenBuckets to fetch.
     */
    orderBy?: TokenBucketsOrderByWithRelationInput | TokenBucketsOrderByWithRelationInput[]
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination#cursor-based-pagination Cursor Docs}
     * 
     * Sets the position for searching for TokenBuckets.
     */
    cursor?: TokenBucketsWhereUniqueInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Take `±n` TokenBuckets from the position of the cursor.
     */
    take?: number
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Skip the first `n` TokenBuckets.
     */
    skip?: number
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/distinct Distinct Docs}
     * 
     * Filter by unique combinations of TokenBuckets.
     */
    distinct?: TokenBucketsScalarFieldEnum | TokenBucketsScalarFieldEnum[]
  }

  /**
   * TokenBuckets findMany
   */
  export type TokenBucketsFindManyArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * Filter, which TokenBuckets to fetch.
     */
    where?: TokenBucketsWhereInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/sorting Sorting Docs}
     * 
     * Determine the order of TokenBuckets to fetch.
     */
    orderBy?: TokenBucketsOrderByWithRelationInput | TokenBucketsOrderByWithRelationInput[]
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination#cursor-based-pagination Cursor Docs}
     * 
     * Sets the position for listing TokenBuckets.
     */
    cursor?: TokenBucketsWhereUniqueInput
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Take `±n` TokenBuckets from the position of the cursor.
     */
    take?: number
    /**
     * {@link https://www.prisma.io/docs/concepts/components/prisma-client/pagination Pagination Docs}
     * 
     * Skip the first `n` TokenBuckets.
     */
    skip?: number
    distinct?: TokenBucketsScalarFieldEnum | TokenBucketsScalarFieldEnum[]
  }

  /**
   * TokenBuckets create
   */
  export type TokenBucketsCreateArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * The data needed to create a TokenBuckets.
     */
    data: XOR<TokenBucketsCreateInput, TokenBucketsUncheckedCreateInput>
  }

  /**
   * TokenBuckets createMany
   */
  export type TokenBucketsCreateManyArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * The data used to create many TokenBuckets.
     */
    data: TokenBucketsCreateManyInput | TokenBucketsCreateManyInput[]
    skipDuplicates?: boolean
  }

  /**
   * TokenBuckets createManyAndReturn
   */
  export type TokenBucketsCreateManyAndReturnArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelectCreateManyAndReturn<ExtArgs> | null
    /**
     * The data used to create many TokenBuckets.
     */
    data: TokenBucketsCreateManyInput | TokenBucketsCreateManyInput[]
    skipDuplicates?: boolean
  }

  /**
   * TokenBuckets update
   */
  export type TokenBucketsUpdateArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * The data needed to update a TokenBuckets.
     */
    data: XOR<TokenBucketsUpdateInput, TokenBucketsUncheckedUpdateInput>
    /**
     * Choose, which TokenBuckets to update.
     */
    where: TokenBucketsWhereUniqueInput
  }

  /**
   * TokenBuckets updateMany
   */
  export type TokenBucketsUpdateManyArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * The data used to update TokenBuckets.
     */
    data: XOR<TokenBucketsUpdateManyMutationInput, TokenBucketsUncheckedUpdateManyInput>
    /**
     * Filter which TokenBuckets to update
     */
    where?: TokenBucketsWhereInput
  }

  /**
   * TokenBuckets upsert
   */
  export type TokenBucketsUpsertArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * The filter to search for the TokenBuckets to update in case it exists.
     */
    where: TokenBucketsWhereUniqueInput
    /**
     * In case the TokenBuckets found by the `where` argument doesn't exist, create a new TokenBuckets with this data.
     */
    create: XOR<TokenBucketsCreateInput, TokenBucketsUncheckedCreateInput>
    /**
     * In case the TokenBuckets was found with the provided `where` argument, update it with this data.
     */
    update: XOR<TokenBucketsUpdateInput, TokenBucketsUncheckedUpdateInput>
  }

  /**
   * TokenBuckets delete
   */
  export type TokenBucketsDeleteArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
    /**
     * Filter which TokenBuckets to delete.
     */
    where: TokenBucketsWhereUniqueInput
  }

  /**
   * TokenBuckets deleteMany
   */
  export type TokenBucketsDeleteManyArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Filter which TokenBuckets to delete
     */
    where?: TokenBucketsWhereInput
  }

  /**
   * TokenBuckets without action
   */
  export type TokenBucketsDefaultArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = {
    /**
     * Select specific fields to fetch from the TokenBuckets
     */
    select?: TokenBucketsSelect<ExtArgs> | null
  }


  /**
   * Enums
   */

  export const TransactionIsolationLevel: {
    ReadUncommitted: 'ReadUncommitted',
    ReadCommitted: 'ReadCommitted',
    RepeatableRead: 'RepeatableRead',
    Serializable: 'Serializable'
  };

  export type TransactionIsolationLevel = (typeof TransactionIsolationLevel)[keyof typeof TransactionIsolationLevel]


  export const TokenBucketsScalarFieldEnum: {
    type: 'type',
    key: 'key',
    tokens: 'tokens',
    lastRefill: 'lastRefill'
  };

  export type TokenBucketsScalarFieldEnum = (typeof TokenBucketsScalarFieldEnum)[keyof typeof TokenBucketsScalarFieldEnum]


  export const SortOrder: {
    asc: 'asc',
    desc: 'desc'
  };

  export type SortOrder = (typeof SortOrder)[keyof typeof SortOrder]


  export const QueryMode: {
    default: 'default',
    insensitive: 'insensitive'
  };

  export type QueryMode = (typeof QueryMode)[keyof typeof QueryMode]


  /**
   * Field references 
   */


  /**
   * Reference to a field of type 'String'
   */
  export type StringFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'String'>
    


  /**
   * Reference to a field of type 'String[]'
   */
  export type ListStringFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'String[]'>
    


  /**
   * Reference to a field of type 'BigInt'
   */
  export type BigIntFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'BigInt'>
    


  /**
   * Reference to a field of type 'BigInt[]'
   */
  export type ListBigIntFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'BigInt[]'>
    


  /**
   * Reference to a field of type 'DateTime'
   */
  export type DateTimeFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'DateTime'>
    


  /**
   * Reference to a field of type 'DateTime[]'
   */
  export type ListDateTimeFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'DateTime[]'>
    


  /**
   * Reference to a field of type 'Int'
   */
  export type IntFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'Int'>
    


  /**
   * Reference to a field of type 'Int[]'
   */
  export type ListIntFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'Int[]'>
    


  /**
   * Reference to a field of type 'Float'
   */
  export type FloatFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'Float'>
    


  /**
   * Reference to a field of type 'Float[]'
   */
  export type ListFloatFieldRefInput<$PrismaModel> = FieldRefInputType<$PrismaModel, 'Float[]'>
    
  /**
   * Deep Input Types
   */


  export type TokenBucketsWhereInput = {
    AND?: TokenBucketsWhereInput | TokenBucketsWhereInput[]
    OR?: TokenBucketsWhereInput[]
    NOT?: TokenBucketsWhereInput | TokenBucketsWhereInput[]
    type?: StringFilter<"TokenBuckets"> | string
    key?: StringFilter<"TokenBuckets"> | string
    tokens?: BigIntFilter<"TokenBuckets"> | bigint | number
    lastRefill?: DateTimeFilter<"TokenBuckets"> | Date | string
  }

  export type TokenBucketsOrderByWithRelationInput = {
    type?: SortOrder
    key?: SortOrder
    tokens?: SortOrder
    lastRefill?: SortOrder
  }

  export type TokenBucketsWhereUniqueInput = Prisma.AtLeast<{
    type_key?: TokenBucketsTypeKeyCompoundUniqueInput
    AND?: TokenBucketsWhereInput | TokenBucketsWhereInput[]
    OR?: TokenBucketsWhereInput[]
    NOT?: TokenBucketsWhereInput | TokenBucketsWhereInput[]
    type?: StringFilter<"TokenBuckets"> | string
    key?: StringFilter<"TokenBuckets"> | string
    tokens?: BigIntFilter<"TokenBuckets"> | bigint | number
    lastRefill?: DateTimeFilter<"TokenBuckets"> | Date | string
  }, "type_key">

  export type TokenBucketsOrderByWithAggregationInput = {
    type?: SortOrder
    key?: SortOrder
    tokens?: SortOrder
    lastRefill?: SortOrder
    _count?: TokenBucketsCountOrderByAggregateInput
    _avg?: TokenBucketsAvgOrderByAggregateInput
    _max?: TokenBucketsMaxOrderByAggregateInput
    _min?: TokenBucketsMinOrderByAggregateInput
    _sum?: TokenBucketsSumOrderByAggregateInput
  }

  export type TokenBucketsScalarWhereWithAggregatesInput = {
    AND?: TokenBucketsScalarWhereWithAggregatesInput | TokenBucketsScalarWhereWithAggregatesInput[]
    OR?: TokenBucketsScalarWhereWithAggregatesInput[]
    NOT?: TokenBucketsScalarWhereWithAggregatesInput | TokenBucketsScalarWhereWithAggregatesInput[]
    type?: StringWithAggregatesFilter<"TokenBuckets"> | string
    key?: StringWithAggregatesFilter<"TokenBuckets"> | string
    tokens?: BigIntWithAggregatesFilter<"TokenBuckets"> | bigint | number
    lastRefill?: DateTimeWithAggregatesFilter<"TokenBuckets"> | Date | string
  }

  export type TokenBucketsCreateInput = {
    type: string
    key: string
    tokens: bigint | number
    lastRefill: Date | string
  }

  export type TokenBucketsUncheckedCreateInput = {
    type: string
    key: string
    tokens: bigint | number
    lastRefill: Date | string
  }

  export type TokenBucketsUpdateInput = {
    type?: StringFieldUpdateOperationsInput | string
    key?: StringFieldUpdateOperationsInput | string
    tokens?: BigIntFieldUpdateOperationsInput | bigint | number
    lastRefill?: DateTimeFieldUpdateOperationsInput | Date | string
  }

  export type TokenBucketsUncheckedUpdateInput = {
    type?: StringFieldUpdateOperationsInput | string
    key?: StringFieldUpdateOperationsInput | string
    tokens?: BigIntFieldUpdateOperationsInput | bigint | number
    lastRefill?: DateTimeFieldUpdateOperationsInput | Date | string
  }

  export type TokenBucketsCreateManyInput = {
    type: string
    key: string
    tokens: bigint | number
    lastRefill: Date | string
  }

  export type TokenBucketsUpdateManyMutationInput = {
    type?: StringFieldUpdateOperationsInput | string
    key?: StringFieldUpdateOperationsInput | string
    tokens?: BigIntFieldUpdateOperationsInput | bigint | number
    lastRefill?: DateTimeFieldUpdateOperationsInput | Date | string
  }

  export type TokenBucketsUncheckedUpdateManyInput = {
    type?: StringFieldUpdateOperationsInput | string
    key?: StringFieldUpdateOperationsInput | string
    tokens?: BigIntFieldUpdateOperationsInput | bigint | number
    lastRefill?: DateTimeFieldUpdateOperationsInput | Date | string
  }

  export type StringFilter<$PrismaModel = never> = {
    equals?: string | StringFieldRefInput<$PrismaModel>
    in?: string[] | ListStringFieldRefInput<$PrismaModel>
    notIn?: string[] | ListStringFieldRefInput<$PrismaModel>
    lt?: string | StringFieldRefInput<$PrismaModel>
    lte?: string | StringFieldRefInput<$PrismaModel>
    gt?: string | StringFieldRefInput<$PrismaModel>
    gte?: string | StringFieldRefInput<$PrismaModel>
    contains?: string | StringFieldRefInput<$PrismaModel>
    startsWith?: string | StringFieldRefInput<$PrismaModel>
    endsWith?: string | StringFieldRefInput<$PrismaModel>
    mode?: QueryMode
    not?: NestedStringFilter<$PrismaModel> | string
  }

  export type BigIntFilter<$PrismaModel = never> = {
    equals?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    in?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    notIn?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    lt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    lte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    not?: NestedBigIntFilter<$PrismaModel> | bigint | number
  }

  export type DateTimeFilter<$PrismaModel = never> = {
    equals?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    in?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    notIn?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    lt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    lte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    not?: NestedDateTimeFilter<$PrismaModel> | Date | string
  }

  export type TokenBucketsTypeKeyCompoundUniqueInput = {
    type: string
    key: string
  }

  export type TokenBucketsCountOrderByAggregateInput = {
    type?: SortOrder
    key?: SortOrder
    tokens?: SortOrder
    lastRefill?: SortOrder
  }

  export type TokenBucketsAvgOrderByAggregateInput = {
    tokens?: SortOrder
  }

  export type TokenBucketsMaxOrderByAggregateInput = {
    type?: SortOrder
    key?: SortOrder
    tokens?: SortOrder
    lastRefill?: SortOrder
  }

  export type TokenBucketsMinOrderByAggregateInput = {
    type?: SortOrder
    key?: SortOrder
    tokens?: SortOrder
    lastRefill?: SortOrder
  }

  export type TokenBucketsSumOrderByAggregateInput = {
    tokens?: SortOrder
  }

  export type StringWithAggregatesFilter<$PrismaModel = never> = {
    equals?: string | StringFieldRefInput<$PrismaModel>
    in?: string[] | ListStringFieldRefInput<$PrismaModel>
    notIn?: string[] | ListStringFieldRefInput<$PrismaModel>
    lt?: string | StringFieldRefInput<$PrismaModel>
    lte?: string | StringFieldRefInput<$PrismaModel>
    gt?: string | StringFieldRefInput<$PrismaModel>
    gte?: string | StringFieldRefInput<$PrismaModel>
    contains?: string | StringFieldRefInput<$PrismaModel>
    startsWith?: string | StringFieldRefInput<$PrismaModel>
    endsWith?: string | StringFieldRefInput<$PrismaModel>
    mode?: QueryMode
    not?: NestedStringWithAggregatesFilter<$PrismaModel> | string
    _count?: NestedIntFilter<$PrismaModel>
    _min?: NestedStringFilter<$PrismaModel>
    _max?: NestedStringFilter<$PrismaModel>
  }

  export type BigIntWithAggregatesFilter<$PrismaModel = never> = {
    equals?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    in?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    notIn?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    lt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    lte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    not?: NestedBigIntWithAggregatesFilter<$PrismaModel> | bigint | number
    _count?: NestedIntFilter<$PrismaModel>
    _avg?: NestedFloatFilter<$PrismaModel>
    _sum?: NestedBigIntFilter<$PrismaModel>
    _min?: NestedBigIntFilter<$PrismaModel>
    _max?: NestedBigIntFilter<$PrismaModel>
  }

  export type DateTimeWithAggregatesFilter<$PrismaModel = never> = {
    equals?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    in?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    notIn?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    lt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    lte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    not?: NestedDateTimeWithAggregatesFilter<$PrismaModel> | Date | string
    _count?: NestedIntFilter<$PrismaModel>
    _min?: NestedDateTimeFilter<$PrismaModel>
    _max?: NestedDateTimeFilter<$PrismaModel>
  }

  export type StringFieldUpdateOperationsInput = {
    set?: string
  }

  export type BigIntFieldUpdateOperationsInput = {
    set?: bigint | number
    increment?: bigint | number
    decrement?: bigint | number
    multiply?: bigint | number
    divide?: bigint | number
  }

  export type DateTimeFieldUpdateOperationsInput = {
    set?: Date | string
  }

  export type NestedStringFilter<$PrismaModel = never> = {
    equals?: string | StringFieldRefInput<$PrismaModel>
    in?: string[] | ListStringFieldRefInput<$PrismaModel>
    notIn?: string[] | ListStringFieldRefInput<$PrismaModel>
    lt?: string | StringFieldRefInput<$PrismaModel>
    lte?: string | StringFieldRefInput<$PrismaModel>
    gt?: string | StringFieldRefInput<$PrismaModel>
    gte?: string | StringFieldRefInput<$PrismaModel>
    contains?: string | StringFieldRefInput<$PrismaModel>
    startsWith?: string | StringFieldRefInput<$PrismaModel>
    endsWith?: string | StringFieldRefInput<$PrismaModel>
    not?: NestedStringFilter<$PrismaModel> | string
  }

  export type NestedBigIntFilter<$PrismaModel = never> = {
    equals?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    in?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    notIn?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    lt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    lte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    not?: NestedBigIntFilter<$PrismaModel> | bigint | number
  }

  export type NestedDateTimeFilter<$PrismaModel = never> = {
    equals?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    in?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    notIn?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    lt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    lte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    not?: NestedDateTimeFilter<$PrismaModel> | Date | string
  }

  export type NestedStringWithAggregatesFilter<$PrismaModel = never> = {
    equals?: string | StringFieldRefInput<$PrismaModel>
    in?: string[] | ListStringFieldRefInput<$PrismaModel>
    notIn?: string[] | ListStringFieldRefInput<$PrismaModel>
    lt?: string | StringFieldRefInput<$PrismaModel>
    lte?: string | StringFieldRefInput<$PrismaModel>
    gt?: string | StringFieldRefInput<$PrismaModel>
    gte?: string | StringFieldRefInput<$PrismaModel>
    contains?: string | StringFieldRefInput<$PrismaModel>
    startsWith?: string | StringFieldRefInput<$PrismaModel>
    endsWith?: string | StringFieldRefInput<$PrismaModel>
    not?: NestedStringWithAggregatesFilter<$PrismaModel> | string
    _count?: NestedIntFilter<$PrismaModel>
    _min?: NestedStringFilter<$PrismaModel>
    _max?: NestedStringFilter<$PrismaModel>
  }

  export type NestedIntFilter<$PrismaModel = never> = {
    equals?: number | IntFieldRefInput<$PrismaModel>
    in?: number[] | ListIntFieldRefInput<$PrismaModel>
    notIn?: number[] | ListIntFieldRefInput<$PrismaModel>
    lt?: number | IntFieldRefInput<$PrismaModel>
    lte?: number | IntFieldRefInput<$PrismaModel>
    gt?: number | IntFieldRefInput<$PrismaModel>
    gte?: number | IntFieldRefInput<$PrismaModel>
    not?: NestedIntFilter<$PrismaModel> | number
  }

  export type NestedBigIntWithAggregatesFilter<$PrismaModel = never> = {
    equals?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    in?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    notIn?: bigint[] | number[] | ListBigIntFieldRefInput<$PrismaModel>
    lt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    lte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gt?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    gte?: bigint | number | BigIntFieldRefInput<$PrismaModel>
    not?: NestedBigIntWithAggregatesFilter<$PrismaModel> | bigint | number
    _count?: NestedIntFilter<$PrismaModel>
    _avg?: NestedFloatFilter<$PrismaModel>
    _sum?: NestedBigIntFilter<$PrismaModel>
    _min?: NestedBigIntFilter<$PrismaModel>
    _max?: NestedBigIntFilter<$PrismaModel>
  }

  export type NestedFloatFilter<$PrismaModel = never> = {
    equals?: number | FloatFieldRefInput<$PrismaModel>
    in?: number[] | ListFloatFieldRefInput<$PrismaModel>
    notIn?: number[] | ListFloatFieldRefInput<$PrismaModel>
    lt?: number | FloatFieldRefInput<$PrismaModel>
    lte?: number | FloatFieldRefInput<$PrismaModel>
    gt?: number | FloatFieldRefInput<$PrismaModel>
    gte?: number | FloatFieldRefInput<$PrismaModel>
    not?: NestedFloatFilter<$PrismaModel> | number
  }

  export type NestedDateTimeWithAggregatesFilter<$PrismaModel = never> = {
    equals?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    in?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    notIn?: Date[] | string[] | ListDateTimeFieldRefInput<$PrismaModel>
    lt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    lte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gt?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    gte?: Date | string | DateTimeFieldRefInput<$PrismaModel>
    not?: NestedDateTimeWithAggregatesFilter<$PrismaModel> | Date | string
    _count?: NestedIntFilter<$PrismaModel>
    _min?: NestedDateTimeFilter<$PrismaModel>
    _max?: NestedDateTimeFilter<$PrismaModel>
  }



  /**
   * Aliases for legacy arg types
   */
    /**
     * @deprecated Use TokenBucketsDefaultArgs instead
     */
    export type TokenBucketsArgs<ExtArgs extends $Extensions.InternalArgs = $Extensions.DefaultArgs> = TokenBucketsDefaultArgs<ExtArgs>

  /**
   * Batch Payload for updateMany & deleteMany & createMany
   */

  export type BatchPayload = {
    count: number
  }

  /**
   * DMMF
   */
  export const dmmf: runtime.BaseDMMF
}