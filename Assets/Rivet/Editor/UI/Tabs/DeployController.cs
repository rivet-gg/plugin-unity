using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.Types;
using Rivet.Editor.UI;
using Rivet.Editor.Util;
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
        private Button _buildDeployButton;
        private DropdownField _stepsDropdown;

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
            _buildDeployButton = _root.Q("BuildDeployButton").Q<Button>("Button");
            _stepsDropdown = _root.Q<DropdownField>("StepsDropdown");

            // Callbacks
            _environmentDropdown.RegisterValueChangedCallback(ev =>
            {
                _mainController.EnvironmentType = EnvironmentType.Remote;
                _mainController.RemoteEnvironmentIndex = _environmentDropdown.index;
                _mainController.OnSelectedEnvironmentChange();
            });

            _buildDeployButton.RegisterCallback<ClickEvent>(ev => OnBuildAndDeploy());
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

        private void OnBuildAndDeploy()
        {
            // Force to remote env to update testing to correct env
            _mainController.EnvironmentType = EnvironmentType.Remote;
            _mainController.OnSelectedEnvironmentChange();

            string? serverPath = Builder.BuildReleaseDedicatedServer();
            if (serverPath == null)
            {
                EditorUtility.DisplayDialog("Server Build Failed", "See Unity console for details.", "Dismiss");
                return;
            }

            // Get the selected environment ID
            string environmentId = _mainController.RemoteEnvironment?.NameId ?? "";

            // Get the selected steps
            int stepsIndex = _stepsDropdown.index;
            bool deployGameServer = stepsIndex == 0 || stepsIndex == 1;
            bool deployBackend = stepsIndex == 0 || stepsIndex == 2;

            // Run deploy with CLI
            _ = new RivetTask(
                "deploy",
                new JObject
                {
                    ["cwd"] = Path.GetDirectoryName(serverPath),
                    ["environment_id"] = environmentId,
                    ["game_server"] = deployGameServer,
                    ["backend"] = deployBackend,
                }
            ).RunAsync();
        }

    }
}