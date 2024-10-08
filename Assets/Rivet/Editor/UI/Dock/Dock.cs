using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Rivet.Editor.Types;
using Rivet.Editor.UI.Dock.Tabs;
using Rivet.Editor.UI.TaskPanel;
using Rivet.Editor.Util;

namespace Rivet.Editor.UI.Dock
{
    public enum MainTab
    {
        Setup, Develop, Modules, Settings,
    }

    public enum EnvironmentType
    {
        Local = 0, Remote = 1,
    }


    public class Dock : EditorWindow
    {
        public static Dock? Singleton;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset;

        private VisualElement _root
        {
            get
            {
                return rootVisualElement;
            }
        }

        public string ApiEndpoint = "https://api.rivet.gg";

        // MARK: Local Game Server
        public string? LocalGameServerExecutablePath;

        // MARK: Backend
        // private int _localBackendPort = 6420;
        // public int LocalBackendPort {
        //     get { return _localBackendPort; }
        //     set {
        //         _localBackendPort = value;
        //         SharedSettings.UpdateFromPlugin();
        //     }
        // }
        public int LocalBackendPort
        {
            get { return PluginSettings.TEMPBackendLocalPort; }
            set
            {
                PluginSettings.TEMPBackendLocalPort = value;
                SharedSettings.UpdateFromPlugin();
            }
        }

        // MARK: Deployed Game Version
        public string? GameVersion;

        // MARK: Bootstrap
        public BootstrapData? BootstrapData;

        // MARK: Tabs
        private MainTab _tab = MainTab.Setup;

        private VisualElement _setupTabButton;
        private VisualElement _setupTabBody;
        private SetupController _setupController;

        private VisualElement _developTabButton;
        private VisualElement _developTabBody;
        private DevelopController _developController;

        private VisualElement _modulesTabButton;
        private VisualElement _modulesTabBody;
        private ModulesController _modulesController;

        private VisualElement _settingsTabButton;
        private VisualElement _settingsTabBody;
        private SettingsController _settingsController;

        // MARK: Environment
        public EnvironmentType EnvironmentType
        {
            get { return PluginSettings.EnvironmentType; }
            set
            {
                PluginSettings.EnvironmentType = value;
                SharedSettings.UpdateFromPlugin();
            }
        }
        public string? RemoteEnvironmentId
        {
            get { return PluginSettings.RemoteEnvironmentId; }
            set
            {
                PluginSettings.RemoteEnvironmentId = value;
                SharedSettings.UpdateFromPlugin();
            }
        }
        public RivetEnvironment? RemoteEnvironment
        {
            get
            {
                return RemoteEnvironmentIndex != null ? BootstrapData?.Environments[RemoteEnvironmentIndex.Value] : null;
            }
        }
        public int? RemoteEnvironmentIndex
        {
            get
            {
                if (BootstrapData is { } data)
                {
                    var idx = data.Environments.FindIndex(x => x.Id == RemoteEnvironmentId);
                    return idx >= 0 ? idx : 0;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value != null && value >= 0 && value < BootstrapData?.Environments.Count)
                {
                    RemoteEnvironmentId = BootstrapData?.Environments[value.Value].Id;
                }
            }
        }

        public EnvironmentBackend? RemoteEnvironmentBackend
        {
            get
            {
                var remoteEnv = RemoteEnvironment;
                return remoteEnv != null ? BootstrapData?.Backends[remoteEnv.Value.Id] : null;
            }
        }

        // MARK: Tasks
        public TaskManager LocalGameServerManager;
        public TaskManager BackendManager;


        [MenuItem("Window/Rivet/Rivet", false, 20)]
        public static void ShowPlugin()
        {
            Dock dock = GetWindow<Dock>();
            dock.titleContent = new GUIContent("Rivet");
        }

