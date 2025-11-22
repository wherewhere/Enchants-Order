<template />

<script lang="ts" setup>
    import { watchPostEffect } from "vue";

    const { lang, title, description } = defineProps<{
        lang: string;
        title: string;
        description: string;
    }>();

    watchPostEffect(() => document.documentElement.lang = lang);
    watchPostEffect(() => document.title = title);
    watchPostEffect(() => {
        const meta = document.querySelector("meta[name='description']");
        if (meta instanceof HTMLMetaElement) {
            meta.content = description;
        }
    });
</script>