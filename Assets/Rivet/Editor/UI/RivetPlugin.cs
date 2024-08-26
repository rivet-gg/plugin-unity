using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Rivet.UI.Screens;
using Rivet.Editor.UI.TaskPanel;
using Rivet.Editor.Util;

namespace Rivet.Editor.UI
{
    public enum Screen
    {
        Login,
        Main,
    }

    public class RivetPlugin : EditorWindow
    {
        public static RivetPlugin? Singleton;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset;

        private Screen _screen = Screen.Login;

        public string ApiEndpoint = "https://api.rivet.gg";

        // MARK: Local Game Server
        public string? LocalGameServerExecutablePath;

        // MARK: Backend
        private int _localBackendPort = 6420;
        public int LocalBackendPort {
            get { return _localBackendPort; }
            set {
                _localBackendPort = value;
                SharedSettings.UpdateFromPlugin();
            }
        }

        // MARK: Tasks
        public TaskManager LocalGameServerManager;
        public TaskManager BackendManager;

        // MARK: Controllers
        public LoginController LoginController;
        public MainController MainController;

        // MARK: UI
        private VisualElement _screenLogin;
        private VisualElement _screenMain;


        [MenuItem("Window/Rivet/Rivet", false, 20)]
        public static void ShowPlugin()
        {
            RivetPlugin window = GetWindow<RivetPlugin>();
            window.titleContent = new GUIContent("Rivet");
        }

        public void CreateGUI()
        {
            Singleton = this;

            PluginSettings.LoadSettings();

            m_VisualTreeAsset.CloneTree(rootVisualElement);

            var screens = rootVisualElement.Q(name: "Screens");
            _screenLogin = screens.Q(name: "Login");
            _screenMain = screens.Q(name: "Main");

            // Bind links
            var links = rootVisualElement.Q(name: "Header").Q(name: "Links");
            links.Q(name: "DashboardButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://hub.rivet.gg"));
            rootVisualElement.Q(className: "docsButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://rivet.gg/docs/unity"));
            rootVisualElement.Q(className: "discordButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://rivet.gg/discord"));
            rootVisualElement.Q(className: "feedbackButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://hub.rivet.gg/?modal=feedback&utm=unity"));

            LoginController = new LoginController(this, _screenLogin);
            MainController = new MainController(this, _screenMain);

            SetScreen(Screen.Login);
        }

        public void SetScreen(Screen screen)
        {
            _screen = screen;
            _screenLogin.style.display = screen == Screen.Login ? DisplayStyle.Flex : DisplayStyle.None;
            _screenMain.style.display = screen == Screen.Main ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public void OnEnable()
        {
            RivetLogger.Log("On Enable");

            // Task managers
            LocalGameServerManager = new(
                initMessage: "Open \"Develop\" and press \"Start\" to start game server.",
                getTaskConfig: async () =>
                {
                    if (LocalGameServerExecutablePath != null)
                    {
                        return new TaskConfig
                        {
                            Name = "exec_command",
                            Input = new JObject
                            {
                                ["cwd"] = Builder.ProjectRoot(),
                                ["cmd"] = LocalGameServerExecutablePath,
                                ["args"] = new JArray { "-batchmode", "-nographics", "-server" },
                            }
                        };
                    }
                    else
                    {
                        return null;
                    }
                },
                getTaskPanel: () => GameServerWindow.GetWindowIfExists()
            );

            BackendManager = new(
                initMessage: "Auto-started by Rivet plugin.",
                getTaskConfig: async () =>
                {
                    // Choose port to run on. This is to avoid potential conflicts with
                    // multiple projects running at the same time.
                    var chooseRes = await new RivetTask("backend_choose_local_port", new JObject()).RunAsync();
                    int port;
                    switch (chooseRes)
                    {
                        case ResultOk<JObject> ok:
                            port = (int)ok.Data["port"];
                            LocalBackendPort = port;
                            break;
                        case ResultErr<JObject> err:
                            RivetLogger.Error($"Failed to choose port: {err}");
                            return null;
                        default:
                            return null;
                    }


                    return new TaskConfig
                    {
                        Name = "backend_dev",
                        Input = new JObject
                        {
                            ["port"] = port,
                            ["cwd"] = Builder.ProjectRoot(),
                        }
                    };
                },
                getTaskPanel: () => BackendWindow.GetWindowIfExists(),
                autoRestart: true
            );

            // Shut down on reload
            AssemblyReloadEvents.beforeAssemblyReload += () => {
                RivetLogger.Log("Before Assembly Reload");
                ShutdownPlugin();
            };

            // // Start backend
            // _ = BackendManager.StartTask();


        }

        public void OnDisable()
        {
            RivetLogger.Log("On Disable");
            ShutdownPlugin();
        }

        /// <summary>
        /// Shuts down any tasks that might be running in the plugin.
        /// </summary>
        private void ShutdownPlugin()
        {

            RivetLogger.Log("Shutdown Plugin");
            LocalGameServerManager.StopTask();
            BackendManager.StopTask();
        }
    }
}