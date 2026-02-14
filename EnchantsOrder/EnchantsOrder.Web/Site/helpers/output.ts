import type { ManualChunksOption, OutputOptions } from "rollup";

export default function getOutputOptions(mode: string, manualChunks?: ManualChunksOption): OutputOptions {
    const output: OutputOptions = { manualChunks };
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