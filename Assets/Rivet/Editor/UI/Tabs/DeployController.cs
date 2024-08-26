using System;
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
using Rivet.Editor.UI.TaskPopup;

namespace Rivet.UI.Tabs
{
    public class DeployController
    {
        private readonly RivetPlugin _pluginWindow;
        private MainController _mainController;
        private readonly VisualElement _root;

        private VisualElement _refreshButton;
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
            _refreshButton = _root.Q("DeployHeader").Q("Header").Q("Action");
            _environmentDropdown = _root.Q<DropdownField>("EnvironmentDropdown");
            _buildDeployButton = _root.Q("BuildDeployButton").Q<Button>("Button");
            _stepsDropdown = _root.Q<DropdownField>("StepsDropdown");

            // Callbacks
            _refreshButton.RegisterCallback<ClickEvent>(ev => { _ = _mainController.GetBootstrapData(); });

            _environmentDropdown.RegisterValueChangedCallback(ev =>
            {
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
            _environmentDropdown.index = _mainController.RemoteEnvironmentIndex ?? -1;
        }

        private void OnBuildAndDeploy()
        {
            // Force to remote env to update testing to correct env
            _mainController.EnvironmentType = EnvironmentType.Remote;
            _mainController.OnSelectedEnvironmentChange();

            // Get the selected environment ID
            string environmentId = _mainController.RemoteEnvironmentId;
            if (environmentId == null)
            {
                throw new System.Exception("Could not get ID for remote env");
            }

            // Get the selected steps
            int stepsIndex = _stepsDropdown.index;
            bool deployGameServer = stepsIndex == 0 || stepsIndex == 1;
            bool deployBackend = stepsIndex == 0 || stepsIndex == 2;

            string? serverPath = null;
            if (deployGameServer)
            {
                try {
                    serverPath = Builder.BuildReleaseDedicatedServer();
                } catch (Exception e) {
                    EditorUtility.DisplayDialog("Server Build Failed", e.Message, "Dismiss");
                }
            }

            // Run deploy with CLI using TaskPopupWindow
            var task = TaskPopupWindow.RunTask("Build & Deploy", "deploy", new JObject
            {
                ["cwd"] = Builder.ProjectRoot(),
                ["environment_id"] = environmentId,
                ["game_server"] = deployGameServer,
                ["backend"] = deployBackend,
            });

            // Save version
            task.OnTaskOutput += output =>
            {
                if (output is ResultOk<JObject> ok)
                {
                    var gameServerObj = ok.Data["game_server"];
                    if (gameServerObj != null && gameServerObj.Type == JTokenType.Object)
                    {
                        var version = gameServerObj["version_name"]?.ToString();
                        RivetPlugin.Singleton.GameVersion = version;
                        SharedSettings.UpdateFromPlugin();
                        Debug.Log($"New game version: {RivetPlugin.Singleton.GameVersion}");
                    }
                }
            };
        }
    }
}