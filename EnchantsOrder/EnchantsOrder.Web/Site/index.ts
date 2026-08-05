import {
    provideFluentDesignSystem,
    baseLayerLuminance,
    StandardLuminance
} from "@fluentui/web-components";
provideFluentDesignSystem().register();

const scheme = matchMedia("(prefers-color-scheme: dark)");
scheme.addEventListener("change", e => baseLayerLuminance.withDefault(e.matches ? StandardLuminance.DarkMode : StandardLuminance.LightMode));
if (scheme.matches) {
    baseLayerLuminance.withDefault(StandardLuminance.DarkMode);
}

import { createApp } from "vue";
import { createHead } from "@unhead/vue/client";
import App from "./App.vue";
import i18n from "./i18n";

createApp(App).use(i18n).use(createHead()).mount("#vue-app");