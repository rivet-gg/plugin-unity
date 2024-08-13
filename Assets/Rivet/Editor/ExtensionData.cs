using UnityEngine;

namespace Rivet.Editor
{
    /// <summary>
    /// Provides extension data for the Rivet plugin.
    /// </summary>
    public static class ExtensionData
    {
        private static string apiEndpoint;
        private static string cloudToken;

        public static string ApiEndpoint
        {
            get => apiEndpoint;
            set => SetAndSave(ref apiEndpoint, value, "ApiEndpoint");
        }

        public static string CloudToken
        {
            get => cloudToken;
            set => SetAndSave(ref cloudToken, value, "RivetToken");
        }

        private static void SetAndSave(ref string field, string value, string prefKey)
        {
            field = value;
            UnityEditor.EditorApplication.delayCall += () => PlayerPrefs.SetString(prefKey, value);
        }
    }
}