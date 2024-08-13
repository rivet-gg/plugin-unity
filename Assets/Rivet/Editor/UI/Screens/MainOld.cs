using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.IO;
using UnityEditor.Build.Reporting;

// namespace Rivet.Editor.UI.Screens.Main
// {
//     enum SelectedTab
//     {
//         Setup = 0, Develop = 1, Deploy = 2, Settings = 3,
//     }

//     enum EnvironmentType
//     {
//         Local = 0, Remote = 1,
//     }

//     enum DeploySteps
//     {
//         Both = 0, GameServer = 1, Backend = 2,
//     }

//     public class Window : RivetPluginWindow.IState
//     {
//         private RivetPluginWindow _pluginWindow;
//         private BootstrapData _bootstrapData;
//         private SelectedTab _selectedTab = SelectedTab.Setup;

//         // MARK: Develop (Environment)
//         private bool _environmentFoldout = true;
//         private EnvironmentType _environmentType = EnvironmentType.Local;
//         private string? _remoteEnvironmentId;
//         private BackendEnvironment? _remoteEnvironment
//         {
//             get
//             {
//                 return _bootstrapData.BackendEnvironments[_remoteEnvironmentIndex];
//             }
//         }
//         private int _remoteEnvironmentIndex
//         {
//             get
//             {
//                 var idx = _bootstrapData.BackendEnvironments.FindIndex(x => x.EnvironmentId == _remoteEnvironmentId);

//                 // Default to 0 if not found
//                 return idx >= 0 ? idx : 0;
//             }
//             set
//             {
//                 if (value >= 0 && value < _bootstrapData.BackendEnvironments.Count)
//                 {
//                     _remoteEnvironmentId = _bootstrapData.BackendEnvironments[value].EnvironmentId;
//                 }
//             }
//         }

//         // MARK: Develop (Local Game Server)
//         private bool _localGameServerFoldout = true;

//         // MARK: Develop (Backend)
//         private bool _backendFoldout = true;

//         // MARK: Deploy
//         private DeploySteps _deploySteps = DeploySteps.Both;

//         public void OnEnter(RivetPluginWindow pluginWindow)
//         {
//             _pluginWindow = pluginWindow;
//             _ = GetBootstrapData();
//         }

//         private async Task GetBootstrapData()
//         {
//             var result = await new ToolchainTask("get_bootstrap_data", new JObject()).RunAsync();
//             if (result is ResultOk<JObject>)
//             {
//                 _bootstrapData = result.Data.ToObject<BootstrapData>();
//                 ExtensionData.ApiEndpoint = _bootstrapData.ApiEndpoint;
//             }
//         }

//         public void OnGUI()
//         {
//             GUILayout.BeginVertical();

//             // Tabs
//             string[] toolbarStrings = { "Setup", "Develop", "Deploy", "Settings" };
//             _selectedTab = (SelectedTab)GUILayout.Toolbar((int)_selectedTab, toolbarStrings);

//             // Draw body
//             GUILayout.BeginVertical();
//             switch (_selectedTab)
//             {
//                 case SelectedTab.Setup:
//                     DrawSetup();
//                     break;
//                 case SelectedTab.Develop:
//                     DrawDevelop();
//                     break;
//                 case SelectedTab.Deploy:
//                     DrawDeploy();
//                     break;
//                 case SelectedTab.Settings:
//                     DrawSettings();
//                     break;
//                 default:
//                     throw new System.Exception("unreachable");
//             }
//             GUILayout.EndVertical();

//             GUILayout.EndVertical();
//         }

//         // MARK: Tab: Setup
//         private void DrawSetup()
//         {
//             GUILayout.Label("Setup");
//             GUILayout.Label("TODO: Check DGS is installed");
//             GUILayout.Label("TODO: Check Docker is installed");
//             GUILayout.Label("TODO: Check scenes are added to build settings");
//         }

//         // MARK: Tab: Develop
//         private void DrawDevelop()
//         {
//             // MARK: Environment
//             _environmentFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_environmentFoldout, "Environment");
//             if (_environmentFoldout)
//             {
//                 EditorGUILayout.LinkButton("Reload");

//                 GUILayout.Label("Configure which backend to connect to. [Learn More]");

//                 string[] envTypes = { "Local", "Remote" };
//                 _environmentType = (EnvironmentType)EditorGUILayout.Popup("Type", (int)_environmentType, envTypes);

