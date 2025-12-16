<template>
    <div class="content">
        <div class="stack-horizontal" style="justify-content: space-between;">
            <h1 id="title">{{ t("title") }}</h1>
            <div class="stack-horizontal" style="padding: 0 4px; align-items: center;">
                <fluent-tooltip class="title-tooltip" anchor="title-info">
                    <BiliBiliCard :theme="theme" style="min-width: 300px;" />
                </fluent-tooltip>
                <Info20Regular id="title-info" class="text-info" />
            </div>
        </div>
        <div class="stack-vertical">
            <SettingsGroup>
                <SettingsCard>
                    <template #icon>
                        <LocalLanguage20Regular :title="t('global.lang.header')" />
                    </template>
                    <template #header>
                        <h3 id="global-lang" class="unset">{{ t("global.lang.header") }}</h3>
                    </template>
                    <template #description>
                        {{ t("global.lang.description") }}
                    </template>
                    <fluent-select :placeholder="t('global.lang.select.placeholder')" v-model="$i18n.locale"
                                   style="min-width: calc(var(--base-height-multiplier) * 23px);">
                        <fluent-option value="zh-CN">中文 (中国)</fluent-option>
                        <fluent-option value="en-US">English (United States)</fluent-option>
                    </fluent-select>
                </SettingsCard>
            </SettingsGroup>
            <SettingsGroup>
                <template #header>
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h2 id="object" class="unset">{{ t("object.header") }}</h2>
                        <div v-if="usePresets" class="text-button" :title="t('object.button.title')" type="button"
                             style="padding: 4px;" @click="listItemEnchantsAsync" :disabled="loading">
                            <fluent-progress-ring v-if="loading"
                                                  style="width: 12px; height: 12px;"></fluent-progress-ring>
                            <TriangleRight12Filled v-else />
                        </div>
                    </div>
                </template>
                <SettingsCard class="settings-nowarp">
                    <template #icon>
                        <Database20Regular :title="t('object.switch.icon.title')" />
                    </template>
                    <template #header>
                        <h3 id="object-switch" class="unset">{{ t("object.switch.header") }}</h3>
                    </template>
                    <template #description>
                        {{ t("object.switch.description") }}
                    </template>
                    <!-- @vue-generic {Switch, "checked" } -->
                    <ValueChangeHost v-model="usePresets" value-name="checked" event-name="change">
                        <fluent-switch>
                            {{ usePresets ? t("object.switch.switch.on") : t("object.switch.switch.off") }}
                        </fluent-switch>
                    </ValueChangeHost>
                </SettingsCard>
                <SettingsCard>
                    <template #icon>
                        <Bug20Regular :title="t('object.penalty.icon.title')" />
                    </template>
                    <template #header>
                        <h3 id="object-penalty" class="unset">{{ t("object.penalty.header") }}</h3>
                    </template>
                    <template #description>
                        {{ t("object.penalty.description") }}
                    </template>
                    <fluent-number-field v-model="initialPenalty" min="0"></fluent-number-field>
                </SettingsCard>
                <SettingsCard v-if="usePresets">
                    <template #icon>
                        <Box20Regular :title="t('object.select.icon.title')" />
                    </template>
                    <template #header>
                        <h3 id="object-select" class="unset">{{ t("object.select.header") }}</h3>
                    </template>
                    <template #description>
                        {{ t("object.select.description") }}
                    </template>
                    <fluent-select :placeholder="t('object.select.select.placeholder')" v-model="item"
                                   style="min-width: calc(var(--base-height-multiplier) * 26px);">
                        <fluent-option v-for="item in items" :value="item">{{ item }}</fluent-option>
                    </fluent-select>
                </SettingsCard>
            </SettingsGroup>
            <SettingsGroup v-if="!usePresets">
                <template #header>
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h2 id="enchantment" class="unset">{{ t("enchantment.header") }}</h2>
                        <div class="text-button" :title="t('enchantment.button.title')" type="button"
                             style="padding: 2px;" @click="addEnchantment" :disabled="!enchantment.name">
                            <Add16Regular />
                        </div>
                    </div>
                </template>
                <SettingsCard>
                    <template #icon>
                        <Rename20Regular :title="t('enchantment.name.icon.title')" />
                    </template>
                    <template #header>
                        <h3 id="enchantment-name" class="unset">{{ t("enchantment.name.header") }}</h3>
                    </template>
                    <template #description>
                        {{ t("enchantment.name.description") }}
                    </template>
                    <!-- @vue-generic {Combobox, "value" } -->
                    <ValueChangeHost v-model="enchantment.name" value-name="value" event-name="change"
                                     style="display: inherit;">
                        <fluent-combobox :placeholder="t('enchantment.name.combobox.placeholder')" autocomplete="both"
                                         style="min-width: 0;">
                            <fluent-option v-for="enchantment in enchantments">
                                {{ enchantment.name }}
                            </fluent-option>
                        </fluent-combobox>
                    </ValueChangeHost>
                </SettingsCard>
                <SettingsCard>
                    <template #icon>
                        <Trophy20Regular :title="t('enchantment.level.header')" />
                    </template>
                    <template #header>
                        <h3 id="enchantment-level" class="unset">{{ t("enchantment.level.header") }}</h3>
                    </template>
                    <template #description>
                        {{ t("enchantment.level.description") }}
                    </template>
                    <fluent-number-field v-model="enchantment.level" min="1"></fluent-number-field>
                </SettingsCard>
                <SettingsCard>
                    <template #icon>
                        <Scales20Regular :title="t('enchantment.weight.header')" />
                    </template>
                    <template #header>
                        <h3 id="enchantment-weight" class="unset">{{ t("enchantment.weight.header") }}</h3>
                    </template>
                    <template #description>
                        <span v-html="t('enchantment.weight.description')"></span>
                    </template>
                    <fluent-number-field v-model="enchantment.weight" min="1"></fluent-number-field>
                </SettingsCard>
            </SettingsGroup>
            <SettingsGroup v-if="!usePresets && wantedList.length">
                <template #header>
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h2 id="wanted" class="unset">{{ t("list.header") }}</h2>
                        <div class="text-button" :title="t('enchantment.list.button.title')" type="button"
                             style="padding: 4px;" @click="orderingWantListAsync"
                             :disabled="!wantedList.length || loading">
                            <fluent-progress-ring v-if="loading"
                                                  style="width: 12px; height: 12px;"></fluent-progress-ring>
                            <TriangleRight12Filled v-else />
                        </div>
                    </div>
                </template>
                <SettingsCard class="settings-nowarp" v-for="item in wantedList">
                    <template #icon>
                        <Book20Regular :title="t('list.item.icon.title')" />
                    </template>
                    <template #header>
                        <h3 class="unset">{{ item.name }}</h3>
                    </template>
                    <template #description>
                        {{ t("list.item.description", [item.level, item.weight, item.level * item.weight]) }}
                    </template>
                    <div class="text-button" :title="t('list.item.button.title')" type="button" style="padding: 8px;"
                         @click="removeEnchantment(item)">
                        <Delete16Regular />
                    </div>
                </SettingsCard>
            </SettingsGroup>
            <SettingsGroup v-if="results.length">
                <template #header>
                    <h2 id="results" class="unset">{{ t("results.header") }}</h2>
                </template>
                <SettingsExpander v-for="result in results" expanded="true">
                    <template #icon>
                        <Script20Regular :title="t('results.step.icon.title')" />
                    </template>
                    <template #header>
                        <h3 class="unset">{{ t("results.step.header", [result.TotalExperience]) }}</h3>
                    </template>
                    <template #description>
                        {{ t("results.step.description", [result.Penalty, result.MaxExperience]) }}
                    </template>
                    <div class="setting-expander-content-grid" style="font-family: var(--font-monospace);">
                        <pre class="unset">{{ readSteps(result.Steps) }}</pre>
                    </div>
                </SettingsExpander>
            </SettingsGroup>
        </div>
    </div>
