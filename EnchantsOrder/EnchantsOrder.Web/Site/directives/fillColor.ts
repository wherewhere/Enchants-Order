import { toRaw, type Directive, type DirectiveHook } from "vue";
import { fillColor, type Swatch } from "@fluentui/web-components";
import type { DesignToken } from "@microsoft/fast-foundation";

const observer = new WeakMap<HTMLElement, () => void>();
const scheme = matchMedia("(prefers-color-scheme: dark)");

function addListener(element: HTMLElement, callback: () => void) {
    if (typeof scheme !== "undefined") {
        observer.set(element, callback);
        scheme.addEventListener("change", callback);
    }
    callback();
}

function removeListener(element: HTMLElement) {
    if (typeof scheme !== "undefined") {
        const callback = observer.get(element);
        if (callback) {
            scheme.removeEventListener("change", callback);
            observer.delete(element);
        }
    }
}

const register: DirectiveHook<HTMLElement, any, DesignToken<Swatch> | undefined, string, any> = async (element, binding) => {
    if (element instanceof HTMLElement) {
        if (binding.value !== binding.oldValue) {
            const color = toRaw(binding.value);
            if (color) {
                // addListener(element, () => fillColor.setValueFor(element, color.getValueFor(element.parentElement!)));
                fillColor.setValueFor(element, color.getValueFor(element.parentElement!));
            }
            else {
                fillColor.deleteValueFor(element);
                // removeListener(element);
            }
        }
    }
};

const unregister: DirectiveHook<HTMLElement, any, DesignToken<Swatch> | undefined, string, any> = element => {
    if (element instanceof HTMLElement) {
        removeListener(element);
    }
};

const directive: Directive<HTMLElement, DesignToken<Swatch> | undefined> = {
    mounted: register,
    updated: register,
    // unmounted: unregister
}

export default directive;