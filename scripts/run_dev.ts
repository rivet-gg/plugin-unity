#!/usr/bin/env -S deno run --allow-net --allow-env --allow-read --allow-write --allow-run

import "./build_dev.ts";

import { glob } from "npm:glob";
import { dirname, fromFileUrl, join } from "jsr:@std/path";

const __dirname = dirname(fromFileUrl(import.meta.url));
const projectDir = `${__dirname}/..`;

// Determine the Unity executable path based on the platform
let unityPath: string;
switch (Deno.build.os) {
    case "windows":
        unityPath =
            "C:\\Program Files\\Unity\\Hub\\Editor\\*\\Editor\\Unity.exe";
        break;
    case "darwin":
        unityPath =
            "/Applications/Unity/Hub/Editor/*/Unity.app/Contents/MacOS/Unity";
        break;
    case "linux":
        unityPath = "/opt/unity/editor/*/Editor/Unity";
        break;
    default:
        throw new Error("Unsupported operating system");
}

// Find the latest Unity version installed
const unityVersions = await glob(unityPath);
if (unityVersions.length === 0) {
    throw new Error("No Unity installation found");
}
unityVersions.sort();
const latestUnity = unityVersions[unityVersions.length - 1];

// Delete the multiple project file lock
const lockFilePath = join(projectDir, "Temp", "UnityLockfile");
try {
    await Deno.remove(lockFilePath);
    console.log("Deleted Unity lock file");
} catch (error) {
    if (!(error instanceof Deno.errors.NotFound)) {
        console.warn("Failed to delete Unity lock file:", error);
    }
}

// Construct the command to open Unity
const command = new Deno.Command(latestUnity, {
    args: ["-projectPath", projectDir, "-logfile", "-"],
    stdin: "inherit",
    stdout: "inherit",
    stderr: "inherit",
});

// Run the command without detaching
const process = command.spawn();

// Wait for the process to exit
const status = await process.status;

if (!status.success) {
    console.error("Failed to open Unity project");
    Deno.exit(status.code);
}

// lldb -o 'process launch -- -projectPath /Users/nathan/rivet/plugin-unity -logfile -' /Applications/Unity/Hub/Editor/2022.3.38f1/Unity.app/Contents/MacOS/Unity
