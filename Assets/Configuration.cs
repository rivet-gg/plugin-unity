#nullable enable

using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// TODO: Move in to generated backend
namespace Backend
{
    /// <summary>
    ///  Derive config in order of priority:
    /// - Environment variable (if running from deployed server)
    /// - Configuration (if running in a client)
    /// - Fallback
    /// </summary>
    public class Configuration
    {
        public string BackendEndpoint
        {
            get
            {
                string env = Environment.GetEnvironmentVariable("BACKEND_ENDPOINT");
                if (!string.IsNullOrEmpty(env)) return env;

                var prefs = PlayerPrefs.GetString("BackendEndpoint");
                if (!string.IsNullOrEmpty(prefs)) return prefs;

                var config = LoadRivetConfig();
                if (config != null) return config.BackendEndpoint;

                Debug.LogWarning("Could not find BackendEndpoint");
                return "http://localhost:6420";
            }
        }

        public string GameVersion
        {
            get
            {
                string env = Environment.GetEnvironmentVariable("GAME_VERSION");
                if (!string.IsNullOrEmpty(env)) return env;

                var prefs = PlayerPrefs.GetString("GameVersion");
                if (!string.IsNullOrEmpty(prefs)) return prefs;

                var config = LoadRivetConfig();
                if (config != null) return config.GameVersion;


                Debug.LogWarning("Could not find GameVersion");
                return "unknown";
            }
        }

        /// <summary>
        /// Load the config exported from the build pipeline.
        /// </summary>
        /// <returns></returns>
        private RivetConfig? LoadRivetConfig()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "rivet_config.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                RivetConfig? rivetSettings = JsonConvert.DeserializeObject<RivetConfig>(json);
                if (rivetSettings != null)
                {
                    return rivetSettings;
                }
                else
                {
                    Debug.LogError("Deserialize RivetConfig is null");
                    return null;
                }
            }
            else
            {
                Debug.LogError("Config file not found: " + filePath);
                return null;
            }
        }
    }

    /// <summary>
    /// Rivet config that gets loaded at runtime.
    /// </summary>
    internal class RivetConfig
    {
        [JsonProperty("backend_endpoint")] public string BackendEndpoint = "";
        [JsonProperty("game_version")] public string GameVersion = "";
    }
}