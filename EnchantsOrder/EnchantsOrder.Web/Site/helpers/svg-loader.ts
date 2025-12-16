import type { Plugin } from "vite";
import { compileTemplate } from "@vue/compiler-sfc";
import { optimize } from "svgo";
import fs from "fs/promises";

export default {
    name: "svg-loader",
    enforce: "pre",
    async load(id) {
        if (id.endsWith(".svg?component")) {
            /** @type {string} */
            let svg: string;
            try {
                const [path] = id.split('?', 2);
                svg = await fs.readFile(path, "utf-8");
            }
            catch {
                return;
            }
            svg = optimize(svg, { path: id }).data;
            svg = svg.replace(/<style/g, '<component is="style"').replace(/<\/style/g, '</component');
            let defaultTitle = "''";
            const svgMatch = svg.match(/<svg\b[^>]*>([\s\S]*?)<\/svg>/i);
            if (svgMatch) {
                const titleMatch = svgMatch[1].match(/^\s*<title[^>]*>([\s\S]*?)<\/title>/i);
                if (titleMatch) {
                    defaultTitle = JSON.stringify(titleMatch[1].trim());
                    svg = svg.replace(/<title([^>]*)>[\s\S]*?<\/title>/i, (_, attrs) => `<title${attrs} v-if="title">{{ title }}</title>`);
                }
                else {
                    svg = svg.replace(/<svg\b([^>]*)>/i, m => `${m}<title v-if="title">{{ title }}</title>`);
                }
            }
            const { code, map } = compileTemplate({
                id: JSON.stringify(id),
                source: svg,
                filename: id,
                transformAssetUrls: false
            })
            return {
                code: `${code}\nexport default { render, props: { title: { type: String, default: ${defaultTitle} } } }`,
                map
            }
        }
    }
} as Plugin;