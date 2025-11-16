<template>
    <div class="settings-card">
        <ProvideValue name="fillColor" :value="fillColor">
            <SettingsPresenter class="presenter">
                <template #icon>
                    <slot name="icon"></slot>
                </template>
                <template #header>
                    <slot name="header"></slot>
                </template>
                <template #description>
                    <slot name="description"></slot>
                </template>
                <slot></slot>
            </SettingsPresenter>
        </ProvideValue>
    </div>
</template>

<script lang="ts">
    import { neutralFillInputRest, type Swatch } from "@fluentui/web-components";
    import type { DesignToken } from "@microsoft/fast-foundation";
    import ProvideValue from "./ProvideValue.vue";
    import SettingsPresenter from "./SettingsPresenter.vue";
    export default {
        name: "SettingsCard",
        components: {
            ProvideValue,
            SettingsPresenter
        },
        data() {
            return {
                fillColor: this.injectFillColor as any as DesignToken<Swatch>
            }
        },
        inject: {
            injectFillColor: {
                from: "fillColor",
                default: neutralFillInputRest
            }
        }
    };
</script>

<style lang="scss" scoped>
    .settings-card {
        display: block;
        box-sizing: border-box;
        background: var(--neutral-fill-input-rest);
        color: var(--neutral-foreground-rest);
        border: calc(var(--stroke-width) * 1px) solid var(--neutral-stroke-layer-rest);
        border-radius: calc(var(--control-corner-radius) * 1px);
        box-shadow: var(--elevation-shadow-card-rest);
        --settings-card-padding: calc(var(--design-unit) * 4px);

        :deep(.presenter) {
            padding: var(--settings-card-padding);
        }
    }
</style>