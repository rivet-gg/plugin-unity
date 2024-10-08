using System;
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
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.Editor.UI.Dock.Tabs
{
    public delegate void EnvironmentChangeDelegate(EnvironmentType type, string? remoteEnvironmentId);

    public class DevelopController
    {
        private readonly Dock _dock;
        private readonly VisualElement _root;

        private VisualElement _refreshButton;
        private DropdownField _environmentTypeDropdown;
        private DropdownField _remoteEnvironmentDropdown;

        private DropdownField _playTypeDropdown;
        private DropdownField _playStepsDropdown;
        private SliderInt _playerCount;
        private Button _lgsStart;
        private Button _lgsStop;
        private Button _lgsRestart;
        private VisualElement _lgsShowLogs;


        private Button _buildDeployButton;
        private DropdownField _stepsDropdown;

        public DevelopController(Dock dock, VisualElement root)
        {
            _dock = dock;
            _root = root;

            InitUI();
        }

        void InitUI()
        {
            var plugin = RivetGlobal.Singleton;

            // Query
            _refreshButton = _root.Q("EnvironmentHeader").Q("Header").Q("Action");
            _environmentTypeDropdown = _root.Q("EnvironmentBody").Q<DropdownField>("TypeDropdown");
            _remoteEnvironmentDropdown = _root.Q("EnvironmentBody").Q<DropdownField>("EnvironmentDropdown");

            _playTypeDropdown = _root.Q("PlayBody").Q<DropdownField>("TypeDropdown");
            _playStepsDropdown = _root.Q("PlayBody").Q<DropdownField>("StepsDropdown");
            _playerCount = _root.Q("PlayBody").Q<SliderInt>("PlayerCountSlider");

            _lgsStart = _root.Q("PlayBody").Q("ButtonRow").Q("StartButton").Q<Button>("Button");
            _lgsStop = _root.Q("PlayBody").Q("ButtonRow").Q("StopButton").Q<Button>("Button");
            _lgsRestart = _root.Q("PlayBody").Q("ButtonRow").Q("RestartButton").Q<Button>("Button");
            _lgsShowLogs = _root.Q("PlayBody").Q("ServerLogsButton");

            _buildDeployButton = _root.Q("BuildDeployButton").Q<Button>("Button");
            _stepsDropdown = _root.Q<DropdownField>("BuildStepsDropdown");

            // Callbacks
            _dock.LocalGameServerManager.StateChange += OnLocalGameServerStateChange;
            OnLocalGameServerStateChange(_dock.LocalGameServerManager.IsRunning);

            _refreshButton.RegisterCallback<ClickEvent>(ev => { _ = plugin.Bootstrap(); });

            _environmentTypeDropdown.RegisterValueChangedCallback(ev =>
            {
                if (!plugin.IsAuthenticated && _environmentTypeDropdown.index == 1)
                {
                    // Force sign in
                    _environmentTypeDropdown.index = 0;
                    _ = Dock.Singleton?.StartSignIn();
                }
                else
                {
                    // Change environment
                    plugin.EnvironmentType = (EnvironmentType)_environmentTypeDropdown.index;
                    _dock.OnSelectedEnvironmentChange();
                }
            });
            _remoteEnvironmentDropdown.RegisterValueChangedCallback(ev =>
            {
                if (plugin.CloudData is { } cloudData && _remoteEnvironmentDropdown.childCount > 0 && _remoteEnvironmentDropdown.index == _remoteEnvironmentDropdown.choices.Count - 1)
                {
                    // Open URL and reset index
                    Application.OpenURL($"https://hub.rivet.gg/games/{cloudData.GameId}/backend?modal=create-environment");
                    _remoteEnvironmentDropdown.index = plugin.RemoteEnvironmentIndex ?? -1;
                }
                else
                {
                    // Update index
                    plugin.RemoteEnvironmentIndex = _remoteEnvironmentDropdown.index;
                    _dock.OnSelectedEnvironmentChange();
                }
            });

            _lgsStart.RegisterCallback<ClickEvent>(ev => { OnLocalGameServerStart(); });
            _lgsStop.RegisterCallback<ClickEvent>(ev => { _ = _dock.LocalGameServerManager.StopTask(); });
            _lgsRestart.RegisterCallback<ClickEvent>(ev => { OnLocalGameServerStart(); });
            _lgsShowLogs.RegisterCallback<ClickEvent>(ev => GameServerWindow.ShowGameServer());

            _playTypeDropdown.RegisterValueChangedCallback(ev => UpdatePlayerCountVisibility());

            _buildDeployButton.RegisterCallback<ClickEvent>(ev => OnBuildAndDeploy());

            // Initial visibility update
            UpdatePlayerCountVisibility();
        }

        public void OnBootstrap()
        {
            var plugin = RivetGlobal.Singleton;

            // Toggle remote environment
            _environmentTypeDropdown.choices[1] = plugin.IsAuthenticated ? "Remote" : "Remote (Requires Sign In)";

            // Add environment list
            if (plugin.CloudData is { } cloudData)
            {
                // Add environments
                List<string> environments = new();
                foreach (var env in cloudData.Environments)
                {
                    string buildName = env.Name;
                    if (cloudData.CurrentBuilds.TryGetValue(env.Id, out var build) && build.Tags.TryGetValue("version", out var versionTag))
                    {
                        buildName += $" ({versionTag})";
                    }
                    environments.Add(buildName);
                }
                environments.Add("+ New Environment");
                _remoteEnvironmentDropdown.choices = environments;
            }
            OnSelectedEnvironmentChange();

            // Disable sign in button
            _buildDeployButton.SetEnabled(plugin.IsAuthenticated);
            _buildDeployButton.Q<Label>(name: "Label").text = plugin.IsAuthenticated ? "Build & Deploy" : "Build & Deploy (Requires Sign In)";
        }

        /// <summary>
        /// Called when selected environment changes.
        /// </summary>
        public void OnSelectedEnvironmentChange()
        {
            var plugin = RivetGlobal.Singleton;
            _environmentTypeDropdown.index = (int)plugin.EnvironmentType;
            _remoteEnvironmentDropdown.index = plugin.RemoteEnvironmentIndex ?? -1;
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
            var canPlayClient = _playTypeDropdown.index == 0 || _playTypeDropdown.index == 1;
            var canPlayServer = _playTypeDropdown.index == 0 || _playTypeDropdown.index == 2;
            var shouldBuild = _playStepsDropdown.index == 0;

            // Server
            if (canPlayServer)
            {
                if (shouldBuild)
                {
                    try
                    {
                        Builder.BuildDevDedicatedServer();
                    }
                    catch (Exception e)
                    {
                        EditorUtility.DisplayDialog("Server Build Failed", e.Message, "Dismiss");
                        return;
                    }
                }
                else
                {
                    string serverPath = Builder.GetDevDedicatedServerExecutablePath();
                    if (!File.Exists(serverPath))
                    {
                        EditorUtility.DisplayDialog("Missing Server Build", $"The server needs to be built before running without the build step.\n\nExpected path: {serverPath}", "OK");
                        return;
                    }
                }

                // Start server
                _ = _dock.LocalGameServerManager.StartTask();
            }

            // Client
            if (canPlayClient)
            {
                if (shouldBuild)
                {
                    try
                    {
                        Builder.BuildDevPlayer();
                    }
                    catch (Exception e)
                    {
                        EditorUtility.DisplayDialog("Player Build Failed", e.Message, "Dismiss");
                        return;
                    }
                }
                else
                {
                    string playerPath = Builder.GetDevPlayerExecutablePath();
                    if (!File.Exists(playerPath))
                    {
                        EditorUtility.DisplayDialog("Missing Player Build", $"The player needs to be built before running without the build step.\n\nExpected path: {playerPath}", "OK");
                        return;
                    }
                }

                // Start players
                Builder.RunMultipleDevPlayers(_playerCount.value);
            }

            // Open game server logs if needed
            if (canPlayServer)
            {
                if (!EditorWindow.HasOpenInstances<GameServerWindow>())
                {
                    GameServerWindow.ShowGameServer();
                }
            }
        }

        private void OnBuildAndDeploy()
        {
            var plugin = RivetGlobal.Singleton;

            // Force to remote env to update testing to correct env
            plugin.EnvironmentType = EnvironmentType.Remote;
            _dock.OnSelectedEnvironmentChange();

            // Get the selected environment ID
            string environmentId = plugin.RemoteEnvironmentId;
            if (environmentId == null)
            {
                throw new System.Exception("Could not get ID for remote env");
            }

            // Get the selected steps
            int stepsIndex = _stepsDropdown.index;
            bool deployGameServer = stepsIndex == 0 || stepsIndex == 1;
            bool deployModules = stepsIndex == 0 || stepsIndex == 2;

            string? serverPath = null;
            if (deployGameServer)
            {
                try
                {
                    serverPath = Builder.BuildReleaseDedicatedServer();
                }
                catch (Exception e)
                {
                    EditorUtility.DisplayDialog("Server Build Failed", e.Message, "Dismiss");
                }
            }

            // Run deploy with CLI using TaskPopupWindow
            TaskPopupWindow.RunTask("Build & Deploy", "deploy", new JObject
            {
                ["cwd"] = Builder.ProjectRoot(),
                ["environment_id"] = environmentId,
                ["game_server"] = deployGameServer,
                ["backend"] = deployModules,
            });
        }

        private void UpdatePlayerCountVisibility()
        {
            bool showPlayerCount = _playTypeDropdown.index == 0 || _playTypeDropdown.index == 1;
            _playerCount.style.display = showPlayerCount ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}