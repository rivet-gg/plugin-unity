#define IN_EDITOR

using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Rivet
{
    public class RivetPluginWindow : EditorWindow
    {
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