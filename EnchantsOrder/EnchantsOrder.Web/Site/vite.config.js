import { defineConfig } from "vite";
import { compileTemplate } from "vue/compiler-sfc";
import vue from "@vitejs/plugin-vue";
import legacy from "@vitejs/plugin-legacy";
import svgLoader from "vite-svg-loader";
import postcssPresetEnv from "postcss-preset-env";
import createCard from "hexo-tag-bilibili-card/lib/create-card.js";

export default defineConfig({
    base: "./",
    plugins: [
        vue({
            include: [/\.vue$/, /\.md$/],
            template: {
                compilerOptions: {
                    isCustomElement: tag => tag.includes('-')
                }
            }
        }),
        legacy({
            targets: ["supports custom-elementsv1"],
            polyfills: false,
            renderLegacyChunks: false
        }),
        svgLoader(), {
            name: "bilibili-card",
            enforce: "pre",
            resolveId(source) {
                if (source.startsWith("bilibili-card:")) {
                    return source;
                }
            },
            async load(id) {
                if (id.startsWith("bilibili-card:")) {
                    const url = new URL(id);
                    const { pathname, searchParams } = url;
                    const imageProxy = searchParams.get("proxy") || undefined;
                    const type = searchParams.get("type") || undefined;
                    const info = searchParams.get("info") || undefined;
                    const card = await createCard(imageProxy, [pathname, type, info], false);
                    const { code } = compileTemplate({
                        id: JSON.stringify(id),
                        source: card,
                        transformAssetUrls: false
                    });
                    return `${code}\nexport default { render: render }`;
                }
            }
        }
    ],
    css: {
        postcss: {
            plugins: [postcssPresetEnv({
                stage: 0,
                browsers: ["supports custom-elementsv1"]
            })]
        },
        preprocessorOptions: {
            scss: {
                importers: [{
                    canonicalize(url) {
                        return url.startsWith("github:") ? new URL(url) : null;
                    },
                    async load(canonicalUrl) {
                        const { pathname, searchParams } = canonicalUrl;
                        const branch = searchParams.get("branch") || "main";
                        const path = searchParams.get("path") || '/index.css';
                        try {
                            return {
                                contents: await fetch(`https://github.com/${pathname}/raw/refs/heads/${branch}${path}`).then(res => res.text()),
                                syntax: (() => {
                                    switch (path.split('.').pop()) {
                                        case "scss":
                                            return "scss";
                                        case "sass":
                                            return "indented";
                                        case "css":
                                        default:
                                            return "css";
                                    }
                                })()
                            }
                        }
                        catch (e) {
                            console.warn(`\nFailed to fetch '${canonicalUrl.href}': ${e}`);
                            return {
                                contents: `@import "https://cdn.jsdelivr.net/gh/${pathname}@${branch}${path}";`,
                                syntax: "css"
                            }
                        }
                    }
                }]
            }
        }
    },
    server: {
        port: 5173
    },
    build: {
        outDir: "dist",
        rollupOptions: {
            output: {
                entryFileNames: "assets/[name].js",
                chunkFileNames: "assets/[name].js",
                assetFileNames: "assets/[name].[ext]",
            }
        }
    }
});