import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import simpleHtmlPlugin from "vite-plugin-simple-html";
import svgLoader from "./helpers/svg-loader.mjs";
import bilibiliCard from "./helpers/bilibili-card.mjs";
import dotnetFrameworkStaticFiles from "./helpers/dotnet-framework-static-files.mjs";
import githubImporter from "./helpers/github-importer.mjs";

export default defineConfig({
    base: "./",
    plugins: [
        vue({
            template: {
                compilerOptions: {
                    isCustomElement: tag => tag.includes('-')
                }
            }
        }),
        simpleHtmlPlugin({
            minify: {
                minifyJs: true,
                sortSpaceSeparatedAttributeValues: true,
                sortAttributes: true,
                tagOmission: false
            }
        }),
        svgLoader,
        bilibiliCard,
        dotnetFrameworkStaticFiles
    ],
    css: {
        preprocessorOptions: {
            scss: {
                importers: [githubImporter]
            }
        },
        devSourcemap: true
    },
    build: {
        outDir: "../wwwroot",
        sourcemap: true,
        minify: "terser",
        rollupOptions: {
            output: {
                assetFileNames: "assets/[name].[ext]",
                chunkFileNames: "assets/[name].js",
                entryFileNames: "assets/[name].js"
            }
        },
        emptyOutDir: true
    }
});