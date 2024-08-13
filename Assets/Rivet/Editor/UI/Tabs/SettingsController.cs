using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.UI;
using Rivet.Editor.UI.TaskPanel;
using Rivet.Editor.Util;
using Rivet.UI.Screens;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.UI.Tabs
{
    public class SettingsController
    {
        private readonly RivetPlugin _pluginWindow;
        private readonly MainController _mainController;
        private readonly VisualElement _root;

        private Button _backendStart;
        private Button _backendStop;
        private Button _backendRestart;
        private VisualElement _backendShowLogs;

        public SettingsController(RivetPlugin window, MainController mainController, VisualElement root)
        {
            _pluginWindow = window;
            _mainController = mainController;
            _root = root;

            InitUI();
        }

        void InitUI()
        {
            // Query
            _backendStart = _root.Q("BackendBody").Q("ButtonRow").Q<Button>("StartButton");
            _backendStop = _root.Q("BackendBody").Q("ButtonRow").Q<Button>("StopButton");
            _backendRestart = _root.Q("BackendBody").Q("ButtonRow").Q<Button>("RestartButton");
            _backendShowLogs = _root.Q("BackendBody").Q("LogsButton");

            // Callbacks
            _mainController.BackendManager.StateChange += OnBackendStateChange;
            OnBackendStateChange(false);

            _root.Q("AccountBody").Q("SignOutButton").RegisterCallback<ClickEvent>(ev => { _ = OnUnlinkGame(); });

            _backendStart.RegisterCallback<ClickEvent>(ev => { _ = _mainController.BackendManager.StartTask(); });
            _backendStop.RegisterCallback<ClickEvent>(ev => _mainController.BackendManager.StopTask());
            _backendRestart.RegisterCallback<ClickEvent>(ev => { _ = _mainController.BackendManager.StartTask(); });
            _backendShowLogs.RegisterCallback<ClickEvent>(ev => BackendWindow.ShowBackend());
        }

        private async Task OnUnlinkGame()
        {
            await new RivetTask("unlink", new JObject()).RunAsync();
            _pluginWindow.SetScreen(Editor.UI.Screen.Login);
        }

        private void OnBackendStateChange(bool running)
        {
            _backendStart.style.display = running ? DisplayStyle.None : DisplayStyle.Flex;
            _backendStop.style.display = running ? DisplayStyle.Flex : DisplayStyle.None;
            _backendRestart.style.display = running ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}