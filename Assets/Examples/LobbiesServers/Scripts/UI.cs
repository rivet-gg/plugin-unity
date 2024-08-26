#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Backend;
using Backend.Modules.Lobbies;
using FishNet.Managing;
using FishNet.Transporting;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private NetworkManager _networkManager = null!;

    public GameObject joinMenuPanel = null!;
    public TMP_Text connectionInfoText = null!;
    public TMP_InputField lobbyIdInputField = null!;
    public Slider moveSpeedSlider = null!;

    private Backend.Model.Lobbies.CreateResponseLobby? _lobby;
    private Backend.Model.Lobbies.CreateResponsePlayersInner? _player;

    private LocalConnectionState? _connectionState;

    private BackendClient TEMPBackendClient()
    {
        var config = new Configuration();
        Debug.Log($"Backend endpoint: {config.BackendEndpoint}");
        return new BackendClient(config.BackendEndpoint);
    }

    private string TEMPGameVersion()
    {
        // var config = new Configuration();
        // return config.GameVersion;
        return "default";
    }

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();

        _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        _networkManager.ClientManager.OnAuthenticated += UpdateConnectionInfo;
        _networkManager.ClientManager.OnRemoteConnectionState += (_) => UpdateConnectionInfo();

        UpdateConnectionInfo();
    }

    private void OnDestroy()
    {
        _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
    }

    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        _connectionState = obj.ConnectionState;
        UpdateConnectionInfo();
    }

    public async void OnClick_Find()
    {
        // Hide menu
        joinMenuPanel.SetActive(false);

        var response = await TEMPBackendClient().Lobbies.FindOrCreate(new Backend.Model.Lobbies.FindOrCreateRequest(
            varVersion: TEMPGameVersion(),
            regions: new List<string> { "local" },
            tags: new Dictionary<string, string>
            {
                // ["gameMode"] = gameMode,
            },
            players: new List<object> { new() },
            createConfig: new Backend.Model.Lobbies.FindOrCreateRequestCreateConfig(
                region: "local",
                tags: new Dictionary<string, string> { },
                maxPlayers: 32,
                maxPlayersDirect: 32
            )
        ));

        _lobby = response.Lobby;
        _player = response.Players[0];
        UpdateConnectionInfo();

        BackendMultiplayerManager.Instance.ConnectToLobby(response.Lobby, response.Players[0]);
    }

    public void OnClick_Join()
    {
        // TODO:
        // // Hide menu
        // joinMenuPanel.SetActive(false);

        // // Find lobby
        // StartCoroutine(_multiplayerManager.JoinLobby(new JoinLobbyRequest
        // {
        //     LobbyId = lobbyIdInputField.text,
        // }, _ => UpdateConnectionInfo(), fail => { Debug.Log($"Failed to join lobby: {fail}"); }));
    }

    public void OnClick_Create()
    {
        // TODO:
        // // Hide menu
        // joinMenuPanel.SetActive(false);

        // // Find lobby
        // StartCoroutine(_multiplayerManager.CreateLobby(new CreateLobbyRequest
        // {
        //     GameMode = "custom",
        //     LobbyConfig = new JObject
        //     {
        //         { "move_speed", moveSpeedSlider.value }
        //     },
        // }, _ => UpdateConnectionInfo(), fail => { Debug.Log($"Failed to create lobby: {fail}"); }));
    }

    public void OnClick_CopyLobbyId()
    {
        GUIUtility.systemCopyBuffer = _lobby?.Id ?? "";
    }

    public void UpdateConnectionInfo()
    {
        // Choose connection state text
        string connectionState;
        switch (_connectionState)
        {
            case null:
                connectionState = "?";
                break;
            case LocalConnectionState.Stopped:
                connectionState = "Stopped";
                break;
            case LocalConnectionState.Started:
                connectionState = "Started";
                break;
            case LocalConnectionState.Starting:
                connectionState = "Starting";
                break;
            default:
                connectionState = "Unknown";
                break;
        }

        // Update UI
        BackendMultiplayerManager.ConnectionConfig? connConfig = null;
        var lobbyTags = "?";
        var lobbyCreate = "?";
        if (_lobby is { } lobby)
        {
            connConfig = BackendMultiplayerManager.Instance.GetConnectionConfig(lobby, _player);
            lobbyTags = string.Join(", ", lobby.Tags.Select(kv => $"{kv.Key}: {kv.Value}"));
            lobbyCreate = DateTimeOffset.FromUnixTimeMilliseconds((long)lobby.CreatedAt).ToString("yyyy-MM-dd HH:mm:ss");
        }
        connectionInfoText.text =
            $"Lobby ID: {_lobby?.Id ?? "?"}\n" +
            $"Lobby Tags: {lobbyTags}\n" +
            $"Lobby Created: {lobbyCreate}\n" +
            $"Hostname: {connConfig?.Hostname ?? "?"}\n" +
            $"Port: {connConfig?.Port.ToString() ?? "?"}\n" +
            $"Connection state: {connectionState}\n\n";
    }
}