using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Rivet.Editor;
using Rivet.Editor.Types;
using Rivet.Editor.UI;
using Rivet.Editor.UI.TaskPanel;
using Rivet.Editor.Util;
using Rivet.UI.Tabs;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rivet.UI.Screens
{
    public enum MainTab
    {
        Setup, Develop, Deploy, Settings,
    }

    public enum EnvironmentType
    {
        Local = 0, Remote = 1,
    }


    public class MainController
    {
        private readonly RivetPlugin _pluginWindow;
        private readonly VisualElement _root;

        // MARK: Tasks
        public TaskManager LocalGameServerManager;
        public TaskManager BackendManager;

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

        private VisualElement _deployTabButton;
        private VisualElement _deployTabBody;
        private DeployController _deployController;

        private VisualElement _settingsTabButton;
        private VisualElement _settingsTabBody;
        private SettingsController _settingsController;

        // MARK: Environment
        public EnvironmentType EnvironmentType = EnvironmentType.Local;
        public string? RemoteEnvironmentId;
        public BackendEnvironment? RemoteEnvironment
        {
            get
            {
                return BootstrapData?.BackendEnvironments[RemoteEnvironmentIndex];
            }
        }
        public int RemoteEnvironmentIndex
        {
            get
            {
                if (BootstrapData is { } data)
                {
                    var idx = data.BackendEnvironments.FindIndex(x => x.EnvironmentId == RemoteEnvironmentId);
                    return idx >= 0 ? idx : -1;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value >= 0 && value < BootstrapData?.BackendEnvironments.Count)
                {
                    RemoteEnvironmentId = BootstrapData?.BackendEnvironments[value].EnvironmentId;
                }
            }
        }

        // MARK: Local Game Server
        public string? LocalGameServerExecutablePath;

        public MainController(RivetPlugin window, VisualElement root)
        {
            _pluginWindow = window;
            _root = root;

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

            // UI
            InitUI();
            SetTab(MainTab.Setup);

            // Fetch data
            _ = GetBootstrapData();

            // Start backend
            _ = BackendManager.StartTask();
        }

        void InitUI()
        {
            var tabBar = _root.Q(name: "TabBar");
            var tabBody = _root.Q(name: "TabBody");

            _setupTabButton = tabBar.Q(name: "Setup");
            _setupTabBody = tabBody.Q(name: "Setup");
            _setupTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Setup));
            _setupController = new SetupController(_pluginWindow, _setupTabBody);

            _developTabButton = tabBar.Q(name: "Develop");
            _developTabBody = tabBody.Q(name: "Develop");
            _developTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Develop));
            _developController = new DevelopController(_pluginWindow, this, _developTabBody);

            _deployTabButton = tabBar.Q(name: "Deploy");
            _deployTabBody = tabBody.Q(name: "Deploy");
            _deployTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Deploy));
            _deployController = new DeployController(_pluginWindow, this, _deployTabBody);

            _settingsTabButton = tabBar.Q(name: "Settings");
            _settingsTabBody = tabBody.Q(name: "Settings");
            _settingsTabButton.RegisterCallback<ClickEvent>(ev => SetTab(MainTab.Settings));
            _settingsController = new SettingsController(_pluginWindow, this, _settingsTabBody);
        }

        void SetTab(MainTab tab)
        {
            _tab = tab;

            _setupTabButton.EnableInClassList("active", tab == MainTab.Setup);
            _setupTabBody.style.display = tab == MainTab.Setup ? DisplayStyle.Flex : DisplayStyle.None;

            _developTabButton.EnableInClassList("active", tab == MainTab.Develop);
            _developTabBody.style.display = tab == MainTab.Develop ? DisplayStyle.Flex : DisplayStyle.None;

            _deployTabButton.EnableInClassList("active", tab == MainTab.Deploy);
            _deployTabBody.style.display = tab == MainTab.Deploy ? DisplayStyle.Flex : DisplayStyle.None;

            _settingsTabButton.EnableInClassList("active", tab == MainTab.Settings);
            _settingsTabBody.style.display = tab == MainTab.Settings ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private async Task GetBootstrapData()
        {
            var result = await new RivetTask("get_bootstrap_data", new JObject()).RunAsync();
            if (result is ResultErr<JObject>) return;

            var data = result.Data.ToObject<BootstrapData>(); ;
            BootstrapData = data;
            ExtensionData.ApiEndpoint = data.ApiEndpoint;

            _developController.OnBootstrap(data);
            _deployController.OnBootstrap(data);
        }

        /// <summary>
        /// Called when the selected environment changes.
        /// </summary>
        public void OnSelectedEnvironmentChange()
        {
            _developController.OnSelectedEnvironmentChange();
            _deployController.OnSelectedEnvironmentChange();
        }
    }
}