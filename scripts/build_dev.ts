#!/usr/bin/env -S deno run --allow-read --allow-write --allow-run

import { ensureDir } from "jsr:@std/fs";
import { dirname, fromFileUrl, join } from "jsr:@std/path";

const __dirname = dirname(fromFileUrl(Deno.mainModule));
const toolchainRepoPath = join(__dirname, "..", "..", "toolchain");
const assetsPath = join(__dirname, "..", "Assets", "Rivet", "Editor", "Native");

const platform = Deno.build.os;
const arch = Deno.build.arch;

const target = {
    os: platform === "darwin" ? "macos" : platform,
    arch: arch === "aarch64" ? "arm64" : arch,
    ext: platform === "windows" ? "dll" : platform === "darwin" ? "dylib" : "so",
    rustTarget: `${arch === "aarch64" ? "aarch64" : "x86_64"}-${
        platform === "windows" ? "pc-windows-gnu" :
        platform === "darwin" ? "apple-darwin" :
        "unknown-linux-gnu"
    }`,
};

async function runCommand(cmd: string[], cwd?: string): Promise<void> {
    const command = new Deno.Command(cmd[0], {
        args: cmd.slice(1),
        cwd,
        stdout: "inherit",
        stderr: "inherit",
    });
    const { code } = await command.output();
    if (code !== 0) throw new Error(`Command failed: ${cmd.join(" ")}`);
}

async function copyFile(src: string, dest: string): Promise<void> {
    await ensureDir(dirname(dest));
    await Deno.copyFile(src, dest);
}

// Build client
await runCommand(["cargo", "build", "--release"], toolchainRepoPath);

// Copy FFI library
const ffiSrc = join(
    toolchainRepoPath,
    "target",
    "release",
    `librivet_toolchain_ffi.${target.ext}`,
);
const ffiDest = join(
    assetsPath,
    `librivet_toolchain_ffi_${target.os}_${target.arch}.${target.ext}`,
);
await copyFile(ffiSrc, ffiDest);

console.log("Library copied successfully.");
