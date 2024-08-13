using UnityEngine;
using UnityEditor;

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

    public class PluginSettings : ScriptableObject
    {
        public const string SettingsPath = "Project/Rivet";

        public static PluginSettings? Settings;

        [SerializeField]
        private bool enableDebugLogs = false;

        public static void LoadSettings() {
            Settings = GetOrCreateSettings();
        }

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

        public static bool EnableDebugLogs
        {
            get { return Settings != null && Settings.enableDebugLogs; }
        }
    }
}