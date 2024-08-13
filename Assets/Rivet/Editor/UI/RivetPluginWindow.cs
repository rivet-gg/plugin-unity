using UnityEngine;
using UnityEditor;

namespace Rivet.Editor.UI
{
    public class RivetPluginWindow : EditorWindow
    {
        // Assign this in the Unity editor
        // public Texture logoTexture;

        public string ApiEndpoint
        {
            get { return ExtensionData.ApiEndpoint; }
            set { ExtensionData.ApiEndpoint = value; }
        }

        public string RivetToken
        {
            get { return ExtensionData.CloudToken; }
            set { ExtensionData.CloudToken = value; }
        }

        // Define an interface for the states
        public interface IState
        {
            void OnEnter(RivetPluginWindow pluginWindow);
            void OnGUI();
        }

        [MenuItem("Window/Rivet")]
        public static void ShowWindow()
        {
            GetWindow<RivetPluginWindow>("Rivet");
        }

        public IState currentState;

        public void TransitionToState(IState newState)
        {
            currentState = newState;
            currentState.OnEnter(this);
        }

        void OnGUI()
        {
            PluginSettings.LoadSettings();

            // Logo
            // GUILayout.Label(logoTexture, GUILayout.Width(200), GUILayout.Height(200));

            // Links
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Dashboard")) Application.OpenURL("https://hub.rivet.gg/");
            if (GUILayout.Button("Documentation")) Application.OpenURL("https://rivet.gg/docs");
            if (GUILayout.Button("Discord")) Application.OpenURL("https://rivet.gg/discord");
            if (GUILayout.Button("Report Bug & Feedback")) Application.OpenURL("https://hub.rivet.gg/?modal=feedback&utm=godot");
            GUILayout.EndHorizontal();

            // Window
            currentState.OnGUI();
        }

        void OnEnable()
        {
            // Initialize the state machine
            TransitionToState(new Screens.Login.Window());
        }
    }
}