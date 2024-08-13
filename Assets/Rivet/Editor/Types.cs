using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rivet.Editor.Types
{
    public struct BootstrapData
    {
        [JsonProperty("token")] public string Token;
        [JsonProperty("api_endpoint")] public string ApiEndpoint;
        [JsonProperty("game_id")] public string GameId;
        [JsonProperty("backend_project")] public BackendProject BackendProject;
        [JsonProperty("backend_environments")] public List<BackendEnvironment> BackendEnvironments;
    }

    public struct BackendProject
    {
        [JsonProperty("project_id")] public string ProjectId;
    }

    public struct BackendEnvironment
    {
        [JsonProperty("display_name")] public string DisplayName;
        [JsonProperty("endpoint")] public string Endpoint;
        [JsonProperty("environment_id")] public string EnvironmentId;
        [JsonProperty("name_id")] public string NameId;
    }

}