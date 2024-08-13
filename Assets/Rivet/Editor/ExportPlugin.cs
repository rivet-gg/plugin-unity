using System.Linq;
using UnityEditor;

namespace Rivet.Editor
{
    /// <summary>
    /// Represents a class that exports a package containing assets related to the Rivet plugin.
    /// </summary>
    public class ExportPackage
    {
        /// <summary>
        /// Exports the assets related to the Rivet plugin as a Unity package.
        /// </summary>
        public static void Export()
        {
            PluginSettings.LoadSettings();

            string[] projectContent = AssetDatabase.GetAllAssetPaths();
            var assetsToExport = projectContent.Where(path => path.StartsWith("Assets/Rivet")).ToArray();
            AssetDatabase.ExportPackage(assetsToExport, "Rivet.unitypackage", ExportPackageOptions.Recurse);
        }
    }
}