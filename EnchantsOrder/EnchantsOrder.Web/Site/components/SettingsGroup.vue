<template>
    <div class="settings-group">
        <div class="header-presenter" v-check-solt="$slots.header">
            <slot name="header"></slot>
        </div>
        <div class="items-presenter" v-check-solt="$slots.default">
            <slot></slot>
        </div>
    </div>
</template>

<script lang="ts" setup>
    import vCheckSolt from "../directives/checkSolt";
</script>

<style lang="scss" scoped>
    .settings-group {
        --body-strong-text-block-font-size: var(--type-ramp-base-font-size);

        :deep(div.header-presenter) {
            margin: 1rem 0 calc(var(--base-horizontal-spacing-multiplier) * 2px) calc(var(--stroke-width) * 1px);
            font-size: var(--body-strong-text-block-font-size);
            font-weight: bold;
            color: var(--neutral-foreground-rest);
        }

        :deep(div.items-presenter) {
            display: flex;
            flex-direction: column;
            row-gap: 0.3rem;
        }
    }
</style>