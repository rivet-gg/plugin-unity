using System;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Rivet.Editor.Util
{
    public static class Builder
    {
        public static string GetUnityEditorPath()
        {
            string editorPath = EditorApplication.applicationPath;

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                // On macOS point to the actual Unity binary inside the .app
                editorPath = Path.Combine(editorPath, "Contents/MacOS/Unity");
            }

            return editorPath;
        }

        // MARK: Run Game Server
        public static BuildTarget GetGameServerBuildTarget()
        {
            // TODO:
            return BuildTarget.StandaloneOSX;
        }

        public static void BuildAndRunServer()
        {
            // Ensure a scene is included
            if (EditorBuildSettings.scenes.Length == 0)
            {
                RivetLogger.Error("No scenes in build settings. Please add at least one scene.");
                return;
            }

            // Configure build settings
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = Path.Combine(ProjectRoot(), "Builds", "DedicatedServer", GetBuildName()),
                target = GetGameServerBuildTarget(),
                subtarget = (int)StandaloneBuildSubtarget.Server
            };

            // // Speed up builds for development
            // var devOptimizations = true;
            // if (devOptimizations)
            // {
            //     buildPlayerOptions.options |= BuildOptions.Development;
            //     buildPlayerOptions.options |= BuildOptions.CompressWithLz4;
            //     EditorUserBuildSettings.compressFilesInPackage = false;

            //     // TODO: Is this right?
            //     buildPlayerOptions.options &= ~BuildOptions.StrictMode;

            //     // buildPlayerOptions.options |= BuildOptions.BuildScriptsOnly;

            //     // TODO: Is this right?
            //     // Disable error checking for faster builds
            //     buildPlayerOptions.extraScriptingDefines = new string[] { "DISABLE_IMPLICIT_CHECKS", "DISABLE_WARNINGS" };
            // }


            // Build the server
            RivetLogger.Log("Building dedicated server...");
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var result = report.summary.result;

            if (result == BuildResult.Succeeded)
            {
                RivetLogger.Log("Dedicated server build completed successfully.");
                LaunchServer(buildPlayerOptions.locationPathName);
            }
            else
            {
                RivetLogger.Error("Dedicated server build failed.");
            }
        }

        public static async void LaunchServer(string buildPath)
        {
            // Add path to binary for macOS
            string executablePath = FindExecutablePath(buildPath);

            // HACK: Show term instead of running inline
            // TODO: Better logs dir
            // string logPath = Path.Combine(Path.GetDirectoryName(executablePath), "server_log.txt");
            await new ToolchainTask("show_term", new JObject
            {
                ["command"] = executablePath,
                // ["args"] = new JArray { "-batchmode", "-nographics", "-logFile", logPath, "-server" },
                ["args"] = new JArray { "-batchmode", "-nographics", "-logFile", "-server" },
            }).RunAsync();

            // OLD:
            // string logPath = Path.Combine(Path.GetDirectoryName(serverPath), "server_log.txt");

            // ProcessStartInfo startInfo = new ProcessStartInfo
            // {
            //     FileName = serverPath,
            //     Arguments = $"-batchmode -nographics -logFile \"{logPath}\"",
            //     UseShellExecute = false,
            //     CreateNoWindow = true
            // };

            // RivetLogger.Log($"Launching dedicated server with command: {startInfo.FileName} {startInfo.Arguments}");

            // try
            // {
            //     using (Process serverProcess = Process.Start(startInfo))
            //     {
            //         RivetLogger.Log("Dedicated server launched successfully.");
            //     }
            // }
            // catch (System.Exception e)
            // {
            //     RivetLogger.Error($"Failed to launch dedicated server: {e.Message}");
            // }
        }

        public static string FindExecutablePath(string serverPath)
        {
            string productName = Application.productName;
            string executableFile = Path.Combine(serverPath, productName);

            if (!File.Exists(executableFile))
            {
                throw new Exception($"Executable {productName} not found in {serverPath}");
            }

            return executableFile;
        }

        public static string[] GetScenePaths()
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }
            return scenes;
        }

        public static string GetBuildName()
        {
            // TODO: Check that we don't need .exe for Windows
            var serverName = "DedicatedServer";
            return serverName;
            // switch (GetGameServerBuildTarget())
            // {
            //     case BuildTarget.StandaloneWindows:
            //     case BuildTarget.StandaloneWindows64:
            //         return $"{serverName}.exe";
            //     case BuildTarget.StandaloneOSX:
            //         return $"{serverName}.app";
            //     case BuildTarget.StandaloneLinux64:
            //         return $"{serverName}";
            //     default:
            //         return serverName;
            // }
        }

        // MARK: Utils
        // TODO: move to utils file
        public static string ProjectRoot()
        {
            var dataPath = Application.dataPath;
            return Directory.GetParent(dataPath).FullName;
        }
    }
}