//                 if (_environmentType == EnvironmentType.Remote)
//                 {
//                     var options = _bootstrapData.BackendEnvironments.Select(x => x.DisplayName).ToList();
//                     options.Add("+ Create Environment");
//                     var newEnvIndex = EditorGUILayout.Popup("Environment", _remoteEnvironmentIndex, options.ToArray());

//                     // Open create new env
//                     if (newEnvIndex == _bootstrapData.BackendEnvironments.Count)
//                     {
//                         Application.OpenURL($"https://hub.rivet.gg/games/{_bootstrapData.GameId}/backend?modal=create-environment");
//                     }
//                     else
//                     {
//                         _remoteEnvironmentIndex = newEnvIndex;
//                     }
//                 }


//                 EditorGUILayout.HelpBox("Make sure you've deployed your game server. Using different versions can cause unexpected errors. [Go to deploy]", MessageType.Warning);
//             }
//             EditorGUILayout.EndFoldoutHeaderGroup();

//             // MARK: Local Game Server
//             _localGameServerFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_localGameServerFoldout, "Local Game Server");
//             if (_localGameServerFoldout)
//             {
//                 GUILayout.Label("Requires restart when game edited.");
//                 GUILayout.Label("TODO: Will take a long time to build the first time it runs");

//                 EditorGUILayout.BeginHorizontal();
//                 if (GUILayout.Button("Start")) _ = OnLocalGameServerStart();
//                 if (GUILayout.Button("Stop")) _ = OnLocalGameServerStop();
//                 if (GUILayout.Button("Restart")) _ = OnLocalGameServerStart();
//                 EditorGUILayout.EndHorizontal();

//                 EditorGUILayout.LinkButton("Show Logs");
//             }
//             EditorGUILayout.EndFoldoutHeaderGroup();

//             // MARK: Backend
//             _backendFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_backendFoldout, "Backend");
//             if (_backendFoldout)
//             {
//                 if (GUILayout.Button("Generate SDK")) _ = OnBackendGenerateSDK();
//                 if (GUILayout.Button("Edit Config")) OnBackendEditConfig();
//             }
//             EditorGUILayout.EndFoldoutHeaderGroup();
//         }

//         private async Task OnLocalGameServerStart()
//         {
//             BuildAndRunServer();
//         }

//         private async Task OnLocalGameServerStop()
//         {
//             RivetLogger.Error("UNIMPLEMENTED");
//         }

//         private async Task OnBackendGenerateSDK() {
//             // HACK: Show term instead of running inline
//             var input = new JObject { ["cwd"] = ProjectRoot(), ["fallback_sdk_path"] = "Assets/Backend", ["target"] = "unity" };
//             await new ToolchainTask("show_term", new JObject
//             {
//                 ["command"] = ToolchainTask.GetRivetCLIPath(),
//                 ["args"] = new JArray { "task", "run", "--run-config", "{}", "--name", "backend_sdk_gen", "--input", input.ToString(Formatting.None) },
//             }).RunAsync();
//         }

//         private void OnBackendEditConfig() {
//             EditorUtility.OpenWithDefaultApp(Path.Combine(ProjectRoot(), "backend.json"));
//         }

//         // MARK: Tab: Deploy
//         private void DrawDeploy()
//         {
//             // Environment
//             var options = _bootstrapData.BackendEnvironments.Select(x => x.DisplayName).ToList();
//             options.Add("+ Create Environment");
//             var newEnvIndex = EditorGUILayout.Popup("Environment", _remoteEnvironmentIndex, options.ToArray());

//             // Open create new env
//             if (newEnvIndex == _bootstrapData.BackendEnvironments.Count)
//             {
//                 Application.OpenURL($"https://hub.rivet.gg/games/{_bootstrapData.GameId}/backend?modal=create-environment");
//             }
//             else
//             {
//                 _remoteEnvironmentIndex = newEnvIndex;
//             }

//             // Steps
//             string[] deployStepsOptions = { "Both Game Server & Backend", "Game Server", "Backend" };
//             _deploySteps = (DeploySteps)EditorGUILayout.Popup("Steps", (int)_deploySteps, deployStepsOptions);

//             if (GUILayout.Button("Build & Deploy")) _ = OnBuildAndDeploy();
//         }

//         private async Task OnBuildAndDeploy()
//         {
//             // // Check if Linux build support is installed
//             // if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64))
//             // {
//             //     RivetLogger.Error("Linux build support is not installed");
//             //     return;
//             // }

