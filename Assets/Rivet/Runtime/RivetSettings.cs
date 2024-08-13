#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Rivet.Runtime
{
    [System.Serializable]
    public class RivetSettings
    {
        public string? ApiEndpoint;
        public string? CloudToken;

        private string? GetApiEndpoint()
        {
            string? endpoint = PlayerPrefs.GetString("ApiEndpoint");
            if (string.IsNullOrEmpty(endpoint))
            {
                var rivetSettings = LoadRivetSettings();
                if (rivetSettings != null)
                {
                    endpoint = rivetSettings.ApiEndpoint;
                }
            }
            return endpoint;
        }

        private string? GetCloudToken()
        {
            string? token = PlayerPrefs.GetString("CloudToken");
            if (string.IsNullOrEmpty(token))
            {
                var rivetSettings = LoadRivetSettings();
                if (rivetSettings != null)
                {
                    token = rivetSettings.CloudToken;
                }
            }
            return token;
        }

        private RivetSettings? LoadRivetSettings()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "rivet_export.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                RivetSettings rivetSettings = JsonUtility.FromJson<RivetSettings>(json);
                return rivetSettings;
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
                return null;
            }
        }
    }
}