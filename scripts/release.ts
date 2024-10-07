#!/usr/bin/env -S deno run -A

import { copy } from "jsr:@std/fs";
import { resolve } from "jsr:@std/path";
import { assert } from "jsr:@std/assert";
import { S3Bucket } from "https://deno.land/x/s3@0.5.0/mod.ts";
import { buildCross } from "../../toolchain/scripts/build/build_cross.ts";

function getRequiredEnvVar(name: string): string {
    const value = Deno.env.get(name);
    if (!value) {
        throw new Error(`Required environment variable ${name} is not set`);
    }
    return value;
}

const assetVersion = getRequiredEnvVar("ASSET_VERSION");
const awsAccessKeyId = getRequiredEnvVar("AWS_ACCESS_KEY_ID");
const awsSecretAccessKey = getRequiredEnvVar("AWS_SECRET_ACCESS_KEY");

assert(/^v(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$/.test(assetVersion), "ASSET_VERSION must be a valid semantic version starting with 'v'");

const REPO_DIR = resolve(import.meta.dirname!, "..");
const OUTPUT_DIR = Deno.env.get("OUTPUT_DIR") ?? await Deno.makeTempDir({ prefix: "rivet-plugin-unity-" });
console.log("Work dir:", OUTPUT_DIR);
const TEMP_DIR = resolve(OUTPUT_DIR, "rivet-plugin-unity");
const BUILD_DIR = resolve(OUTPUT_DIR, "build");
const ZIP_PATH = resolve(OUTPUT_DIR, "rivet-plugin-unity.zip");

async function buildCrossPlatform() {
    // We build this in the repo dir in order to make sure we use the build cache
    console.log("Building cross-platform libraries");
    try {
      await buildCross(BUILD_DIR, ["rivet-toolchain-ffi"]);
    } catch (err) {
      throw new Error(`Failed to build cross-platform libraries: ${err}`);
    }
}

async function copyFilesToTemp() {
    console.log("Copying files to temp directory");
    await copy(REPO_DIR, TEMP_DIR, { overwrite: true });
}

async function removeGitIgnoredFiles() {
    console.log("Removing git ignored files...");
    const gitClean = new Deno.Command("git", {
        args: ["clean", "-X", "-f"],
        cwd: TEMP_DIR,
        stdout: "inherit",
        stderr: "inherit",
    });
    const gitCleanOutput = await gitClean.output();
    assert(gitCleanOutput.success, "Failed to remove git ignored files");
}

async function removeUnnecessaryFiles() {
    console.log("Removing unnecessary files...");

    // Preserve only Assets & LICENSE at the root
    for await (const entry of Deno.readDir(TEMP_DIR)) {
        const path = resolve(TEMP_DIR, entry.name);
        if (entry.name !== "Assets" && entry.name !== "LICENSE") {
            if (entry.isDirectory) {
                await Deno.remove(path, { recursive: true });
            } else {
                await Deno.remove(path);
            }
        }
    }

    // Preserve only the Rivet folder within Assets
    const assetsPath = resolve(TEMP_DIR, "Assets");
    for await (const entry of Deno.readDir(assetsPath)) {
        const path = resolve(assetsPath, entry.name);
        if (entry.name !== "Rivet") {
            if (entry.isDirectory) {
                await Deno.remove(path, { recursive: true });
            } else {
                await Deno.remove(path);
            }
        }
    }
}

async function copyDylibs() {
    console.log("Copying dylibs to Assets/Rivet/Editor/Native");

    const nativeDir = resolve(TEMP_DIR, "Assets", "Rivet", "Editor", "Native");
    await Deno.mkdir(nativeDir, { recursive: true });

    const platforms = [
        { os: "macos", arch: "arm64", ext: "dylib", prefix: "lib" },
        // { os: "macos", arch: "x86_64", ext: "dylib", prefix: "lib" },
        { os: "windows", arch: "x86_64", ext: "dll", prefix: "" },
        // { os: "linux", arch: "x86_64", ext: "so", prefix: "lib" },
    ];

    for (const { os, arch, ext, prefix } of platforms) {
        const src = resolve(BUILD_DIR, `${os}_${arch}`, `${prefix}rivet_toolchain_ffi.${ext}`);
        const dest = resolve(nativeDir, `${prefix}rivet_toolchain_ffi_${os}_${arch}.${ext}`);
        console.log(`Copying ${src} -> ${dest}`);
        await Deno.copyFile(src, dest);
    }
}

async function templateFiles() {
  const packageJsonPath = resolve(TEMP_DIR, "Assets", "Rivet", "package.json");
  const packageJson = await Deno.readTextFile(packageJsonPath);
  await Deno.writeTextFile(packageJsonPath, packageJson.replace("{{VERSION}}", assetVersion.slice(1)));
}

async function generateZipFile() {
    console.log("Generating zip file");
    const zipOutput = await (new Deno.Command("zip", {
        args: ["-r", ZIP_PATH, "."],
        cwd: TEMP_DIR,
        stdout: "inherit",
        stderr: "inherit",
    })).output();
    assert(zipOutput.success, "Failed to create zip");
    console.log(`Zip file created: ${ZIP_PATH}`);
}

async function uploadZipToS3(): Promise<{ zipUrl: string }> {
    console.log("Uploading zip file to S3");
    const bucket = new S3Bucket({
        accessKeyID: awsAccessKeyId,
        secretKey: awsSecretAccessKey,
        bucket: "rivet-releases",
        region: "auto",
        endpointURL: "https://2a94c6a0ced8d35ea63cddc86c2681e7.r2.cloudflarestorage.com/rivet-releases",
    });

    const zipObjectKey = `plugin-unity/${assetVersion}/rivet-plugin-unity.zip`;

    const zipFileData = await Deno.readFile(ZIP_PATH);

    await bucket.putObject(zipObjectKey, zipFileData);

    console.log(`Uploaded zip file to S3: ${zipObjectKey}`);

    return {
        zipUrl: `https://releases.rivet.gg/${zipObjectKey}`,
    };
}


async function main() {
    await buildCrossPlatform();
    await copyFilesToTemp();
    await removeGitIgnoredFiles();
    await removeUnnecessaryFiles();
    await copyDylibs();
    await templateFiles();
    await generateZipFile();
    const { zipUrl } = await uploadZipToS3();
    console.log('Uploaded', zipUrl);
}

if (import.meta.main) {
    main();
}
