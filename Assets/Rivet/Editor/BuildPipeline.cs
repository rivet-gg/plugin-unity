using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        // Create the asset file before the build
        RivetSettings data = ScriptableObject.CreateInstance<RivetSettings>();
        data.ApiEndpoint = ExtensionData.ApiEndpoint;
        data.RivetToken = ExtensionData.RivetToken;
        AssetDatabase.CreateAsset(data, "Assets/rivet_export.asset");
        AssetDatabase.SaveAssets();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        // Delete the asset file after the build
        AssetDatabase.DeleteAsset("Assets/rivet_export.asset");
    }
}