</template>

<script lang="ts" setup>
    import AsyncLock from "async-lock";
    import type { DotnetHostBuilder } from "../_framework/dotnet.js";
    import type { Exports, IEnchantment, OrderingResults } from "./types";
    import type { Combobox, Switch } from "@fluentui/web-components";
    import { computed, onMounted, ref, shallowRef, watch, watchEffect, watchPostEffect } from "vue";
    import { useI18n } from "vue-i18n";
    import { useSeoMeta } from "@unhead/vue";
    import { compressToEncodedURIComponent, decompressFromEncodedURIComponent } from "lz-string";
    import { getEnchantments, type Enchantment, type IEnchant } from "./helpers/enchantment";
    import { keywords } from "./package.json";
    import theme from "bilibili-card/src/styles/bilibili-card.fluent.css?url";
    import SettingsCard from "./components/SettingsCard.vue";
    import SettingsExpander from "./components/SettingsExpander.vue";
    import SettingsGroup from "./components/SettingsGroup.vue";
    import ValueChangeHost from "./components/ValueChangeHost.vue";
    import Info20Regular from "@fluentui/svg-icons/icons/info_20_regular.svg?component";
    import LocalLanguage20Regular from "@fluentui/svg-icons/icons/local_language_20_regular.svg?component";
    import TriangleRight12Filled from "@fluentui/svg-icons/icons/triangle_right_12_filled.svg?component";
    import Database20Regular from "@fluentui/svg-icons/icons/database_20_regular.svg?component";
    import Bug20Regular from "@fluentui/svg-icons/icons/bug_20_regular.svg?component";
    import Box20Regular from "@fluentui/svg-icons/icons/box_20_regular.svg?component";
    import Add16Regular from "@fluentui/svg-icons/icons/add_16_regular.svg?component";
    import Rename20Regular from "@fluentui/svg-icons/icons/rename_20_regular.svg?component";
    import Trophy20Regular from "@fluentui/svg-icons/icons/trophy_20_regular.svg?component";
    import Scales20Regular from "@fluentui/svg-icons/icons/scales_20_regular.svg?component";
    import Book20Regular from "@fluentui/svg-icons/icons/book_20_regular.svg?component";
    import Delete16Regular from "@fluentui/svg-icons/icons/delete_16_regular.svg?component";
    import Script20Regular from "@fluentui/svg-icons/icons/script_20_regular.svg?component";
    import BiliBiliCard from "bilibili-card:BV1xkTLzsEM5";

    const { locale, t } = useI18n();
    watchPostEffect(() => document.documentElement.lang = locale.value);

    const title = computed(() => t("title"));
    const description = computed(() => t("description"));
    const author = "wherewhere";
    useSeoMeta({
        // Basic SEO
        title,
        description,
        author: author,
        keywords: keywords.join(", "),

        // Open Graph
        ogTitle: title,
        ogDescription: description,
        ogType: "website",
        ogLocale: () => locale.value.replace('-', '_'),
        ogSiteName: title,

        // Twitter
        twitterCard: "summary",
        twitterSite: "@wherewhere7",

        // Product specific (structured data will be generated)
        articleAuthor: [author],
        articleTag: keywords
    });

    const locker = new AsyncLock();
    const enchantments = ref<Enchantment[]>([]);
    const initialPenalty = shallowRef(0);
    const enchantment = ref({
        name: '',
        level: 1,
        weight: 1
    } as Enchantment);
    const loading = shallowRef(false);
    const usePresets = shallowRef(false);
    const items = ref<string[]>([]);
    const item = shallowRef<string>();
    const wantedList = ref<Enchantment[]>([]);
    const results = ref<OrderingResults[]>([]);

    let exports = null as Exports | null;
    let isInitDotnet = false;

    watchEffect(() => {
        try {
            loading.value = true;
            enchantments.value = getEnchantments(locale.value as "zh-CN" | "en-US");
            items.value = enchantments.value.slice().sort((a, b) => b.items.length - a.items.length)[0].items;
            item.value = items.value[0];
        }
        finally {
            loading.value = false;
        }
    });

    watch(
        () => enchantment.value.name,
        (newValue, oldValue) => {
            if (newValue !== oldValue) {
                const _enchantment = enchantments.value.find(x => x.name === enchantment.value.name);
                if (_enchantment) {
                    enchantment.value = { ..._enchantment };
                }
            }
        }
    );

    async function initDotNetAsync() {
        if (!isInitDotnet) {
            await locker.acquire("dotnet-init", async () => {
                if (!isInitDotnet) {
                    const url = "../_framework/dotnet.js";
                    const { dotnet } = await import(/* @vite-ignore */ url) as { dotnet: DotnetHostBuilder };
                    const { getAssemblyExports, getConfig } = await dotnet.create();
                    const config = getConfig();
                    const { Exports } = await getAssemblyExports(config.mainAssemblyName!);
                    exports = Exports;
                    isInitDotnet = true;
                }
            });
        }
    }

    function addEnchantment() {
        if (enchantment.value.name) {
            const temp = { ...enchantment.value };
            temp.level = +temp.level;
            temp.weight = +temp.weight;
            wantedList.value.push(temp);
        }
    }

    function removeEnchantment(item: Enchantment) {
        if (item) {
            const index = wantedList.value.indexOf(item);
            if (index > -1) {
                wantedList.value.splice(index, 1);
            }
        }
    }

    function readSteps(steps: IEnchantment[][]) {
        function getRomanNumber(num: number, maxValue = 2000) {
            if (num > maxValue) { return num.toString(); }
            const arabic = [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];
            const roman = ['M', "CM", 'D', "CD", 'C', "XC", 'L', "XL", 'X', "IX", 'V', "IV", 'I'];
            let i = 0;
            let result = '';
            while (num > 0) {
                while (num >= arabic[i]) {
                    num -= arabic[i];
                    result += roman[i];
                }
                i++;
            }
            return result;
        }
        function readEnchantment(enchantment: IEnchantment) {
            return `${enchantment.Name} ${getRomanNumber(enchantment.Level)}`;
        }
        function readStep(step: IEnchantment[], index: number) {
            const builder = [];
            builder.push(t("results.step.content", [index]));
            const count = step.length;
            const half = Math.floor(count / 2);
            for (let i = half; i > 0; i--) {
                const flag = count === 2;
                const idx = (half * 2) - (i * 2);
                builder.push(
                    ` ${flag ? '' : '('}${readEnchantment(step[idx])} + ${readEnchantment(step[idx + 1])}${flag ? '' : ')'}${idx + 2 === count ? '' : " +"}`
                );
            }
            if (count % 2 === 1) {
                builder.push(` ${readEnchantment(step[step.length - 1])}`);
            }
            return builder.join('');
        }
        return steps.map((x, i) => readStep(x, i + 1)).join('\n');
    }

    async function listItemEnchantsAsync() {
        try {
            loading.value = true;
            const text = item.value;
            const enchants = enchantments.value.filter(x => !x.hidden && x.items.some(i => i === text));
            if (enchants.length) {
                const group: IEnchant[][] = [];
                while (enchants.length) {
                    const enchantment = enchants[0];
                    let list: IEnchant[] = [enchantment];
                    enchants.splice(0, 1);
                    if (enchantment.incompatible && enchantment.incompatible.length) {
                        for (let i = enchants.length; --i >= 0;) {
                            const temp = enchants[i];
                            if (temp.incompatible && temp.incompatible.some(x => x === enchantment.name)) {
                                list.push(temp);
                                enchants.splice(i, 1);
                            }
                        }
                        if (list.length) {
                            interface ITreeEnchant extends IEnchant {
                                enchantments: IEnchant[];
                            }
                            const tempList: (IEnchant | ITreeEnchant)[] = [];
                            while (list.length) {
                                const temp = list[0];
                                list.splice(0, 1);
                                const temps = list.filter(x => x.level === temp.level && x.weight === temp.weight);
                                if (temps.length) {
                                    const array = [...temps, temp].sort((a, b) => a.name.localeCompare(b.name));
                                    tempList.push({
                                        enchantments: array,
                                        get level() {
                                            return this.enchantments[0].level;
                                        },
                                        get weight() {
                                            return this.enchantments[0].weight;
                                        },
                                        get name() {
                                            return this.enchantments.map(x => x.name).join('/');
                                        }
                                    } as ITreeEnchant);
                                    for (const enchantmentTemp of array) {
                                        const idx = list.indexOf(enchantmentTemp);
                                        if (idx > -1) { list.splice(idx, 1); }
                                    }
                                }
                                else {
                                    tempList.push(temp);
                                }
                            }
                            list = tempList;
                        }
                    }
                    group.push(list.flat());
                }
                function getAllEnchantmentPaths(group: IEnchant[][]) {
                    const result: IEnchant[][] = [];
                    function growing(depth: number, path: IEnchant[]) {
                        if (depth === group.length) {
                            result.push([...path]);
                            return;
                        }
                        const next = depth + 1;
                        for (const enchantment of group[depth]) {
                            path.push(enchantment);
                            growing(next, path);
                            path.pop();
                        }
                    }
                    growing(0, []);
                    return result;
                }
                results.value = [];
                for (const list of getAllEnchantmentPaths(group)) {
                    try {
                        results.value.push(await orderingAsync(list, initialPenalty.value));
                    }
                    catch (ex) {
                        console.warn(ex);
                    }
                }
            }
        }
        finally {
            loading.value = false;
        }
    }

    async function orderingWantListAsync() {
        try {
            loading.value = true;
            if (wantedList.value.length) {
                results.value = [await orderingAsync(wantedList.value, initialPenalty.value)];
                setSettings();
            }
        }
        finally {
            loading.value = false;
        }
    }

    async function orderingAsync(wantedList: IEnchant[], initialPenalty: number | string) {
        await initDotNetAsync();
        return exports!.Ordering(wantedList, +initialPenalty);
    }

    let hashChanged = false;
    function loadSettings() {
        if (hashChanged) {
            hashChanged = false;
            return;
        }
        const hash = location.hash.substring(1);
        if (hash) {
            const params = new URLSearchParams(hash);
            if (params.has("wanted")) {
                const value = params.get("wanted");
                if (value) {
                    const wanted = decompressFromEncodedURIComponent(value);
                    if (wanted) {
                        const list = JSON.parse(wanted)
                        if (Array.isArray(list)) {
                            wantedList.value = list;
                        }
                    }
                }
            }
        }
    }

    function setSettings() {
        const settings: { [key: string]: string } = {};
        if (wantedList.value.length) {
            settings.wanted = compressToEncodedURIComponent(
                JSON.stringify(
                    wantedList.value.map(x => ({
                        name: x.name,
                        level: x.level,
                        weight: x.weight
                    }))));
        }
        location.hash = new URLSearchParams(settings).toString();
        hashChanged = true;
    }

    onMounted(() => {
        loadSettings();
        addEventListener("hashchange", loadSettings);
    });
