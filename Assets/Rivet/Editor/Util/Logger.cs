using UnityEngine;

namespace Rivet.Editor
{
    public static class RivetLogger
    {
        public static void Log(string message)
        {
            if (PluginSettings.EnableDebugLogs)
            {
                Debug.Log($"[Rivet] {message}");
            }
        }

        public static void Warning(string message) => Debug.LogWarning($"[Rivet] {message}");

        public static void Error(string message) => Debug.LogError($"[Rivet] {message}");
    }

}