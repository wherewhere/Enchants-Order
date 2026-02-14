import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import simpleHtmlPlugin from "vite-plugin-simple-html";
import svgLoader from "./helpers/svg-loader";
import bilibiliCard from "bilibili-card/dist/lib/bilibili-card";
import dotnetFrameworkStaticFiles from "./helpers/dotnet-framework-static-files";
import githubImporter from "./helpers/github-importer";
import cssnano from "cssnano";
import getOutputOptions from "./helpers/output";

export default defineConfig(({ mode }) => {
    return {
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
            minify: "terser",
            rollupOptions: {
                output: getOutputOptions(mode)
            },
            emptyOutDir: true
        }
    };
});