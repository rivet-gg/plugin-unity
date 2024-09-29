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

            // Create dock
            Singleton = this;

            // Create global
            RivetGlobal.Singleton = new();

            // Load settings
            PluginSettings.LoadSettings();
            SharedSettings.LoadSettings();

            // Task managers
            var plugin = RivetGlobal.Singleton;
            LocalGameServerManager = new(
                initMessage: "Open \"Develop\" and press \"Start\" to start game server.",
                getStartConfig: async () =>
                {
                    if (plugin.LocalGameServerExecutablePath != null)
                    {
                        return new TaskConfig
                        {
                            Name = "game_server.start",
                            Input = new JObject
                            {
                                ["cwd"] = Builder.ProjectRoot(),
                                ["cmd"] = plugin.LocalGameServerExecutablePath,
                                ["args"] = new JArray { "-batchmode", "-nographics", "-server" },
                            }
                        };
                    }
                    else
                    {
                        RivetLogger.Warning("LocalGameServerManager.Start: no local game server executable path");
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
            _ = RivetGlobal.Singleton.Bootstrap();

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

        public void OnBootstrap()
        {
            _developController.OnBootstrap();
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
