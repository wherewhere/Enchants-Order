<template>
    <slot></slot>
</template>

<script lang="ts">
    import { computed, type PropType } from "vue";
    export default {
        name: "ProvideValue",
        props: {
            name: {
                type: String,
                required: true
            },
            value: undefined! as PropType<any>,
        },
        provide() {
            return {
                [this.name!]: computed(() => this.value)
            }
        }
    }
</script>