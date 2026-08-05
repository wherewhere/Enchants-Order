import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import simpleHtmlPlugin from "vite-plugin-simple-html";
import svgLoader from "./helpers/svg-loader.mts";
import bilibiliCard from "bilibili-card/dist/lib/bilibili-card";
import dotnetFrameworkStaticFiles from "./helpers/dotnet-framework-static-files.mts";
import githubImporter from "./helpers/github-importer.mts";
import cssnano from "cssnano";

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
        bilibiliCard(),
        svgLoader,
        dotnetFrameworkStaticFiles
    ],
    css: {
        preprocessorOptions: {
            scss: {
                importers: [githubImporter]
            }
        },
        postcss: {
            plugins: [
                cssnano({
                    preset: "advanced"
                })
            ]
        },
        devSourcemap: true
    },
    build: {
        outDir: "../wwwroot",
        sourcemap: true,
        rolldownOptions: {
            checks: {
                pluginTimings: false
            }
        },
        emptyOutDir: true,
        chunkSizeWarningLimit: 1024
    }
});