        public void CreateGUI()
        {
            m_VisualTreeAsset.CloneTree(rootVisualElement);

            // Bind links
            var links = rootVisualElement.Q(name: "Header").Q(name: "Links");
            links.Q(name: "SignInButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://rivet.gg/learn/unity"));
            links.Q(name: "HubButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://rivet.gg/learn/unity"));
            links.Q(name: "DocsButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://rivet.gg/learn/unity"));
            links.Q(name: "FeedbackButton").RegisterCallback<ClickEvent>((ev) => Application.OpenURL("https://hub.rivet.gg/?modal=feedback&utm=unity"));

            var tabBar = _root.Q(name: "TabBar");
            var tabBody = _root.Q(name: "TabBody");

            _setupTabButton = tabBar.Q(name: "Setup");
            _setupTabBody = tabBody.Q(name: "Setup");
            _setupTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Setup));
            _setupController = new SetupController(this, _setupTabBody);

            _developTabButton = tabBar.Q(name: "Develop");
            _developTabBody = tabBody.Q(name: "Develop");
            _developTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Develop));
            _developController = new DevelopController(this, _developTabBody);

            _modulesTabButton = tabBar.Q(name: "Modules");
            _modulesTabBody = tabBody.Q(name: "Modules");
            _modulesTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Modules));
            _modulesController = new ModulesController(this, _modulesTabBody);

            _settingsTabButton = tabBar.Q(name: "Settings");
            _settingsTabBody = tabBody.Q(name: "Settings");
            _settingsTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Settings));
            _settingsController = new SettingsController(this, _settingsTabBody);

            SetTab(MainTab.Setup);
        }

        public void OnEnable()
        {
            RivetLogger.Log("On Enable");

            Singleton = this;

            PluginSettings.LoadSettings();
            SharedSettings.LoadSettings();

            // Task managers
            LocalGameServerManager = new(
                initMessage: "Open \"Develop\" and press \"Start\" to start game server.",
                getStartConfig: async () =>
                {
                    if (LocalGameServerExecutablePath != null)
                    {
                        return new TaskConfig
                        {
                            Name = "game_server.start",
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
                getStopConfig: async () =>
                {
                    return new TaskConfig
                    {
                        Name = "game_server.stop",
                        Input = new JObject { }
                    };
                },
                getTaskPanel: () => GameServerWindow.GetWindowIfExists()
            );

            BackendManager = new(
                initMessage: "Auto-started by Rivet plugin.",
                getStartConfig: async () =>
                {
                    return new TaskConfig
                    {
                        Name = "backend.start",
                        Input = new JObject
                        {
                            ["cwd"] = Builder.ProjectRoot(),
                        }
                    };
                },
                getStopConfig: async () =>
                {
                    return new TaskConfig
                    {
                        Name = "backend.stop",
                        Input = new JObject { }
                    };
                },
                getTaskPanel: () => BackendWindow.GetWindowIfExists(),
                autoRestart: true
            );

            // Shut down on reload
            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                RivetLogger.Log("Before Assembly Reload");
                ShutdownPlugin();
            };

            // Bootstrap
            _ = GetBootstrapData();

            // Start backend
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

        void SetTab(MainTab tab)
        {
            _tab = tab;

            _setupTabButton.EnableInClassList("active", tab == MainTab.Setup);
            _setupTabBody.style.display = tab == MainTab.Setup ? DisplayStyle.Flex : DisplayStyle.None;

            _developTabButton.EnableInClassList("active", tab == MainTab.Develop);
            _developTabBody.style.display = tab == MainTab.Develop ? DisplayStyle.Flex : DisplayStyle.None;

            _modulesTabButton.EnableInClassList("active", tab == MainTab.Modules);
            _modulesTabBody.style.display = tab == MainTab.Modules ? DisplayStyle.Flex : DisplayStyle.None;

            _settingsTabButton.EnableInClassList("active", tab == MainTab.Settings);
            _settingsTabBody.style.display = tab == MainTab.Settings ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public async Task GetBootstrapData()
        {
            var result = await new RivetTask("get_bootstrap_data", new JObject()).RunAsync();
            if (result is ResultErr<JObject> err)
            {
                return;
            }

            var data = result.Data.ToObject<BootstrapData>(); ;
            BootstrapData = data;

            _developController.OnBootstrap(data);

            SharedSettings.UpdateFromPlugin();
        }

        /// <summary>
        /// Called when the selected environment changes.
        /// </summary>
        public void OnSelectedEnvironmentChange()
        {
            _developController.OnSelectedEnvironmentChange();
        }
    }
}
