using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rivet.Editor.Types
{
    public struct BootstrapData
    {
        [JsonProperty("cloud")] public CloudData? Cloud;
    }

    public struct CloudData
    {
        [JsonProperty("token")] public string Token;
        [JsonProperty("api_endpoint")] public string ApiEndpoint;
        [JsonProperty("game_id")] public string GameId;
        [JsonProperty("envs")] public List<RivetEnvironment> Environments;
        [JsonProperty("backends")] public Dictionary<string, EnvironmentBackend> Backends;
        [JsonProperty("current_builds")] public Dictionary<string, ServersBuild> CurrentBuilds;
    }

    public struct RivetEnvironment
    {
        [JsonProperty("id")] public string Id;
        [JsonProperty("created_at")] public string CreatedAt;
        [JsonProperty("slug")] public string Slug;
        [JsonProperty("name")] public string Name;
    }

    public struct EnvironmentBackend
    {
        [JsonProperty("created_at")] public string CreatedAt;
        [JsonProperty("endpoint")] public string Endpoint;
        [JsonProperty("id")] public string Id;
        [JsonProperty("slug")] public string Slug;
        [JsonProperty("tier")] public string Tier;
    }

    public struct ServersBuild
    {
        [JsonProperty("id")] public string id;
        [JsonProperty("name")] public string Name;
        [JsonProperty("tags")] public Dictionary<string, string> Tags;
    }
}