//             // // Set the build settings
//             // BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
//             // {
//             //     scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
//             //     locationPathName = "builds/LinuxServer/LinuxServer.x86_64", // Output path
//             //     target = BuildTarget.StandaloneLinux64, // Target platform
//             //     subtarget = (int)StandaloneBuildSubtarget.Server // Headless mode for server build
//             // };

//             // // Build the player
//             // var result = BuildPipeline.BuildPlayer(buildPlayerOptions);

//             // // If the build failed, log an error, and don't continue
//             // if (result.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
//             // {
//             //     RivetLogger.Error("Build failed: " + result.summary.result);
//             //     return;
//             // }

//             // // Run deploy with CLI
//             // new System.Threading.Thread(async () =>
//             // {
//             //     var result = await new ToolchainTask(
//             //         "deploy",
//             //         new JObject
//             //         {
//             //             ["cwd"] = "TODO",
//             //             ["environment_id"] = _gameData.namespaces[_selectedEnvironment].Item1.name_id,
//             //             ["game_server"] = true,
//             //             ["backend"] = true,
//             //         }
//             //     ).RunAsync();
//             // }).Start();
//         }

//         // MARK: Tab: Settings
//         private void DrawSettings()
//         {
//             GUILayout.Label("Account");
//             if (GUILayout.Button("Sign Out"))
//             {
//                 _ = OnUnlinkGame();
//             }

//             GUILayout.Label("Plugin");
//             GUILayout.BeginHorizontal();
//             GUILayout.Button("Edit User");
//             GUILayout.Button("Edit Project");
//             GUILayout.EndHorizontal();

//             GUILayout.Label("Local Backend Daemon");
//             GUILayout.Label("Runs your local backend server used for development in the background. Rarely needs to be stopped.");
//             GUILayout.BeginHorizontal();
//             if (GUILayout.Button("Start")) _ = OnBackendStart();
//             if (GUILayout.Button("Stop")) _ = OnBackendStop();
//             if (GUILayout.Button("Restart")) _ = OnBackendStart();
//             GUILayout.EndHorizontal();
//             EditorGUILayout.LinkButton("Show Logs");

//             GUILayout.Label("Source Code");
//             EditorGUILayout.LinkButton("rivet-gg/rivet");
//             EditorGUILayout.LinkButton("rivet-gg/opengb");
//             EditorGUILayout.LinkButton("rivet-gg/plugin-godot");
//         }


//         private async Task OnBackendStart()
//         {
//             // HACK: Show term instead of running inline

//             var input = new JObject { ["port"] = 6420, ["cwd"] = ProjectRoot() };
//             await new ToolchainTask("show_term", new JObject
//             {
//                 ["command"] = ToolchainTask.GetRivetCLIPath(),
//                 ["args"] = new JArray { "task", "run", "--run-config", "{}", "--name", "backend_dev", "--input", input.ToString(Formatting.None) },
//             }).RunAsync();
//         }


//         private async Task OnBackendStop()
//         {
//             RivetLogger.Error("UNIMPLEMENTED");
//         }

//         private async Task OnUnlinkGame()
//         {
//             await new ToolchainTask("unlink", new JObject()).RunAsync();
//             _pluginWindow.TransitionToState(new Screens.Login.Window());
//         }

//         private string GetUnityEditorPath()
//         {
//             string editorPath = EditorApplication.applicationPath;

//             if (Application.platform == RuntimePlatform.OSXEditor)
//             {
//                 // On macOS point to the actual Unity binary inside the .app
//                 editorPath = Path.Combine(editorPath, "Contents/MacOS/Unity");
//             }

//             return editorPath;
//         }

//         // MARK: Run Game Server
//         private BuildTarget GetGameServerBuildTarget()
//         {
//             // TODO:
//             return BuildTarget.StandaloneOSX;
//         }

//         private void BuildAndRunServer()
//         {
//             // Ensure a scene is included
//             if (EditorBuildSettings.scenes.Length == 0)
//             {
//                 RivetLogger.Error("No scenes in build settings. Please add at least one scene.");
//                 return;
//             }

//             // Configure build settings
//             var buildPlayerOptions = new BuildPlayerOptions
//             {
//                 scenes = GetScenePaths(),
//                 locationPathName = Path.Combine(ProjectRoot(), "Builds", "DedicatedServer", GetBuildName()),
//                 target = GetGameServerBuildTarget(),
//                 subtarget = (int)StandaloneBuildSubtarget.Server
//             };

