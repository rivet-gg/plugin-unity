using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace Rivet.Editor
{
    public class BuildScript : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            PluginSettings.LoadSettings();

            // Create streaming assets dir if needed
            string streamingAssetsPath = Application.streamingAssetsPath;
            if (!Directory.Exists(streamingAssetsPath))
            {
                Directory.CreateDirectory(streamingAssetsPath);
            }

            // Create asset file
            var data = new RivetConfig
            {
                BackendEndpoint = SharedSettings.BackendEndpoint,
                GameVersion = SharedSettings.GameVersion,
            };

            string json = JsonConvert.SerializeObject(data);
            string filePath = Path.Combine(Application.streamingAssetsPath, "rivet_config.json");
            File.WriteAllText(filePath, json);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "rivet_config.json");
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }

            string metaFilePath = jsonFilePath + ".meta";
            if (File.Exists(metaFilePath))
            {
                File.Delete(metaFilePath);
            }
        }
    }

    internal class RivetConfig
    {
        [JsonProperty("backend_endpoint")] public string BackendEndpoint;
        [JsonProperty("game_version")] public string GameVersion;
    }
}