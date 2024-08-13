using System;
using System.IO;
using System.Threading.Tasks;
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

        /// <summary>
        /// Builds a server used for local development.
        /// </summary>
        /// <returns>Returns the task config to run the server.</returns>
        public static string? BuildDevServer()
        {
            // Ensure a scene is included
            if (EditorBuildSettings.scenes.Length == 0)
            {
                RivetLogger.Error("No scenes in build settings. Please add at least one scene.");
                return null;
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

                return FindExecutablePath(buildPlayerOptions.locationPathName);
            }
            else
            {
                RivetLogger.Error("Dedicated server build failed.");
                return null;
            }
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