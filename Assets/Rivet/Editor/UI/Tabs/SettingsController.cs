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

        private Button _editUserButton;
        private Button _editProjectButton;

        // Add enum definition
        private enum SettingsType
        {
            User,
            Project
        }

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

            // Query new buttons
            _editUserButton = _root.Q("PluginBody").Q("ButtonRow").Q<Button>("EditUserButton");
            _editProjectButton = _root.Q("PluginBody").Q("ButtonRow").Q<Button>("EditProjectButton");

            // Callbacks
            _pluginWindow.BackendManager.StateChange += OnBackendStateChange;
            OnBackendStateChange(false);

            _root.Q("AccountBody").Q("SignOutButton").RegisterCallback<ClickEvent>(ev => { _ = OnUnlinkGame(); });

            // _backendStart.RegisterCallback<ClickEvent>(ev => { _ = _pluginWindow.BackendManager.StartTask(); });
            // _backendStop.RegisterCallback<ClickEvent>(ev => _pluginWindow.BackendManager.StopTask());
            // _backendRestart.RegisterCallback<ClickEvent>(ev => { _ = _pluginWindow.BackendManager.StartTask(); });
            _backendStart.RegisterCallback<ClickEvent>(ev => { _ = OnStartBackend(); });  // TODO: Remove
            _backendShowLogs.RegisterCallback<ClickEvent>(ev => BackendWindow.ShowBackend());

            // Update callbacks
            _editUserButton.RegisterCallback<ClickEvent>(ev => { _ = OnEditSettings(SettingsType.User); });
            _editProjectButton.RegisterCallback<ClickEvent>(ev => { _ = OnEditSettings(SettingsType.Project); });
        }

        private async Task OnStartBackend()
        {
            // Choose port to run on. This is to avoid potential conflicts with
            // multiple projects running at the same time.
            var chooseRes = await new RivetTask("backend_choose_local_port", new JObject()).RunAsync();
            int port;
            switch (chooseRes)
            {
                case ResultOk<JObject> ok:
                    port = (int)ok.Data["port"];
                    RivetPlugin.Singleton.LocalBackendPort = port;
                    break;
                case ResultErr<JObject> err:
                    RivetLogger.Error($"Failed to choose port: {err}");
                    return;
                default:
                    return;
            }

            // var input = new JObject {
            //     ["port"] = port,
            //     ["cwd"] = Builder.ProjectRoot()
            // };
            // await new RivetTask("show_term", new JObject
            // {
            //     ["command"] = RivetTask.GetRivetCLIPath(),
            //     ["args"] = new JArray { "task", "run", "--run-config", "{}", "--name", "backend_dev", "--input", input.ToString(Formatting.None) },
            // }).RunAsync();
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