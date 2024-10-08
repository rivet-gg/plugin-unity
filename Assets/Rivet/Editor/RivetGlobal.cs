using UnityEditor.Build;
using System.Threading.Tasks;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rivet.Editor.Types;
using Rivet.Editor.UI.Dock;

namespace Rivet.Editor
{
    public enum EnvironmentType
    {
        Local = 0, Remote = 1,
    }

    public class RivetGlobal
    {
        public static RivetGlobal? Singleton;

        // MARK: Bootstrap
        // Null if not bootstrapped yet
        public BootstrapData? BootstrapData;

        // Null if not authenticated
        public CloudData? CloudData
        {
            get
            {
                return BootstrapData?.Cloud;
            }
        }

        // If the user has the credentials required to connect to Rivet Cloud.
        public bool IsAuthenticated
        {
            get
            {
                return CloudData != null;
            }
        }

        // MARK: Environment
        public EnvironmentType EnvironmentType
        {
            get { return PluginSettings.EnvironmentType; }
            set
            {
                PluginSettings.EnvironmentType = value;
                SharedSettings.UpdateFromPlugin();
            }
        }
        public string? RemoteEnvironmentId
        {
            get { return PluginSettings.RemoteEnvironmentId; }
            set
            {
                PluginSettings.RemoteEnvironmentId = value;
                SharedSettings.UpdateFromPlugin();
            }
        }
        public RivetEnvironment? RemoteEnvironment
        {
            get
            {
                return RemoteEnvironmentIndex != null ? CloudData?.Environments[RemoteEnvironmentIndex.Value] : null;
            }
        }
        public int? RemoteEnvironmentIndex
        {
            get
            {
                if (CloudData is { } data)
                {
                    var idx = data.Environments.FindIndex(x => x.Id == RemoteEnvironmentId);
                    return idx >= 0 ? idx : 0;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value != null && value >= 0 && value < CloudData?.Environments.Count)
                {
                    RemoteEnvironmentId = CloudData?.Environments[value.Value].Id;
                }
            }
        }

        // MARK: Port
        public int LocalBackendPort = 6420;

        public string LocalBackendEndpoint
        {
            get
            {
                return $"http://127.0.0.1:{LocalBackendPort}";
            }
        }

        public int LocalEditorPort = 6421;

        public string LocalEditorEndpoint
        {
            get
            {
                return $"http://127.0.0.1:{LocalEditorPort}";
            }
        }

        // Endpoint to connect to
        public string BackendEndpoint
        {
            get
            {
                switch (EnvironmentType)
                {
                    case EnvironmentType.Local:
                        return LocalBackendEndpoint;
                    case EnvironmentType.Remote:
                        if (CloudData is { } cloudData && RemoteEnvironmentId is { } remoteEnvId)
                        {
                            return cloudData.Backends[remoteEnvId].Endpoint;
                        }
                        else
                        {
                            RivetLogger.Error("BackendEndpoint: unreachable");
                            return "";
                        }
                    default:
                        RivetLogger.Error("BackendEndpoint: unreachable");
                        return "";
                }
            }
        }

        // If the Rivet SDK has been generated.
        public bool BackendSdkExists = false;

        // The current deployed build slug.
        public string CurrentBuildSlug
        {
            get
            {
                switch (EnvironmentType)
                {
                    case EnvironmentType.Local:
                        return "local";
                    case EnvironmentType.Remote:
                        if (CloudData is { } cloudData && RemoteEnvironmentId is { } remoteEnvId)
                        {
                            if (cloudData.CurrentBuilds.TryGetValue(remoteEnvId, out var build) && build.Tags.TryGetValue("version", out var versionTag))
                            {
                                return versionTag;
                            }
                            else
                            {
                                RivetLogger.Error("CurrentBuildSlug: no current build or version in build");
                                return "";
                            }
                        }
                        else
                        {
                            RivetLogger.Error("CurrentBuildSlug: not authenticated");
                            return "";
                        }
                    default:
                        RivetLogger.Error("BackendEndpoint: unreachable");
                        return "";
                }
            }
        }

        // MARK: Local Game Server
        public string? LocalGameServerExecutablePath;

        // MARK: Bootstrap
        public async Task Bootstrap()
        {
            var result = await new RivetTask("get_bootstrap_data", new JObject()).RunAsync();
            if (result is ResultErr<JObject> err)
            {
                return;
            }

            // Save data
            var data = result.Data.ToObject<BootstrapData>(); ;
            BootstrapData = data;

            // Update configuration
            SharedSettings.UpdateFromPlugin();

            // Emit event
            Dock.Singleton?.OnBootstrap();
        }
    }
}