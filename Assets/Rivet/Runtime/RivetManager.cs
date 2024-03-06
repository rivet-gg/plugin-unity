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
    public string? RivetToken;
    public string? ApiEndpoint;
}

public class RivetManager : MonoBehaviour
{
    [HideInInspector]
    public string? RivetToken = null;

    [HideInInspector]
    public string? ApiEndpoint = null;

    [HideInInspector]
    public string? MatchmakerApiEndpoint => ApiEndpoint + "/matchmaker";

    /// <summary>
    /// The response from the last <see cref="FindLobby"/> call. Used to maintain information about the Rivet player &
    /// lobby.
    /// </summary>
    public FindLobbyResponse? FindLobbyResponse { get; private set; }

    private void Start()
    {
        // Try to load Rivet runtime settings
        var rivetSettings = Resources.Load<RivetSettings>("RivetSettings");
        if (rivetSettings != null)
        {
            RivetToken = rivetSettings.RivetToken;
            ApiEndpoint = rivetSettings.ApiEndpoint;
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
        // Try loading from environment
        var token = Environment.GetEnvironmentVariable("RIVET_TOKEN");
        if (token != null && token.Length > 0)
        {
            return token;
        }

        // Try loading from PlayerPrefs
        string value = PlayerPrefs.GetString("RIVET_EDITOR_TOKEN");
        if (value.Length > 0)
        {
            return value;
        }

        // Try loading from RivetSettings
        if (RivetToken != null && RivetToken.Length > 0)
        {
            return RivetToken;
        }

        throw new Exception("RIVET_TOKEN not set");
    }

    public IEnumerator PostRequest<TReq, TRes>(string url, TReq requestBody, Action<TRes> success, Action<string> fail, string token = "")
    {
        if (token.Length == 0)
        {
            token = GetToken();
        }

        var debugRequestDescription = "POST " + url;

        var requestBodyStr = JsonConvert.SerializeObject(requestBody,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        var www = UnityWebRequest.Post(url, requestBodyStr, "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + token);

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                if (www.responseCode == 200)
                {
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

        var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Authorization", "Bearer " + GetToken());

        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                if (www.responseCode == 200)
                {
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