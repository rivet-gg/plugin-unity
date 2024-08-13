using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Rivet.UI.Screens;
using Rivet.Editor.UI.TaskPanel;

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

        private VisualElement _screenLogin;
        private VisualElement _screenMain;

        public LoginController LoginController;
        public MainController MainController;

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
    }
}