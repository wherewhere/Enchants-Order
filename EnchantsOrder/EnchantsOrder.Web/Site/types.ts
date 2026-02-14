/// <reference types="./env" />
import type { IEnchant } from "./helpers/enchantment"
export interface IEnchantment {
    readonly Name: string;
    readonly Level: number;
    readonly Weight: number;
    readonly Experience: number;
}
export type OrderingResults = {
    readonly Penalty: number;
    readonly MaxExperience: number;
    readonly TotalExperience: number;
    readonly Steps: IEnchantment[][];
    readonly IsTooExpensive: boolean;
    readonly IsTooManyPenalty: boolean;
}
export type Exports = {
    Ordering(wantedList: IEnchant[], initialPenalty: number): Promise<OrderingResults>;
}