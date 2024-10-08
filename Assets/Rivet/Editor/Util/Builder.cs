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
        /// Build target for building for the current OS.
        /// 
        /// Used for player & dev server builds.
        /// </summary>
        /// <returns>Returns the path to the built player executable.</returns>
        public static BuildTarget GetLocalBuildTarget()
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
                    RivetLogger.Log("Unsupported platform for build");
                    return BuildTarget.StandaloneWindows64; // Default to Windows as fallback
            }
        }

        public static string BuildDevPlayer()
        {
            // Check if the target platform is supported
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, GetLocalBuildTarget()))
            {
                throw new Exception(
                    $"{GetLocalBuildTarget()} build support is not installed. Please install it from the Unity Hub to proceed with the build process."
                );
            }

            // Ensure a scene is included
            if (EditorBuildSettings.scenes.Length == 0)
            {
                throw new Exception("No scenes in build settings. Please add a scene under File > Build Settings. The first build is the scene the server will run.");
            }

            // Configure build settings
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = Path.Combine(ProjectRoot(), "Builds", "Development", "Player", GetPlatformArchFolder(GetLocalBuildTarget()), GetBuildName("Player", GetLocalBuildTarget())),
                target = GetLocalBuildTarget(),
                options = BuildOptions.Development | BuildOptions.AllowDebugging
            };

            // Build the player
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildResult result = report.summary.result;

            if (result == BuildResult.Succeeded)
            {
                string executablePath = FindPlayerExecutablePath(buildPlayerOptions.locationPathName, buildPlayerOptions.target);
                RivetLogger.Log("Dev player build succeeded: " + executablePath);
                return executablePath;
            }
            else
            {
                throw new Exception("Dev player build failed. Check console for errors.");
            }
        }

        /// <summary>
        /// Builds and runs multiple instances of the development player, each with its own log file.
        /// </summary>
        /// <param name="instanceCount">The number of player instances to run.</param>
        public static void BuildAndRunMultipleDevPlayers(int instanceCount)
        {
            string playerPath;
            try {
                playerPath = BuildDevPlayer();
            } catch (Exception e) {
                EditorUtility.DisplayDialog("Player Build Failed", e.Message, "Dismiss");
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
                    RivetLogger.Log($"Started dev player instance {i + 1} with log file: {logFilePath}");
                }
                catch (Exception e)
                {
                    RivetLogger.Error($"Failed to start dev player instance {i + 1}: {e.Message}");
                }
            }
        }

        // MARK: Run Game Server
        /// <summary>
        /// Builds a server used for local development.
        /// </summary>
        /// <returns>Returns the task config to run the server.</returns>
        public static string BuildDevDedicatedServer()
        {
            // Check if the target platform is supported
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, GetLocalBuildTarget()))
            {
                throw new Exception($"{GetLocalBuildTarget()} build support is not installed. Please install it from the Unity Hub to proceed with the build process.");
            }

            // Ensure a scene is included
            if (EditorBuildSettings.scenes.Length == 0)
            {
                throw new Exception("No scenes in build settings. Please add a scene under File > Build Settings. The first build is the scene the server will run.");
            }

            // Configure build settings
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = Path.Combine(ProjectRoot(), "Builds", "Development", "DedicatedServer", GetPlatformArchFolder(GetLocalBuildTarget()), GetBuildName("DedicatedServer", GetLocalBuildTarget(), true)),
                target = GetLocalBuildTarget(),
                options = BuildOptions.Development | BuildOptions.CompressWithLz4 | BuildOptions.EnableHeadlessMode,
                subtarget = (int)StandaloneBuildSubtarget.Server
            };

            return FindServerExecutablePath(buildPlayerOptions.locationPathName, buildPlayerOptions.target);

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
                throw new Exception("Dedicated server build failed. Ensure the \"Dedicated Server\" Unity module is installed for your platform in the Unity Hub. Check the console for errors.");
            }
        }

        public static string BuildReleaseDedicatedServer()
        {
            // Ensure a scene is included
            if (EditorBuildSettings.scenes.Length == 0)
            {
                throw new Exception("No scenes in build settings. Please add a scene under File > Build Settings. The first build is the scene the server will run.");
            }

            // Check if Linux build support is installed
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64))
            {
                EditorUtility.DisplayDialog(
                    "Linux Build Support Missing",
                    "Linux build support is not installed. Please install it from the Unity Hub to proceed with the build and deploy process.",
                    "Dismiss"
                );
                throw new Exception("Linux build support is not installed");
            }

            // Configure build settings
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetScenePaths(),
                locationPathName = Path.Combine(ProjectRoot(), "Builds", "Release", "DedicatedServer", GetPlatformArchFolder(BuildTarget.StandaloneLinux64), GetBuildName("DedicatedServer", BuildTarget.StandaloneLinux64, true)),
                target = BuildTarget.StandaloneLinux64,
                options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode,
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
                throw new Exception("Production server build failed. Ensure the \"Dedicated Server\" Unity module is installed for Linux in the Unity Hub. Check console for errors.");
            }
        }

        public static string FindServerExecutablePath(string serverPath, BuildTarget buildTarget)
        {
            string productName = Application.productName;
            string executableFile;

            switch (buildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    // Check for .app bundle first
                    executableFile = Path.Combine(serverPath, "Contents", "MacOS", productName);
                    if (!File.Exists(executableFile))
                    {
                        // If .app bundle doesn't exist, check for standalone executable
                        executableFile = Path.Combine(serverPath, productName);
                    }
                    break;
                case BuildTarget.StandaloneLinux64:
                    executableFile = serverPath;
                    break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    executableFile = serverPath;
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
                executablePath = Path.Combine(buildPath, "Contents", "MacOS", productName);
            }
            else
            {
                executablePath = Path.Combine(buildPath, productName);
                if (buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64)
                {
                    executablePath += ".exe";
                }
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

        public static string GetBuildName(string baseName, BuildTarget target, bool isServer = false)
        {
            if (target == BuildTarget.StandaloneOSX && !isServer)
            {
                return baseName + ".app";
            } else if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64) {
                return baseName + ".exe";
            }
            return baseName;
        }

        private static string GetPlatformArchFolder(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                    return "Windows_x86_32";
                case BuildTarget.StandaloneWindows64:
                    return "Windows_x86_64";
                case BuildTarget.StandaloneOSX:
                    return "macOS_Universal";
                case BuildTarget.StandaloneLinux64:
                    return "Linux_x86_64";
                default:
                    return "Unknown";
            }
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