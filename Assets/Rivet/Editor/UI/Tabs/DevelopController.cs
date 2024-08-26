using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.Types;
using Rivet.Editor.UI;
using Rivet.Editor.UI.TaskPanel;
using Rivet.Editor.UI.TaskPopup;
using Rivet.Editor.Util;
using Rivet.UI.Screens;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.UI.Tabs
{
    public delegate void EnvironmentChangeDelegate(EnvironmentType type, string? remoteEnvironmentId);

    public class DevelopController
    {
        private readonly RivetPlugin _pluginWindow;
        private readonly MainController _mainController;
        private readonly VisualElement _root;

        private DropdownField _environmentTypeDropdown;
        private DropdownField _remoteEnvironmentDropdown;

        private Button _lgsStart;
        private Button _lgsStop;
        private Button _lgsRestart;
        private VisualElement _lgsShowLogs;

        private VisualElement _backendGenerateSdk;
        private VisualElement _backendEditConfig;

        public DevelopController(RivetPlugin window, MainController mainController, VisualElement root)
        {
            _pluginWindow = window;
            _mainController = mainController;
            _root = root;

            InitUI();
        }

        void InitUI()
        {
            // Query
            _environmentTypeDropdown = _root.Q("EnvironmentBody").Q<DropdownField>("TypeDropdown");
            _remoteEnvironmentDropdown = _root.Q("EnvironmentBody").Q<DropdownField>("EnvironmentDropdown");

            _lgsStart = _root.Q("LocalGameServerBody").Q("ButtonRow").Q("StartButton").Q<Button>("Button");
            _lgsStop = _root.Q("LocalGameServerBody").Q("ButtonRow").Q("StopButton").Q<Button>("Button");
            _lgsRestart = _root.Q("LocalGameServerBody").Q("ButtonRow").Q("RestartButton").Q<Button>("Button");
            _lgsShowLogs = _root.Q("LocalGameServerBody").Q("LogsButton");

            _backendGenerateSdk = _root.Q("BackendBody").Q("GenerateSdkButton").Q<Button>("Button");
            _backendEditConfig = _root.Q("BackendBody").Q("EditConfigButton").Q<Button>("Button");

            // Callbacks
            _pluginWindow.LocalGameServerManager.StateChange += OnLocalGameServerStateChange;
            OnLocalGameServerStateChange(false);

            _environmentTypeDropdown.RegisterValueChangedCallback(ev =>
            {
                _mainController.EnvironmentType = (EnvironmentType)_environmentTypeDropdown.index;
                _mainController.OnSelectedEnvironmentChange();
            });
            _remoteEnvironmentDropdown.RegisterValueChangedCallback(ev =>
            {
                if (_mainController.BootstrapData is { } bootstrapData && _remoteEnvironmentDropdown.childCount > 0 && _remoteEnvironmentDropdown.index == _remoteEnvironmentDropdown.choices.Count - 1)
                {
                    // Open URL and reset index
                    Application.OpenURL($"https://hub.rivet.gg/games/{bootstrapData.GameId}/backend?modal=create-environment");
                    _remoteEnvironmentDropdown.index = _mainController.RemoteEnvironmentIndex;
                }
                else
                {
                    // Update index
                    _mainController.RemoteEnvironmentIndex = _remoteEnvironmentDropdown.index;
                    _mainController.OnSelectedEnvironmentChange();
                }
            });


            _lgsStart.RegisterCallback<ClickEvent>(ev => { OnLocalGameServerStart(); });
            _lgsStop.RegisterCallback<ClickEvent>(ev => _pluginWindow.LocalGameServerManager.StopTask());
            _lgsRestart.RegisterCallback<ClickEvent>(ev => { OnLocalGameServerStart(); });
            _lgsShowLogs.RegisterCallback<ClickEvent>(ev => GameServerWindow.ShowGameServer());

            _backendGenerateSdk.RegisterCallback<ClickEvent>(ev => OnBackendGenerateSDK());
            _backendEditConfig.RegisterCallback<ClickEvent>(ev => OnBackendEditConfig());
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
            _remoteEnvironmentDropdown.choices = environments;

            OnSelectedEnvironmentChange();
        }

        /// <summary>
        /// Called when selected environment changes.
        /// </summary>
        public void OnSelectedEnvironmentChange()
        {
            _environmentTypeDropdown.index = (int)_mainController.EnvironmentType;
            _remoteEnvironmentDropdown.index = _mainController.RemoteEnvironmentIndex;
            _remoteEnvironmentDropdown.style.display = _environmentTypeDropdown.index == (int)EnvironmentType.Local ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void OnLocalGameServerStateChange(bool running)
        {
            // Use parent since this is a template holding a button
            _lgsStart.parent.style.display = running ? DisplayStyle.None : DisplayStyle.Flex;
            _lgsStop.parent.style.display = running ? DisplayStyle.Flex : DisplayStyle.None;
            _lgsRestart.parent.style.display = running ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnLocalGameServerStart()
        {
            _pluginWindow.LocalGameServerExecutablePath = Builder.BuildDevServer();
            _ = _pluginWindow.LocalGameServerManager.StartTask();
        }

        private void OnBackendGenerateSDK()
        {
            TaskPopupWindow.RunTask("Generate SDK", "backend_sdk_gen", new JObject
            {
                ["cwd"] = Builder.ProjectRoot(),
                ["fallback_sdk_path"] = "Assets/Backend",
                ["target"] = "unity",
            });
        }

        private void OnBackendEditConfig()
        {
            EditorUtility.OpenWithDefaultApp(Path.Combine(Builder.ProjectRoot(), "backend.json"));
        }
    }
}