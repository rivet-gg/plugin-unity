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
    prefix: platform === "windows" ? "" : "lib",
};

async function copyFile(src: string, dest: string): Promise<void> {
    await ensureDir(dirname(dest));
    await Deno.copyFile(src, dest);
}

// Build client
// const buildCommand = new Deno.Command("cargo", {
//     args: ["+nightly", "build", "--package", "rivet-toolchain-ffi"],
//     env: {
//       "DYLD_INSERT_LIBRARIES": "/Users/nathan/.rustup/toolchains/nightly-aarch64-apple-darwin/lib/rustlib/aarch64-apple-darwin/lib/librustc-nightly_rt.asan.dylib",
//       "RUSTFLAGS": "-Zsanitizer=address"
//     },
//     cwd: toolchainRepoPath,
//     stdout: "inherit",
//     stderr: "inherit",
// });
const buildCommand = new Deno.Command("cargo", {
    args: ["build", "--package", "rivet-toolchain-ffi"],
    cwd: toolchainRepoPath,
    stdout: "inherit",
    stderr: "inherit",
});
const { code } = await buildCommand.output();
if (code !== 0) throw new Error("Command failed: cargo build --package rivet-toolchain-ffi");

// Copy FFI library
const ffiSrc = join(
    toolchainRepoPath,
    "target",
    "debug",
    `${target.prefix}rivet_toolchain_ffi.${target.ext}`,
);
const ffiDest = join(
    assetsPath,
    `${target.prefix}rivet_toolchain_ffi_${target.os}_${target.arch}.${target.ext}`,
);
console.log(`Copying ${ffiSrc} -> ${ffiDest}`);
await copyFile(ffiSrc, ffiDest);

console.log(`Finished building`);
