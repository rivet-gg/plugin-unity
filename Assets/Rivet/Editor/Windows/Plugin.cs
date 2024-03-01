using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.Security.Cryptography;

public struct BootstrapData
{
    [JsonProperty("api_endpoint")] public string ApiEndpoint;
    [JsonProperty("game_id")] public string GameId;
    [JsonProperty("token")] public string Token;
}

public class Region
{
    public string provider;
    public string provider_display_name;
    public string region_display_name;
    public string region_id;
    public string region_name_id;
    public string universal_region;
}

public class Namespace
{
    public string create_ts;
    public string display_name;
    public string name_id;
    public string namespace_id;
    public string version_id;
}

public class Version
{
    public string create_ts;
    public string display_name;
    public string version_id;
}

public class Game
{
    public List<Region> available_regions;
    public string create_ts;
    public string developer_group_id;
    public string display_name;
    public string game_id;
    public string name_id;
    public List<Namespace> namespaces;
    public int total_player_count;
    public List<Version> versions;
}

public class Watch
{
    public string index;
}

public class Root
{
    public Game game;
    public Watch watch;
}

public class GameData
{
    public List<(Namespace, Version)> namespaces;
}

namespace Rivet
{
    public class Plugin : RivetPluginWindow.IState
    {
        public Texture logoTexture; // Assign this in the Unity editor
        public GameData gameData;
        public int selectedIndex = 0;
        public bool showPlaytestOptions = true;
        public bool showDeployOptions = false;
        public bool showSettingsOptions = false;
        public RivetPluginWindow window;
        public BootstrapData bootstrapData;
        private bool thisMachineSelected = false;
        private string rivetEditorToken = "";

        public void OnEnter(RivetPluginWindow pluginWindow)
        {
            this.window = pluginWindow;
            GetBootstrapData();
        }

        private void GetBootstrapData()
        {
            new System.Threading.Thread(() =>
            {
                var getBootstrapResult = RivetCLI.RunCommand(
                    "sidekick",
                    "get-bootstrap-data"
                );

                switch (getBootstrapResult)
                {
                    case SuccessResult<JObject> successResult:
                        var data = successResult.Data["Ok"];
                        // TODO: Deserialize this better
                        bootstrapData = new BootstrapData
                        {
                            ApiEndpoint = data["api_endpoint"].ToString(),
                            GameId = data["game_id"].ToString(),
                            Token = data["token"].ToString()
                        };

                        Debug.Log("Bootstrap data: " + bootstrapData.ApiEndpoint + " " + bootstrapData.GameId + " " + bootstrapData.Token);

                        // Fetch plugin data
                        gameData = FetchPluginData();

                        break;
                    case ErrorResult<JObject> errorResult:
                        UnityEngine.Debug.LogError(errorResult.Message);
                        break;
                }
            }).Start();
        }

        public GameData FetchPluginData()
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{bootstrapData.ApiEndpoint}/cloud/games/{bootstrapData.GameId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bootstrapData.Token);
            var response = httpClient.SendAsync(request).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Debug.LogError("Failed to fetch plugin data");
                return null;
            }

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var root = JsonConvert.DeserializeObject<Root>(responseBody);

            var gameData = new GameData
            {
                namespaces = new List<(Namespace, Version)>()
            };

            foreach (var ns in root.game.namespaces)
            {
                var version = root.game.versions.FirstOrDefault(version => version.version_id == ns.version_id);

                if (version != null)
                {
                    gameData.namespaces.Add((ns, version));
                }
            }

