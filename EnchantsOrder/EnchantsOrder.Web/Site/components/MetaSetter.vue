<script lang="ts">
    export default {
        props: {
            lang: {
                type: String,
                required: true
            },
            title: {
                type: String,
                required: true
            },
            description: {
                type: String,
                required: true
            }
        },
        watch: {
            lang(newValue: string, oldValue: string) {
                if (newValue !== oldValue) {
                    this.setLang(newValue);
                }
            },
            title(newValue: string, oldValue: string) {
                if (newValue !== oldValue) {
                    this.setTitle(newValue);
                }
            },
            description(newValue: string, oldValue: string) {
                if (newValue !== oldValue) {
                    this.setDescription(newValue);
                }
            }
        },
        methods: {
            setLang(lang: string) {
                document.documentElement.lang = lang;
            },
            setTitle(title: string) {
                document.title = title;
            },
            setDescription(description: string) {
                const meta = document.querySelector("meta[name='description']");
                if (meta instanceof HTMLMetaElement) {
                    meta.content = description;
                }
            }
        },
        mounted() {
            this.setLang(this.lang);
            this.setTitle(this.title);
            this.setDescription(this.description);
        }
    }
</script>