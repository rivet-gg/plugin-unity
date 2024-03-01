using UnityEngine;
using System.Collections.Generic;

namespace Rivet
{
    public class RivetHelper : MonoBehaviour
    {
        public delegate void StartServer();
        public static event StartServer OnStartServer;

        public delegate void StartClient();
        public static event StartClient OnStartClient;

        private bool multiplayerSetup = false;
        private Dictionary<int, string> playerTokens = new Dictionary<int, string>();
        private string playerToken = null;

        // Singleton instance
        public static RivetHelper Instance { get; private set; }

        private void Awake()
        {
            // Singleton setup
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsDedicatedServer()
        {
            // Check if the environment variable UNITY_SERVER is set
            return System.Environment.GetEnvironmentVariable("UNITY_SERVER") == "1";
        }

        public void SetupMultiplayer()
        {
            Debug.Assert(!multiplayerSetup, "RivetHelper.SetupMultiplayer already called");
            multiplayerSetup = true;

            // Setup SceneMultiplayer here

            if (IsDedicatedServer())
            {
                Debug.Log("Starting server");
                OnStartServer?.Invoke();
            }
            else
            {
                Debug.Log("Starting client");
                OnStartClient?.Invoke();
            }
        }

        public void SetPlayerToken(string _playerToken)
        {
            Debug.Assert(multiplayerSetup, "RivetHelper.SetupMultiplayer has not been called");
            Debug.Assert(!IsDedicatedServer(), "Cannot call RivetHelper.SetPlayerToken on server");
            playerToken = _playerToken;
        }

        // Implement other methods here
    }
}