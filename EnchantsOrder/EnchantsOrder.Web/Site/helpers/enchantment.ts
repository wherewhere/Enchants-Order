import enchants_zh from "../../../EnchantsOrder.Demo/Assets/Enchants/Enchants.zh-CN.json";
import enchants_en from "../../../EnchantsOrder.Demo/Assets/Enchants/Enchants.en-US.json";
const enchants = {
    "zh-CN": enchants_zh,
    "en-US": enchants_en
};
type Enchant = {
    hidden?: boolean,
    levelMax: number,
    weight: number,
    incompatible?: string[],
    items: string[]
}
export function getEnchantments(locale: keyof typeof enchants) {
    const json: { [key: string]: Enchant } = enchants[locale] || enchants["en-US"];
    const enchantments: Enchantment[] = [];
    for (const name in json) {
        const enchantment = json[name];
        enchantments.push({
            name,
            level: enchantment.levelMax,
            weight: enchantment.weight,
            hidden: enchantment.hidden,
            items: enchantment.items,
            incompatible: enchantment.incompatible,
            get experience() {
                return this.level * this.weight;
            }
        });
    }
    return enchantments;
}
export interface IEnchant {
    readonly level: number,
    readonly weight: number,
    readonly name: string
};
export type Enchantment = {
    name: string;
    level: number;
    weight: number;
    hidden?: boolean;
    items: string[];
    incompatible?: string[];
    readonly experience: number;
}