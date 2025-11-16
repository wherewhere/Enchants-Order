<template>
    <div class="markdown-body">
        <slot></slot>
    </div>
</template>

<style lang="scss" scoped>
    :deep(h1) {
        font-size: var(--type-ramp-plus-2-font-size);
        line-height: var(--type-ramp-plus-2-line-height);
    }

    :deep(h2) {
        font-size: var(--type-ramp-plus-1-font-size);
        line-height: var(--type-ramp-plus-1-line-height);
    }

    :deep(h3) {
        font-size: var(--type-ramp-base-font-size);
        line-height: var(--type-ramp-base-line-height);
    }
</style>