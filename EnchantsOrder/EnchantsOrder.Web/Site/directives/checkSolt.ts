import type { FunctionDirective, Slot } from "vue";

export function isSlot(solt?: Slot) {
    if (typeof solt === "undefined") {
        return false;
    }
    else if (typeof solt === "function") {
        let value = solt();
        if (value instanceof Array) {
            const result = value.some(x => {
                if (typeof x === "object") {
                    if (typeof x.type === "symbol") {
                        const child = x.children;
                        if (typeof child === "string" || child instanceof Array) {
                            return !!child.length;
                        }
                        else {
                            return !!child;
                        }
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return false;
                }
            });
            return result;
        }
    }
    return false;
}

const directive: FunctionDirective<HTMLElement, Slot | undefined> = (element, binding) => {
    if (element instanceof HTMLElement) {
        const solt = binding.value;
        if (isSlot(solt)) {
            if (element.style.display === "none") {
                element.style.display = '';
            }
        }
        else {
            element.style.display = "none";
        }
    }
};

export default directive;