</script>

<style lang="scss">
    @use "github:microsoft/fluentui-blazor?branch=dev&path=/src/Core/wwwroot/css/reboot.css";

    :root {
        --font-monospace: "Cascadia Code NF", "Cascadia Code PL", "Cascadia Code", "Cascadia Next SC", "Cascadia Next TC", "Cascadia Next JP", Consolas, "Courier New", "Liberation Mono", SFMono-Regular, Menlo, Monaco, monospace;
        --settings-card-padding: calc(var(--design-unit) * 4px);
        color-scheme: light;

        @media (prefers-color-scheme: dark) {
            color-scheme: dark;
        }
    }

    * {
        transition: background-color 0.083s ease-in-out;
    }

    body,
    .body {
        width: 100%;
        height: 100%;
        overflow: hidden;
        background: var(--neutral-fill-stealth-rest);
    }
</style>

<style lang="scss" scoped>
    .content {
        height: 100%;
        overflow-y: auto;
        padding: 0 16px 16px;
        transition: padding 0.083s ease-in-out, background-color 0.083s ease-in-out;

        @media (min-width: 640px) {
            padding: 0 32px 24px;
        }

        @media (min-width: 1007px) {
            padding: 0 64px 24px;
        }

        @media (min-width: 1372px) {
            padding: 0 128px 24px;
        }
    }

    #title {
        margin: 24px 0;
        font-size: 28px;
        font-weight: 600;
        line-height: 1.34;
    }

    :deep(.stack-vertical) {
        display: flex;
        flex-direction: column;
    }

    :deep(.stack-horizontal) {
        display: flex;
        flex-direction: row;
    }

    :deep(h6.unset),
    :deep(h5.unset),
    :deep(h4.unset),
    :deep(h3.unset),
    :deep(h2.unset),
    :deep(h1.unset) {
        margin-top: 0;
        margin-bottom: 0;
        font-weight: inherit;
        font-family: inherit;
        font-size: inherit;
        line-height: inherit;
    }

    :deep(fluent-select)::part(listbox),
    :deep(fluent-combobox)::part(listbox) {
        max-height: calc(var(--base-height-multiplier) * 30px);
    }

    :deep(fluent-tooltip.title-tooltip) {
        &::part(tooltip) {
            padding: 0;

            &::after {
                display: none;
            }
        }

        .video-holder {
            border-radius: calc(var(--control-corner-radius) * 1px);

            .cover-box {
                border-radius: calc(var(--control-corner-radius) * 1px) 0 0 calc(var(--control-corner-radius) * 1px);
            }

            .video-content-container {
                border-radius: 0 calc(var(--control-corner-radius) * 1px) calc(var(--control-corner-radius) * 1px) 0;
            }
        }
    }

    :deep(pre.unset) {
        margin-top: 0;
        margin-bottom: 0;
        font-size: inherit;
        font-family: inherit;
        white-space: pre-wrap;
    }

    .text-button {
        fill: currentColor;
        border-radius: calc(var(--control-corner-radius) * 1px);
    }

    .text-button:hover {
        background: var(--neutral-fill-input-hover);
    }

    .text-button:active {
        background: var(--neutral-fill-input-active);
    }

    .text-button[disabled="true"] {
        cursor: not-allowed;
        opacity: var(--disabled-opacity);
    }

    .text-button[disabled="true"]:active {
        background: none;
    }

    .text-info {
        outline: none;
        fill: var(--neutral-foreground-rest);
    }

    .text-info:hover {
        fill: var(--neutral-foreground-hover);
    }

    .text-button[disabled="true"] {
        cursor: not-allowed;
        opacity: var(--disabled-opacity);
    }
</style>