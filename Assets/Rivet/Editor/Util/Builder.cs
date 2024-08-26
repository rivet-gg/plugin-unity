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

        /// <summary>
        /// Builds a development player for the host OS.
        /// </summary>
        /// <returns>Returns the path to the built player executable.</returns>
        public static BuildTarget GetPlayerBuildTarget()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    return BuildTarget.StandaloneWindows64;
                case RuntimePlatform.OSXEditor:
                    return BuildTarget.StandaloneOSX;
                case RuntimePlatform.LinuxEditor:
                    return BuildTarget.StandaloneLinux64;
                default:
                    Debug.LogError("Unsupported platform for player build");
                    return BuildTarget.StandaloneWindows64; // Default to Windows as fallback
            }
        }

        public static string? BuildDevPlayer()
        {
            // Check if the target platform is supported
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, GetPlayerBuildTarget()))
            {
                RivetLogger.Error($"{GetPlayerBuildTarget()} build support is not installed");
                EditorUtility.DisplayDialog(
                    $"{GetPlayerBuildTarget()} Build Support Missing",
                    $"{GetPlayerBuildTarget()} build support is not installed. Please install it from the Unity Hub to proceed with the build process.",
                    "Dismiss"
                );
                return null;
            }

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
                locationPathName = Path.Combine(ProjectRoot(), "Builds", "DevPlayer", GetBuildName("DevPlayer")),
                target = GetPlayerBuildTarget(),
                options = BuildOptions.Development | BuildOptions.AllowDebugging
            };

            // Build the player
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildResult result = report.summary.result;

            if (result == BuildResult.Succeeded)
            {
                string executablePath = FindPlayerExecutablePath(buildPlayerOptions.locationPathName, buildPlayerOptions.target);
                Debug.Log("Dev player build succeeded: " + executablePath);
                return executablePath;
            }
            else
            {
                Debug.LogError("Dev player build failed");
                return null;
            }
        }

        /// <summary>
        /// Builds and runs multiple instances of the development player, each with its own log file.
        /// </summary>
        /// <param name="instanceCount">The number of player instances to run.</param>
        public static void BuildAndRunMultipleDevPlayers(int instanceCount)
        {
            string? playerPath = BuildDevPlayer();
            if (playerPath == null)
            {
                Debug.LogError("Failed to build dev player. Cannot run instances.");
                return;
            }

            string logDirectory = Path.Combine(ProjectRoot(), "Logs", "DevPlayers");
            Directory.CreateDirectory(logDirectory);

            for (int i = 0; i < instanceCount; i++)
            {
                try
                {
                    string logFilePath = Path.Combine(logDirectory, $"DevPlayer_{i + 1}.log");
                    var arguments = $"-screen-fullscreen 0 -logFile \"{logFilePath}\"";
                    var startInfo = new System.Diagnostics.ProcessStartInfo(playerPath)
                    {
                        Arguments = arguments,
                        UseShellExecute = false
                    };

                    System.Diagnostics.Process.Start(startInfo);
                    Debug.Log($"Started dev player instance {i + 1} with log file: {logFilePath}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to start dev player instance {i + 1}: {e.Message}");
                }
            }
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
        public static string? BuildDevDedicatedServer()
        {
            // Check if the target platform is supported
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, GetGameServerBuildTarget()))
            {
                RivetLogger.Error($"{GetGameServerBuildTarget()} build support is not installed");
                EditorUtility.DisplayDialog(
                    $"{GetGameServerBuildTarget()} Build Support Missing",
                    $"{GetGameServerBuildTarget()} build support is not installed. Please install it from the Unity Hub to proceed with the build process.",
                    "Dismiss"
                );
                return null;
            }

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
                locationPathName = Path.Combine(ProjectRoot(), "Builds", "DevDedicatedServer", GetBuildName("DevDedicatedServer")),
                target = GetGameServerBuildTarget(),
                options = BuildOptions.Development | BuildOptions.CompressWithLz4,
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

                return FindServerExecutablePath(buildPlayerOptions.locationPathName, buildPlayerOptions.target);
            }
            else
            {
                RivetLogger.Error("Dedicated server build failed.");
                return null;
            }
        }

        public static string? BuildReleaseDedicatedServer()
        {
            // Ensure a scene is included
            if (EditorBuildSettings.scenes.Length == 0)
            {
                RivetLogger.Error("No scenes in build settings. Please add at least one scene.");
                return null;
            }

            // Check if Linux build support is installed
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64))
            {
                RivetLogger.Error("Linux build support is not installed");
                EditorUtility.DisplayDialog(
                    "Linux Build Support Missing",
                    "Linux build support is not installed. Please install it from the Unity Hub to proceed with the build and deploy process.",
                    "Dismiss"
                );
                return null;
            }

            // Configure build settings
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = Path.Combine(ProjectRoot(), "Builds", "ReleaseDedicatedServer", GetBuildName("ReleaseDedicatedServer")),
                target = BuildTarget.StandaloneLinux64,
                options = BuildOptions.CompressWithLz4HC,
                subtarget = (int)StandaloneBuildSubtarget.Server
            };

            // Build the server
            RivetLogger.Log("Building production server...");
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildResult result = report.summary.result;

            if (result == BuildResult.Succeeded)
            {
                RivetLogger.Log("Production server build completed successfully.");
                return FindServerExecutablePath(buildPlayerOptions.locationPathName, buildPlayerOptions.target);
            }
            else
            {
                RivetLogger.Error("Production server build failed.");
                return null;
            }
        }

        public static string FindServerExecutablePath(string serverPath, BuildTarget buildTarget)
        {
            string productName = Application.productName;
            string executableFile;

            switch (buildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    executableFile = Path.Combine(serverPath, productName, productName);
                    break;
                case BuildTarget.StandaloneLinux64:
                    executableFile = serverPath;
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    executableFile = Path.Combine(serverPath, $"{productName}.exe");
                    break;
                default:
                    throw new ArgumentException($"Unsupported build target: {buildTarget}");
            }

            if (!File.Exists(executableFile))
            {
                throw new Exception($"Executable {productName} not found at {executableFile}");
            }

            return executableFile;
        }

        public static string FindPlayerExecutablePath(string buildPath, BuildTarget buildTarget)
        {
            string productName = Application.productName;
            string executablePath;

            if (buildTarget == BuildTarget.StandaloneOSX)
            {
                string lastPathComponent = Path.GetFileName(buildPath);
                executablePath = Path.Combine($"{buildPath}.app", "Contents", "MacOS", productName);
            }
            else
            {
                // For other platforms, use the existing logic
                executablePath = Path.Combine(buildPath, productName);
            }

            if (!File.Exists(executablePath))
            {
                throw new Exception($"Executable {productName} not found at {executablePath}");
            }

            return executablePath;
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

        public static string GetBuildName(string baseName)
        {
            // TODO: Check that we don't need .exe for Windows
            var buildName = baseName;
            return buildName;
            // switch (GetGameServerBuildTarget())
            // {
            //     case BuildTarget.StandaloneWindows:
            //     case BuildTarget.StandaloneWindows64:
            //         return $"{buildName}.exe";
            //     case BuildTarget.StandaloneOSX:
            //         return $"{buildName}.app";
            //     case BuildTarget.StandaloneLinux64:
            //         return $"{buildName}";
            //     default:
            //         return buildName;
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