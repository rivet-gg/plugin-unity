#define IN_EDITOR

using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

public static class ExtensionData
{
    public static string RivetToken { get; set; }
    private static string apiEndpoint = "https://api.rivet.gg";

    public static string ApiEndpoint
    {
        get { return apiEndpoint; }
        set { apiEndpoint = value; }
    }
}

namespace Rivet
{
    public class RivetPluginWindow : EditorWindow
    {
        public string ApiEndpoint
        {
            get { return ExtensionData.ApiEndpoint; }
            set { ExtensionData.ApiEndpoint = value; }
        }

        public string RivetToken
        {
            get { return ExtensionData.RivetToken; }
            set { ExtensionData.RivetToken = value; }
        }

        // Define an interface for the states
        public interface IState
        {
            void OnEnter(RivetPluginWindow pluginWindow);
            void OnGUI();
        }

        [MenuItem("Window/Rivet Plugin")]
        public static void ShowWindow()
        {
            GetWindow<RivetPluginWindow>("Rivet Plugin");
        }

        // Add a variable to hold the current state
        public IState currentState;

        // Add a method to handle the state transitions
        public void TransitionToState(IState newState)
        {
            currentState = newState;
            currentState.OnEnter(this);
        }

        void OnGUI()
        {
            // Call the OnGUI method of the current state
            currentState.OnGUI();
        }

        void OnEnable()
        {
            // Initialize the state machine
            TransitionToState(new Installer());
        }
    }
}