#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using UnityEngine.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum CreateLobbyRequestPublicity
{
    [EnumMember(Value = "public")] Public,
    [EnumMember(Value = "private")] Private,
}

public struct FindLobbyRequest
{
    [JsonProperty("game_modes")] public string[] GameModes;
    [JsonProperty("regions")] public string[]? Regions;
}

public struct JoinLobbyRequest
{
    [JsonProperty("lobby_id")] public string LobbyId;
}

public struct CreateLobbyRequest
{
    [JsonProperty("game_mode")] public string GameMode;
    [JsonProperty("region")] public string? Region;
    [JsonProperty("publicity")] public CreateLobbyRequestPublicity Publicity;
    [JsonProperty("lobby_config")] public JObject? LobbyConfig;
}

public struct FindLobbyResponse
{
    [JsonProperty("lobby")] public RivetLobby Lobby;
    [JsonProperty("ports")] public Dictionary<string, RivetLobbyPort> Ports;
    [JsonProperty("player")] public RivetPlayer Player;
}

public struct RivetLobby
{
    [JsonProperty("lobby_id")] public string LobbyId;
    [JsonProperty("host")] public string Host;
    [JsonProperty("port")] public int Port;
}

public struct RivetLobbyPort
{
    [JsonProperty("hostname")] public string? Hostname;
    [JsonProperty("port")] public ushort Port;
    [JsonProperty("is_tls")] public bool IsTls;
}

public struct RivetPlayer
{
    [JsonProperty("token")] public string Token;
}

[CreateAssetMenu(fileName = "RivetSettings", menuName = "ScriptableObjects/RivetSettings", order = 1)]
public class RivetSettings : ScriptableObject
{
    public string? rivetToken;
}

public class RivetManager : MonoBehaviour
{
    const string MatchmakerApiEndpoint = "https://api.rivet.gg/matchmaker";

    [HideInInspector]
    public string? rivetToken = null;

    /// <summary>
    /// The response from the last <see cref="FindLobby"/> call. Used to maintain information about the Rivet player &
    /// lobby.
    /// </summary>
    public FindLobbyResponse? FindLobbyResponse { get; private set; }

    private void Start()
    {
        Debug.Log("RivetManager.Start");
        // Try to set the Rivet Token from the asset if it exists
        if (rivetToken == null || rivetToken.Length == 0)
        {
            var rivetSettings = Resources.Load<RivetSettings>("RivetSettings");
            if (rivetSettings != null)
            {
                Debug.Log("RivetSettings: " + rivetSettings.rivetToken);
                rivetToken = rivetSettings.rivetToken;
            }
        }
    }

    #region API: Matchmaker.Lobbies

    /// <summary>
    /// <a href="https://rivet.gg/docs/matchmaker/api/lobbies/find">Documentation</a>
    /// </summary>
    /// 
    /// <param name="request"></param>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    /// <returns></returns>
    public IEnumerator FindLobby(FindLobbyRequest request, Action<FindLobbyResponse> success,
        Action<string> fail)
    {
        yield return PostRequest<FindLobbyRequest, FindLobbyResponse>(MatchmakerApiEndpoint + "/lobbies/find",
            request, res =>
            {
                // Save response
                FindLobbyResponse = res;



                success(res);
            }, fail);
    }

    /// <summary>
    /// <a href="https://rivet.gg/docs/matchmaker/api/lobbies/join">Documentation</a>
    /// </summary>
    /// 
    /// <param name="request"></param>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    /// <returns></returns>
    public IEnumerator JoinLobby(JoinLobbyRequest request, Action<FindLobbyResponse> success,
        Action<string> fail)
    {
        yield return PostRequest<JoinLobbyRequest, FindLobbyResponse>(MatchmakerApiEndpoint + "/lobbies/join",
            request, res =>
            {
                // Save response
                FindLobbyResponse = res;

                // // Connect to server
                // var port = res.Ports["default"];
                // Debug.Log("Connecting to " + port.Hostname + ":" + port.Port);
                // _networkManager.ClientManager.StartConnection(port.Hostname, port.Port);

                success(res);
            }, fail);
    }

    /// <summary>
    /// <a href="https://rivet.gg/docs/matchmaker/api/lobbies/create">Documentation</a>
    /// </summary>
    /// 
    /// <param name="request"></param>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    /// <returns></returns>
    public IEnumerator CreateLobby(CreateLobbyRequest request, Action<FindLobbyResponse> success,
        Action<string> fail)
    {
        yield return PostRequest<CreateLobbyRequest, FindLobbyResponse>(MatchmakerApiEndpoint + "/lobbies/create",
            request, res =>
            {
                // Save response
                FindLobbyResponse = res;

                // // Connect to server
                // var port = res.Ports["default"];
                // Debug.Log("Connecting to " + port.Hostname + ":" + port.Port);
                // _networkManager.ClientManager.StartConnection(port.Hostname, port.Port);

                success(res);
            }, fail);
    }

    /// <summary>
    /// <a href="https://rivet.gg/docs/matchmaker/api/lobbies/ready">Documentation</a>
    /// </summary>
    /// 
    /// <param name="success"></param>
    /// <param name="fail"></param>
    /// <returns></returns>
    public IEnumerator LobbyReady(Action success, Action<string> fail)
    {
        yield return PostRequest<Dictionary<string, string>, object>(MatchmakerApiEndpoint + "/lobbies/ready",
            new Dictionary<string, string>(), (_) => success(), fail);
    }

    #endregion

    #region API: Matchmaker.Players

