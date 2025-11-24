/// <reference types="vite/client" />
/// <reference types="vue-i18n" />

declare module "async-lock" {
    namespace AsyncLock {
        type AsyncLockDoneCallback<T> = (err?: Error | null, ret?: T) => void;
        interface AsyncLockOptions { }
    }
    class AsyncLock {
        constructor(options?: AsyncLock.AsyncLockOptions);
        /**
         * Lock on asynchronous code.
         *
         * @param key resource key or keys to lock
         * @param fn function to execute
         * @param opts options
         *
         * @example
         * import AsyncLock = require('async-lock');
         * const lock = new AsyncLock();
         *
         * lock.acquire(
         *     key,
         *     () => {
         *         // return value or promise
         *     },
         *     opts
         * ).then(() => {
         *     // lock released
         * });
         */
        acquire<T>(
            key: string | string[],
            fn: (() => T | PromiseLike<T>) | ((done: AsyncLock.AsyncLockDoneCallback<T>) => any),
            opts?: AsyncLock.AsyncLockOptions,
        ): Promise<T>;
    }
    export = AsyncLock;
}

declare module "*.svg?component" {
    import type { defineComponent, SVGAttributes, VNode } from "vue";
    export const render: (_ctx: { title?: string }, _cache: any[]) => VNode;
    interface SVGComponentProps extends SVGAttributes {
        title?: string;
    }
    const component: ReturnType<typeof defineComponent<SVGComponentProps>>;
    export default component;
}

declare module "bilibili-card:*" {
    import type { defineComponent, HTMLAttributes, VNode } from "vue";
    export const render: (_ctx: object, _cache: any[]) => VNode;
    const component: ReturnType<typeof defineComponent<HTMLAttributes>>;
    export default component;
}

declare module "*/dotnet.js" {
    export type DotnetHostBuilder = {
        /**
         * Starts the runtime and returns promise of the API object.
         */
        create(): Promise<RuntimeAPI>;
    };
    export type MonoConfig = {
        /**
         * Name of the assembly with main entrypoint
         */
        mainAssemblyName?: string;
    }
    type RunAPIType = {
        /**
         * Returns the [JSExport] methods of the assembly with the given name
         */
        getAssemblyExports(assemblyName: string): Promise<any>;
        /**
         * Returns the configuration object used to start the runtime.
         */
        getConfig: () => MonoConfig;
    };
    type APIType = RunAPIType;
    export type RuntimeAPI = {} & APIType;
    export const dotnet: DotnetHostBuilder;
}