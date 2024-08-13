using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            _lgsStop.RegisterCallback<ClickEvent>(ev => { OnLocalGameServerStop(); });
            _lgsRestart.RegisterCallback<ClickEvent>(ev => { OnLocalGameServerStart(); });
            _lgsShowLogs.RegisterCallback<ClickEvent>(ev => { Debug.Log("TODO"); });

            _backendGenerateSdk.RegisterCallback<ClickEvent>(ev => { _ = OnBackendGenerateSDK(); });
            _backendEditConfig.RegisterCallback<ClickEvent>(ev => { OnBackendEditConfig(); });
        }

        public void OnBootstrap(BootstrapData data)
        {
            // Add environments
            List<string> environments = new();
            foreach (var env in data.BackendEnvironments)
            {
                environments.Add(env.DisplayName);
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

        private void OnLocalGameServerStart()
        {
            Builder.BuildAndRunServer();
        }

        private void OnLocalGameServerStop()
        {
            RivetLogger.Error("UNIMPLEMENTED");
        }

        private async Task OnBackendGenerateSDK()
        {
            // HACK: Show term instead of running inline
            var input = new JObject { ["cwd"] = Builder.ProjectRoot(), ["fallback_sdk_path"] = "Assets/Backend", ["target"] = "unity" };
            await new ToolchainTask("show_term", new JObject
            {
                ["command"] = ToolchainTask.GetRivetCLIPath(),
                ["args"] = new JArray { "task", "run", "--run-config", "{}", "--name", "backend_sdk_gen", "--input", input.ToString(Formatting.None) },
            }).RunAsync();
        }

        private void OnBackendEditConfig()
        {
            EditorUtility.OpenWithDefaultApp(Path.Combine(Builder.ProjectRoot(), "backend.json"));
        }
    }
}