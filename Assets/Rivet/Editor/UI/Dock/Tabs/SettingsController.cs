using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.UI;
using Rivet.Editor.UI.TaskPanel;
using Rivet.Editor.Util;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.Editor.UI.Dock.Tabs
{
    public class SettingsController
    {
        private readonly Dock _dock;
        private readonly VisualElement _root;

        private Button _backendStart;
        private Button _backendStop;
        private Button _backendRestart;
        private VisualElement _backendShowLogs;

        private Button _editUserButton;
        private Button _editProjectButton;

        // Add enum definition
        private enum SettingsType
        {
            User,
            Project
        }

        public SettingsController(Dock dock, VisualElement root)
        {
            _dock = dock;
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

            // Query new buttons
            _editUserButton = _root.Q("PluginBody").Q("ButtonRow").Q<Button>("EditUserButton");
            _editProjectButton = _root.Q("PluginBody").Q("ButtonRow").Q<Button>("EditProjectButton");

            // Callbacks
            _dock.BackendManager.StateChange += OnBackendStateChange;
            OnBackendStateChange(false);

            _root.Q("AccountBody").Q("SignOutButton").RegisterCallback<ClickEvent>(ev => { _ = OnUnlinkGame(); });

            _backendStart.RegisterCallback<ClickEvent>(ev => { _ = _dock.BackendManager.StartTask(); });
            _backendStop.RegisterCallback<ClickEvent>(ev => _dock.BackendManager.StopTask());
            _backendRestart.RegisterCallback<ClickEvent>(ev => { _ = _dock.BackendManager.StartTask(); });
            _backendShowLogs.RegisterCallback<ClickEvent>(ev => BackendWindow.ShowBackend());

            // Update callbacks
            _editUserButton.RegisterCallback<ClickEvent>(ev => { _ = OnEditSettings(SettingsType.User); });
            _editProjectButton.RegisterCallback<ClickEvent>(ev => { _ = OnEditSettings(SettingsType.Project); });
        }

        private async Task OnUnlinkGame()
        {
            await new RivetTask("unlink", new JObject()).RunAsync();
            // _dock.SetScreen(Editor.UI.Screen.Login);
        }

        private void OnBackendStateChange(bool running)
        {
            _backendStart.style.display = running ? DisplayStyle.None : DisplayStyle.Flex;
            _backendStop.style.display = running ? DisplayStyle.Flex : DisplayStyle.None;
            _backendRestart.style.display = running ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private async Task OnEditSettings(SettingsType type)
        {
            var pathsResult = await new RivetTask("get_settings_path", new JObject()).RunAsync();
            
            string path;
            switch (pathsResult)
            {
                case ResultOk<JObject> ok:
                    path = type == SettingsType.Project ? (string)ok.Data["project_path"] : (string)ok.Data["user_path"];
                    break;
                case ResultErr<JObject> err:
                    RivetLogger.Error($"Failed to get settings paths: {err}");
                    return;
                default:
                    return;
            }

            await new RivetTask("open", new JObject { ["path"] = path }).RunAsync();
        }
    }
}