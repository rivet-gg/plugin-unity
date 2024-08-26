using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.Types;
using Rivet.Editor.UI;
using Rivet.UI.Screens;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.UI.Tabs
{
    public class DeployController
    {
        private readonly RivetPlugin _pluginWindow;
        private MainController _mainController;
        private readonly VisualElement _root;

        private DropdownField _environmentDropdown;

        public DeployController(RivetPlugin window, MainController mainController, VisualElement root)
        {
            _pluginWindow = window;
            _mainController = mainController;
            _root = root;

            InitUI();
        }

        void InitUI()
        {
            // Query
            _environmentDropdown = _root.Q<DropdownField>("EnvironmentDropdown");

            // Callbacks
            _environmentDropdown.RegisterValueChangedCallback(ev =>
            {
                _mainController.EnvironmentType = EnvironmentType.Remote;
                _mainController.RemoteEnvironmentIndex = _environmentDropdown.index;
                _mainController.OnSelectedEnvironmentChange();
            });
        }

        public void OnBootstrap(BootstrapData data)
        {
            // Add environments
            List<string> environments = new();
            foreach (var env in data.Environments)
            {
                environments.Add(env.Name);
            }
            environments.Add("+ New Environment");
            _environmentDropdown.choices = environments;

            if (environments.Count > 0)
            {
                _environmentDropdown.index = 0;
                _mainController.RemoteEnvironmentIndex = 0;
            }

            OnSelectedEnvironmentChange();
        }

        public void OnSelectedEnvironmentChange()
        {
            _environmentDropdown.index = _mainController.RemoteEnvironmentIndex;
        }

        private async Task OnBuildAndDeploy()
        {
            // Force to remote env to update testing to correct env
            _mainController.EnvironmentType = EnvironmentType.Remote;
            _mainController.OnSelectedEnvironmentChange();

            // // Check if Linux build support is installed
            // if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64))
            // {
            //     RivetLogger.Error("Linux build support is not installed");
            //     return;
            // }

            // // Set the build settings
            // BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            // {
            //     scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
            //     locationPathName = "builds/LinuxServer/LinuxServer.x86_64", // Output path
            //     target = BuildTarget.StandaloneLinux64, // Target platform
            //     subtarget = (int)StandaloneBuildSubtarget.Server // Headless mode for server build
            // };

            // // Build the player
            // var result = BuildPipeline.BuildPlayer(buildPlayerOptions);

            // // If the build failed, log an error, and don't continue
            // if (result.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            // {
            //     RivetLogger.Error("Build failed: " + result.summary.result);
            //     return;
            // }

            // // Run deploy with CLI
            // new System.Threading.Thread(async () =>
            // {
            //     var result = await new ToolchainTask(
            //         "deploy",
            //         new JObject
            //         {
            //             ["cwd"] = "TODO",
            //             ["environment_id"] = _gameData.namespaces[_selectedEnvironment].Item1.name_id,
            //             ["game_server"] = true,
            //             ["backend"] = true,
            //         }
            //     ).RunAsync();
            // }).Start();
        }

    }
}