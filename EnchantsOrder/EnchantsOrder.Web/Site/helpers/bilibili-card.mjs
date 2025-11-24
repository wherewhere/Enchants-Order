import { compileTemplate } from "vue/compiler-sfc";
import createCard from "hexo-tag-bilibili-card/lib/create-card.js";

/** @type {import("vite").Plugin} */
export default {
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
            /** @type {string} */
            const card = await createCard(imageProxy, [pathname, type, info], false);
            const { code, map } = compileTemplate({
                source: card,
                filename: id,
                id: JSON.stringify(id),
                transformAssetUrls: false
            });
            return {
                code: `${code}\nexport default { render }`,
                map
            };
        }
    }
};