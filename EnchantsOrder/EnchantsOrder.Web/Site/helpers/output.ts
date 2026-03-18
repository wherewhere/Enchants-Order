import type { CodeSplittingOptions, OutputOptions } from "rolldown";

export default function getOutputOptions(mode: string, codeSplitting?: boolean | CodeSplittingOptions): OutputOptions {
    const output: OutputOptions = { codeSplitting };
    if (mode === "publish") {
        output.assetFileNames = "assets/[name].[hash].[ext]";
        output.chunkFileNames = "assets/[name].[hash].js";
        output.entryFileNames = "assets/[name].[hash].js";
    }
    else {
        output.assetFileNames = "assets/[name].[ext]";
        output.chunkFileNames = "assets/[name].js";
        output.entryFileNames = "assets/[name].js";
    }
    return output;
}