    /// <summary>
    /// <a href="https://rivet.gg/docs/matchmaker/api/players/connected">Documentation</a>
    /// </summary>
    /// 
    /// <param name="playerToken"></param>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    /// <returns></returns>
    public IEnumerator PlayerConnected(string playerToken, Action success, Action<string> fail)
    {
        yield return PostRequest<Dictionary<string, string>, object>(
            MatchmakerApiEndpoint + "/players/connected",
            new Dictionary<string, string>
            {
                { "player_token", playerToken },
            }, (_) => success(), fail);
    }

    /// <summary>
    /// <a href="https://rivet.gg/docs/matchmaker/api/players/disconnected">Documentation</a>
    /// </summary>
    /// 
    /// <param name="playerToken"></param>
    /// <param name="success"></param>
    /// <param name="fail"></param>
    /// <returns></returns>
    public IEnumerator PlayerDisconnected(string playerToken, Action success, Action<string> fail)
    {
        yield return PostRequest<Dictionary<string, string>, object>(
            MatchmakerApiEndpoint + "/players/disconnected", new Dictionary<string, string>
            {
                { "player_token", playerToken },
            }, (_) => success(), fail);
    }

    #endregion

    #region API Requests

    private string GetToken()
    {
        // Check which environment we are in
#if UNITY_SERVER
        Debug.Log("UNITY_SERVER");
        Debug.Log(Environment.GetEnvironmentVariable("RIVET_TOKEN"));
#endif
#if IN_EDITOR
        Debug.Log("IN_EDITOR");
        string value = PlayerPrefs.GetString("RIVET_EDITOR_TOKEN");
        Debug.Log("Editor token: " + value);
#endif

#if UNITY_SERVER
        var token = Environment.GetEnvironmentVariable("RIVET_TOKEN");
        if (token != null)
        {
            return token;
        }
#endif

        // Try loading from PlayerPrefs
        string value = PlayerPrefs.GetString("RIVET_EDITOR_TOKEN");
        if (value.Length > 0)
        {
            return value;
        }

        if (rivetToken != null && rivetToken.Length > 0)
        {
            return rivetToken;
        }

        throw new Exception("RIVET_TOKEN not set");
    }

    public IEnumerator PostRequest<TReq, TRes>(string url, TReq requestBody, Action<TRes> success, Action<string> fail, string token = "")
    {
        Debug.Log("PostRequest: " + url);
        if (token.Length == 0)
        {
            token = GetToken();
        }

        var debugRequestDescription = "POST " + url;

        var requestBodyStr = JsonConvert.SerializeObject(requestBody,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        Debug.Log(debugRequestDescription + " Request: " + requestBodyStr + "\n" + Environment.StackTrace);

        var www = UnityWebRequest.Post(url, requestBodyStr, "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + token);

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("In progress");
                break;
            case UnityWebRequest.Result.Success:
                if (www.responseCode == 200)
                {
                    Debug.Log(debugRequestDescription + " Success: " + www.downloadHandler.text);
                    var responseBody = JsonConvert.DeserializeObject<TRes>(www.downloadHandler.text);
                    success(responseBody!);
                }
                else
                {
                    string statusError = "Error status " + www.responseCode + ": " + www.downloadHandler.text;
                    Debug.LogError(debugRequestDescription + " " + statusError);
                    fail(statusError);
                }

                break;
            case UnityWebRequest.Result.ConnectionError:
                string connectionError = "ConnectionError: " + www.error;
                Debug.LogError(debugRequestDescription + " " + connectionError + "\n" + Environment.StackTrace);
                fail(connectionError);
                break;
            case UnityWebRequest.Result.ProtocolError:
                string protocolError = "ProtocolError: " + www.error + " " + www.downloadHandler.text;
                Debug.LogError(debugRequestDescription + " " + protocolError + "\n" + Environment.StackTrace);
                fail(protocolError);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                string dpe = "DataProcessingError: " + www.error;
                Debug.LogError(debugRequestDescription + " " + dpe + "\n" + Environment.StackTrace);
                fail(dpe);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public IEnumerator GetRequest<TRes>(string url, Action<TRes> success, Action<string> fail, string token = "")
    {
        if (token.Length == 0)
        {
            token = GetToken();
        }

        var debugRequestDescription = "GET " + url;
        Debug.Log(debugRequestDescription + "\n" + Environment.StackTrace);

        var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer " + GetToken());

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("In progress");
                break;
            case UnityWebRequest.Result.Success:
                if (www.responseCode == 200)
                {
                    Debug.Log(debugRequestDescription + " Success: " + www.downloadHandler.text);
                    var responseBody = JsonConvert.DeserializeObject<TRes>(www.downloadHandler.text);
                    success(responseBody!);
                }
                else
                {
                    string statusError = "Error status " + www.responseCode + ": " + www.downloadHandler.text;
                    Debug.LogError(debugRequestDescription + " " + statusError);
                    fail(statusError);
                }

                break;
            case UnityWebRequest.Result.ConnectionError:
                string connectionError = "ConnectionError: " + www.error;
                Debug.LogError(debugRequestDescription + " " + connectionError + "\n" + Environment.StackTrace);
                fail(connectionError);
                break;
            case UnityWebRequest.Result.ProtocolError:
                string protocolError = "ProtocolError: " + www.error + " " + www.downloadHandler.text;
                Debug.LogError(debugRequestDescription + " " + protocolError + "\n" + Environment.StackTrace);
                fail(protocolError);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                string dpe = "DataProcessingError: " + www.error;
                Debug.LogError(debugRequestDescription + " " + dpe + "\n" + Environment.StackTrace);
                fail(dpe);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}