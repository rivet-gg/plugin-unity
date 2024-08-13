using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Rivet.UI.Screens;

namespace Rivet.Editor.UI
{
    public enum Screen
    {
        Login,
        Main,
    }

    public class RivetPlugin : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset;

        private Screen _screen = Screen.Login;

        public string ApiEndpoint = "https://api.rivet.gg";

        private VisualElement _screenLogin;
        private VisualElement _screenMain;

        private LoginController _loginController;
        private MainController _mainController;

        [MenuItem("Window/UI Toolkit/RivetPlugin")]
        public static void ShowExample()
        {
            RivetPlugin wnd = GetWindow<RivetPlugin>();
            wnd.titleContent = new GUIContent("RivetPlugin");
        }

        public void CreateGUI()
        {
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

            _loginController = new LoginController(this, _screenLogin);
            _mainController = new MainController(this, _screenMain);

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