            // Return the data to the main thread
            return gameData;
        }

        public void OnGUI()
        {
            // if (GUILayout.Button("Build and Deploy"))
            // {
            //     // Check if Linux build support is installed
            //     if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64))
            //     {
            //         Debug.LogError("Linux build support is not installed");
            //         // return;
            //     }

            //     // Set the build settings
            //     BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            //     {
            //         scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
            //         locationPathName = "build/LinuxServer/LinuxServer.x86_64", // Output path
            //         target = BuildTarget.StandaloneLinux64, // Target platform
            //         subtarget = (int)StandaloneBuildSubtarget.Server // Headless mode for server build
            //     };

            //     // Build the player
            //     BuildPipeline.BuildPlayer(buildPlayerOptions);

            //     // Run deploy with CLI
            //     new System.Threading.Thread(() =>
            //     {
            //         var result = RivetCLI.RunCommand(
            //             "sidekick",
            //             "--show-terminal",
            //             "deploy",
            //             "--namespace",
            //             gameData.namespaces[selectedIndex].Item1.name_id
            //         );
            //     }).Start();
            // }
            // return;

            // The existing GUI code goes here
            // Debug.LogError("Start 1");
            GUILayout.BeginVertical();

            try
            {

                // Logo
                GUILayout.Label(logoTexture, GUILayout.Width(200), GUILayout.Height(200));

                // Horizontal line
                GUILayout.Label("", GUI.skin.horizontalSlider);

                // Links
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Hub")) Application.OpenURL("https://hub.rivet.gg/");
                if (GUILayout.Button("Docs")) Application.OpenURL("https://rivet.gg/docs");
                if (GUILayout.Button("Discord")) Application.OpenURL("https://rivet.gg/discord");
                GUILayout.EndHorizontal();

                // Buttons
                if (GUILayout.Button("Playtest")) { showPlaytestOptions = true; showDeployOptions = false; showSettingsOptions = false; }
                if (GUILayout.Button("Deploy")) { showDeployOptions = true; showPlaytestOptions = false; showSettingsOptions = false; }
                if (GUILayout.Button("Settings")) { showSettingsOptions = true; showDeployOptions = false; showPlaytestOptions = false; }

                // Playtest options
                if (showPlaytestOptions)
                {
                    GUILayout.Label("Server");
                    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                    GUIStyle toggleButtonStyle = new GUIStyle(GUI.skin.button) { normal = GUI.skin.button.active };


                    GUILayout.BeginHorizontal();
                    bool thisMachineClicked = GUILayout.Button("This machine", thisMachineSelected ? toggleButtonStyle : buttonStyle);
                    if (thisMachineClicked) thisMachineSelected = true;

                    bool rivetServersClicked = GUILayout.Button("Rivet servers", !thisMachineSelected ? toggleButtonStyle : buttonStyle);
                    if (rivetServersClicked) thisMachineSelected = false;
                    GUILayout.EndHorizontal();

                    GUILayout.Label("Namespace");
                    if (gameData == null)
                    {
                        GUILayout.Label("Loading...");
                    }
                    else
                    {
                        var namespaces = gameData.namespaces.Select(space => space.Item1.display_name).ToArray();
                        if (namespaces.Length == 0)
                        {
                            GUILayout.Label("No namespaces found");
                        }
                        int oldSelectedIndex = selectedIndex;
                        selectedIndex = EditorGUILayout.Popup(selectedIndex, namespaces);

                        if (thisMachineClicked || rivetServersClicked || oldSelectedIndex != selectedIndex)
                        {
                            GetNamespaceToken();
                        }
                    }
                }

                // Deploy options
                if (showDeployOptions)
                {
                    GUILayout.Label("Build and deploy server");
                    if (gameData == null)
                    {
                        GUILayout.Label("Loading...");
                    }
                    else
                    {
                        var namespaces = gameData.namespaces.Select(space => space.Item1.display_name).ToArray();
                        if (namespaces.Length == 0)
                        {
                            GUILayout.Label("No namespaces found");
                        }
                        selectedIndex = EditorGUILayout.Popup(selectedIndex, namespaces);
                    }

                    if (GUILayout.Button("Build and Deploy"))
                    {
                        // Check if Linux build support is installed
                        if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64))
                        {
                            Debug.LogError("Linux build support is not installed");
                            // return;
                        }

                        // Set the build settings
                        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
                        {
                            scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray(),
                            locationPathName = "builds/LinuxServer/LinuxServer.x86_64", // Output path
                            target = BuildTarget.StandaloneLinux64, // Target platform
                            subtarget = (int)StandaloneBuildSubtarget.Server // Headless mode for server build
                        };

                        // Build the player
                        var result = BuildPipeline.BuildPlayer(buildPlayerOptions);
                        Debug.Log("Build result: " + result);

                        // Run deploy with CLI
                        new System.Threading.Thread(() =>
                        {
                            var result = RivetCLI.RunCommand(
                                "sidekick",
                                "--show-terminal",
                                "deploy",
                                "--namespace",
                                gameData.namespaces[selectedIndex].Item1.name_id
                            );
                        }).Start();

                        return;
                    }
                }

                // Settings options
                if (showSettingsOptions)
                {
                    if (GUILayout.Button("Unlink game"))
                    {
                        new System.Threading.Thread(() =>
                    {
                        // First, check if we're already logged in
                        var result = RivetCLI.RunCommand(
                            "unlink");

                        switch (result)
                        {
                            case SuccessResult<JObject> getLinkSuccessResult:
                                if (getLinkSuccessResult.Data["Ok"] == null)
                                {
                                    // RivetPluginBridge.DisplayCliError(result); TODO:
                                    UnityEngine.Debug.LogError("Error: " + result.Data);
                                    return;
                                }

                                break;
                        }

                        window.TransitionToState(new Login());
                    }).Start();
                    };
                }
            }
            finally
            {
                GUILayout.EndVertical();
            }
        }

        /// <summary>
        /// Retrieves the namespace token based on the selected machine and namespace ID.
        /// </summary>
        private void GetNamespaceToken()
        {
            new System.Threading.Thread(() =>
            {
                var command = thisMachineSelected ? "get-namespace-development-token" : "get-namespace-public-token";
                var namespaceId = gameData.namespaces[selectedIndex].Item1.name_id;

                var result = RivetCLI.RunCommand("sidekick", command, "--namespace", namespaceId);

                switch (result)
                {
                    case SuccessResult<JObject> successResult:
                        var token = successResult.Data["Ok"]["token"].ToString();
                        UnityEngine.Debug.Log("Rivet Token: " + token);
                        rivetEditorToken = token;
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            PlayerPrefs.SetString("RIVET_EDITOR_TOKEN", token);
                            Debug.Log("Saved token to PlayerPrefs");
                        };
                        break;
                    case ErrorResult<JObject> errorResult:
                        UnityEngine.Debug.LogError(errorResult.Message);
                        break;
                }

            }).Start();
        }
    }
}