//             // // Speed up builds for development
//             // var devOptimizations = true;
//             // if (devOptimizations)
//             // {
//             //     buildPlayerOptions.options |= BuildOptions.Development;
//             //     buildPlayerOptions.options |= BuildOptions.CompressWithLz4;
//             //     EditorUserBuildSettings.compressFilesInPackage = false;

//             //     // TODO: Is this right?
//             //     buildPlayerOptions.options &= ~BuildOptions.StrictMode;

//             //     // buildPlayerOptions.options |= BuildOptions.BuildScriptsOnly;

//             //     // TODO: Is this right?
//             //     // Disable error checking for faster builds
//             //     buildPlayerOptions.extraScriptingDefines = new string[] { "DISABLE_IMPLICIT_CHECKS", "DISABLE_WARNINGS" };
//             // }


//             // Build the server
//             RivetLogger.Log("Building dedicated server...");
//             var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
//             var result = report.summary.result;

//             if (result == BuildResult.Succeeded)
//             {
//                 RivetLogger.Log("Dedicated server build completed successfully.");
//                 LaunchServer(buildPlayerOptions.locationPathName);
//             }
//             else
//             {
//                 RivetLogger.Error("Dedicated server build failed.");
//             }
//         }

//         private async void LaunchServer(string buildPath)
//         {
//             // Add path to binary for macOS
//             string executablePath = FindExecutablePath(buildPath);

//             // HACK: Show term instead of running inline
//             // TODO: Better logs dir
//             // string logPath = Path.Combine(Path.GetDirectoryName(executablePath), "server_log.txt");
//             await new ToolchainTask("show_term", new JObject
//             {
//                 ["command"] = executablePath,
//                 // ["args"] = new JArray { "-batchmode", "-nographics", "-logFile", logPath, "-server" },
//                 ["args"] = new JArray { "-batchmode", "-nographics", "-logFile", "-server" },
//             }).RunAsync();

//             // OLD:
//             // string logPath = Path.Combine(Path.GetDirectoryName(serverPath), "server_log.txt");

//             // ProcessStartInfo startInfo = new ProcessStartInfo
//             // {
//             //     FileName = serverPath,
//             //     Arguments = $"-batchmode -nographics -logFile \"{logPath}\"",
//             //     UseShellExecute = false,
//             //     CreateNoWindow = true
//             // };

//             // RivetLogger.Log($"Launching dedicated server with command: {startInfo.FileName} {startInfo.Arguments}");

//             // try
//             // {
//             //     using (Process serverProcess = Process.Start(startInfo))
//             //     {
//             //         RivetLogger.Log("Dedicated server launched successfully.");
//             //     }
//             // }
//             // catch (System.Exception e)
//             // {
//             //     RivetLogger.Error($"Failed to launch dedicated server: {e.Message}");
//             // }
//         }

//         private string FindExecutablePath(string serverPath)
//         {
//             string productName = Application.productName;
//             string executableFile = Path.Combine(serverPath, productName);

//             if (!File.Exists(executableFile))
//             {
//                 throw new Exception($"Executable {productName} not found in {serverPath}");
//             }

//             return executableFile;
//         }

//         private string[] GetScenePaths()
//         {
//             string[] scenes = new string[EditorBuildSettings.scenes.Length];
//             for (int i = 0; i < scenes.Length; i++)
//             {
//                 scenes[i] = EditorBuildSettings.scenes[i].path;
//             }
//             return scenes;
//         }

//         private string GetBuildName()
//         {
//             // TODO: Check that we don't need .exe for Windows
//             var serverName = "DedicatedServer";
//             return serverName;
//             // switch (GetGameServerBuildTarget())
//             // {
//             //     case BuildTarget.StandaloneWindows:
//             //     case BuildTarget.StandaloneWindows64:
//             //         return $"{serverName}.exe";
//             //     case BuildTarget.StandaloneOSX:
//             //         return $"{serverName}.app";
//             //     case BuildTarget.StandaloneLinux64:
//             //         return $"{serverName}";
//             //     default:
//             //         return serverName;
//             // }
//         }

//         // MARK: Utils
//         // TODO: move to utils file
//         private string ProjectRoot()
//         {
//             var dataPath = Application.dataPath;
//             return Directory.GetParent(dataPath).FullName;
//         }
//     }
// }