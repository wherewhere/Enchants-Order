import {
    provideFluentDesignSystem,
    fluentAccordion,
    fluentAccordionItem,
    fluentAnchoredRegion,
    fluentButton,
    fluentCombobox,
    fluentNumberField,
    fluentOption,
    fluentProgressRing,
    fluentSelect,
    fluentSwitch,
    fluentTooltip,
    baseLayerLuminance,
    StandardLuminance
} from "@fluentui/web-components";
provideFluentDesignSystem()
    .register(
        fluentAccordion(),
        fluentAccordionItem(),
        fluentAnchoredRegion(),
        fluentButton(),
        fluentCombobox(),
        fluentNumberField(),
        fluentOption(),
        fluentProgressRing(),
        fluentSelect(),
        fluentSwitch(),
        fluentTooltip()
    );

const scheme = matchMedia("(prefers-color-scheme: dark)");
if (typeof scheme !== "undefined") {
    scheme.addEventListener("change", e => baseLayerLuminance.withDefault(e.matches ? StandardLuminance.DarkMode : StandardLuminance.LightMode));
    if (scheme.matches) {
        baseLayerLuminance.withDefault(StandardLuminance.DarkMode);
    }
}

import { createApp } from "vue";
import App from "./App.vue";
import i18n from "./i18n";

createApp(App).use(i18n).mount("#vue-app");