#nullable enable

using UnityEngine;
using UnityEditor;
using Rivet.Editor.UI;
using Rivet.UI.Screens;

namespace Rivet.Editor
{
    static class PluginSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreatePluginSettingsProvider()
        {
            var provider = new SettingsProvider(PluginSettings.SettingsPath, SettingsScope.Project)
            {
                label = "Rivet",
                guiHandler = (searchContext) =>
                {
                    var settings = PluginSettings.GetSerializedSettings();
                    EditorGUILayout.PropertyField(settings.FindProperty("enableDebugLogs"), new GUIContent("Enable Debug Logs"));
                    settings.ApplyModifiedProperties();
                },
                keywords = new System.Collections.Generic.HashSet<string>(new[] { "Plugin", "Debug", "Logs" })
            };

            return provider;
        }
    }

    /// <summary>
    /// Settings specific to the plugin.
    /// </summary>
    public class PluginSettings : ScriptableObject
    {
        public const string SettingsPath = "Project/Rivet";

        public static PluginSettings? Settings;


        public static void LoadSettings()
        {
            Settings = GetOrCreateSettings();
        }

        // MARK: Ser/de
        internal static PluginSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<PluginSettings>("Assets/Rivet/Editor/PluginSettings.asset");
            if (settings == null)
            {
                settings = CreateInstance<PluginSettings>();
                AssetDatabase.CreateAsset(settings, "Assets/Rivet/Editor/PluginSettings.asset");
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

        private static void SaveSettings()
        {
            if (Settings != null)
            {
                EditorUtility.SetDirty(Settings);
                AssetDatabase.SaveAssets();
            }
        }

        // MARK: Settings
        [SerializeField]
        private EnvironmentType environmentType = EnvironmentType.Local;

        public static EnvironmentType EnvironmentType
        {
            get { return Settings != null ? Settings.environmentType : EnvironmentType.Local; }
            set
            {
                if (Settings != null)
                {
                    Settings.environmentType = value;
                    SaveSettings();
                }
            }
        }

        [SerializeField]
        private string? remoteEnvironmentId;

        public static string? RemoteEnvironmentId
        {
            get { return Settings?.remoteEnvironmentId; }
            set
            {
                if (Settings != null)
                {
                    Settings.remoteEnvironmentId = value;
                    SaveSettings();
                }
            }
        }

        [SerializeField]
        private bool enableDebugLogs = false;

        public static bool EnableDebugLogs
        {
            get { return Settings != null && Settings.enableDebugLogs; }
        }

        // TODO: Remove this with cleaner impl
        [SerializeField]
        private int tempBackendLocalPort = 6420;

        public static int TEMPBackendLocalPort
        {
            get { return Settings?.tempBackendLocalPort ?? 6420; }
            set
            {
                if (Settings != null)
                {
                    Settings.tempBackendLocalPort = value;
                    SaveSettings();
                }
            }

        }
    }

    /// <summary>
    /// Shared settings that can be used by the PIE game instance.
    /// 
    /// See ExportConfig for more context.
    /// </summary>
    public static class SharedSettings
    {
        private static string backendEndpoint;
        private static string gameVersion;

        public static string BackendEndpoint
        {
            get => backendEndpoint;
            set => SetAndSave(ref backendEndpoint, value, "BackendEndpoint");
        }

        public static string GameVersion
        {
            get => gameVersion;
            set => SetAndSave(ref gameVersion, value, "GameVersion");
        }

        public static void LoadSettings()
        {
            backendEndpoint = PlayerPrefs.GetString("BackendEndpoint");
            gameVersion = PlayerPrefs.GetString("GameVersion");
        }

        private static void SetAndSave(ref string field, string value, string prefKey)
        {
            field = value;
            EditorApplication.delayCall += () => PlayerPrefs.SetString(prefKey, value);
        }

        /// <summary>
        /// Updates relevant shared settings from the plugin.
        /// </summary>
        public static void UpdateFromPlugin()
        {
            if (RivetPlugin.Singleton is { } plugin)
            {
                BackendEndpoint = plugin.MainController.EnvironmentType switch
                {
                    Rivet.UI.Screens.EnvironmentType.Local => $"http://localhost:{plugin.LocalBackendPort}",
                    Rivet.UI.Screens.EnvironmentType.Remote => plugin.MainController.RemoteEnvironmentBackend?.Endpoint ?? "http://localhost:6420",
                    _ => throw new System.NotImplementedException(),
                };

                GameVersion = plugin.GameVersion ?? "unknown";

                RivetLogger.Log($"Update Shared Settings [BackendEndpoint={BackendEndpoint} GameVersion={GameVersion}]");
            }
        }
    }
}