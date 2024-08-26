using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using UnityEngine;
using FishNet;
using FishNet.Managing;
using FishNet.Managing.Transporting;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using Backend;
using Backend.Model.Lobbies;
using FishNet.Connection;
using FishNet.Managing.Server;
using System.Reflection.Emit;

namespace Backend.Modules.Lobbies
{
    public enum TransportKind { Tugboat }


    public class BackendMultiplayerManager : MonoBehaviour
    {
        public struct ConnectionConfig
        {
            public string Hostname;
            public ushort Port;
            public bool IsTls;
        }

        public static BackendMultiplayerManager Instance { get; private set; }

        // User Settings
        public TransportKind transportKind = TransportKind.Tugboat;
        public string portName = "game";

        // Events
        public event Action ServerConnected;
        public event Action ServerDisconnected;
        public event Action<int> ClientConnected;
        public event Action<int> ClientDisconnected;

        // State
        public bool IsServer { get; private set; }
        private NetworkManager _networkManager;
        private TransportManager _transportManager;
        private Transport _transport;
        private bool _multiplayerSetup = false;

        // Server-only variables
        private string _serverHostname = "127.0.0.1";
        private ushort _serverPort = 7777;
        private string _lobbyId = "00000000-0000-0000-0000-000000000000";
        private string _lobbyToken;
        private Dictionary<int, string> _playerTokens = new Dictionary<int, string>();

        // Client-only variables
        private string _playerToken;

        private BackendClient _backendClient;

        private void Awake()
        {

            // Setup singleton
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Already has singleton");
                Destroy(gameObject);
                return;
            }
            Instance = this;

            DontDestroyOnLoad(gameObject);

            // Create client
            var config = new Configuration();
            Debug.Log($"Backend endpoint: {config.BackendEndpoint}");
            _backendClient = new BackendClient(config.BackendEndpoint);

            // Setup FishNet
            _networkManager = InstanceFinder.NetworkManager;
            if (_networkManager == null)
            {
                Debug.LogError("NetworkManager not found in the scene.");
                return;
            }

            _transportManager = _networkManager.TransportManager;

            switch (transportKind)
            {
                case TransportKind.Tugboat:
                    _transport = _transportManager.GetTransport<Tugboat>();
                    break;
                default:
                    throw new Exception("unreachable");
            }

            if (_transport == null)
            {
                Debug.LogError($"Transport of type {transportKind} not found.");
                return;
            }
        }

        private void Start()
        {
            Debug.Log("Starting multiplayer");
            SetupMultiplayer();
        }

        public void SetupMultiplayer()
        {
            if (_multiplayerSetup)
            {
                Debug.LogWarning("SetupMultiplayer already called");
                return;
            }
            _multiplayerSetup = true;

            IsServer = Array.IndexOf(Environment.GetCommandLineArgs(), "-server") != -1;

            // Setup events
            _networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
            _networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
            _networkManager.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;


            // Setup authentication (You'll need to implement this part)
            // _networkManager.ServerManager.AuthorizeConnection += AuthorizeConnection;
            // _networkManager.ClientManager.OnAuthenticated += OnClientAuthenticated;

            if (IsServer)
            {
                _serverHostname = Environment.GetEnvironmentVariable("SERVER_HOSTNAME") ?? _serverHostname;
                _serverPort = ushort.TryParse(Environment.GetEnvironmentVariable("SERVER_PORT"), out ushort port) ? port : _serverPort;
                _lobbyId = Environment.GetEnvironmentVariable("LOBBY_ID") ?? _lobbyId;
                _lobbyToken = Environment.GetEnvironmentVariable("LOBBY_TOKEN");

                StartServer();
            }
        }

        private void StartServer()
        {
            Debug.Log($"Starting server [transport={transportKind} host={_serverHostname}:{_serverPort} lobbyId={_lobbyId}]");

            _transport.SetServerBindAddress(_serverHostname, IPAddressType.IPv4);
            _transport.SetPort(_serverPort);
            _networkManager.ServerManager.StartConnection();

            _ = NotifyLobbyReady();
        }

        private async Task NotifyLobbyReady()
        {
            var request = new SetLobbyReadyRequest
            {
                LobbyId = _lobbyId,
                LobbyToken = _lobbyToken
            };

            // TODO: try instead of this
            try
            {
                var response = await _backendClient.Lobbies.SetLobbyReady(request);
                Debug.Log("Lobby ready");
            }
            catch (Exception error)
            {
                Debug.LogError($"Lobby ready failed: {error}");
                Application.Quit(1);
            }
        }

        public ConnectionConfig? GetConnectionConfig(Model.Lobbies.CreateResponseLobby lobbyResponse, Model.Lobbies.CreateResponsePlayersInner player)
        {
            // TODO: Validate compatibility with transport

            if (lobbyResponse.Backend != null)
            {
                if (lobbyResponse.Backend.Server != null)
                {
                    // Server backend
                    if (lobbyResponse.Backend.Server.Ports.TryGetValue(portName, out var serverPort))
                    {
                        return new ConnectionConfig
                        {
                            Hostname = serverPort.PublicHostname,
                            Port = (ushort)serverPort.PublicPort,
                            IsTls = serverPort.Protocol == CreateResponseLobbyBackendServerPortsValue.ProtocolEnum.Https || serverPort.Protocol == CreateResponseLobbyBackendServerPortsValue.ProtocolEnum.TcpTls,
                        };
                    }
                    else
                    {
                        Debug.LogError($"No port with name {portName}");
                        return null;
                    }
                }
                else if (lobbyResponse.Backend.LocalDevelopment != null)
                {
                    // Local development backend
                    if (lobbyResponse.Backend.LocalDevelopment.Ports.TryGetValue(portName, out var localPort))
                    {
                        return new ConnectionConfig
                        {
                            Hostname = localPort.Hostname,
                            Port = (ushort)localPort.Port,
                            IsTls = false,
                        };
                    }
                    else
                    {
                        Debug.LogError($"No port with name {portName}");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError("Unsupported lobby backend type");
                    return null;
                }
            }
            else
            {
                Debug.LogError("No backend information found in lobby response");
                return null;
            }
        }

        public void ConnectToLobby(Model.Lobbies.CreateResponseLobby lobbyResponse, Model.Lobbies.CreateResponsePlayersInner player)
        {
            if (!_multiplayerSetup)
            {
                Debug.LogError("SetupMultiplayer needs to be called in Start");
                return;
            }
            if (IsServer)
            {
                Debug.LogWarning("Cannot call ConnectToLobby on server");
                return;
            }

            Debug.Log($"Connecting to lobby: {lobbyResponse.Id}");

            _playerToken = player.Token;

            var connConfigOpt = GetConnectionConfig(lobbyResponse, player);
            if (connConfigOpt == null)
            {
                Debug.LogError("Could not get connection config");
                return;
            }
            var connConfig = connConfigOpt.Value;

            Debug.Log($"Connecting to {connConfig.Hostname}:{connConfig.Port} (TLS: {connConfig.IsTls})");

            _transport.SetClientAddress(connConfig.Hostname);
            _transport.SetPort(connConfig.Port);

            // TODO: Enable TLS for WS

            _networkManager.ClientManager.StartConnection();
        }

        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            Debug.Log($"Client connection state: {args.ConnectionState}");
            switch (args.ConnectionState)
            {
                case LocalConnectionState.Started:
                    Debug.Log("Connected to server");
                    ServerConnected?.Invoke();
                    break;
                case LocalConnectionState.Stopped:
                    Debug.Log("Disconnected from server");
                    ServerDisconnected?.Invoke();
                    break;
            }
        }

        private void OnServerConnectionState(ServerConnectionStateArgs args)
        {
            if (args.ConnectionState == LocalConnectionState.Started)
            {
                Debug.Log("Server started");
            }
            else if (args.ConnectionState == LocalConnectionState.Stopped)
            {
                Debug.Log("Server stopped");
            }
        }

        private void OnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == RemoteConnectionState.Started)
            {
                Debug.Log($"Client connected: {conn.ClientId}");
                ClientConnected?.Invoke(conn.ClientId);
                if (IsServer)
                {
                    _ = AuthenticateClient(conn.ClientId);
                }
            }
            else if (args.ConnectionState == RemoteConnectionState.Stopped)
            {
                Debug.Log($"Client disconnected: {conn.ClientId}");
                ClientDisconnected?.Invoke(conn.ClientId);
                if (IsServer)
                {
                    _ = HandleClientDisconnect(conn.ClientId);
                }
            }
        }

        private async Task AuthenticateClient(int clientId)

        {
            // In a real implementation, you would receive the player token from the client
            // and validate it with the backend. For this example, we'll use a dummy token.
            string playerToken = $"dummy_token_{clientId}";
            _playerTokens[clientId] = playerToken;

            var request = new SetPlayerConnectedRequest
            {
                LobbyId = _lobbyId,
                LobbyToken = _lobbyToken,
                PlayerTokens = new List<string> { playerToken }
            };

            try
            {
                var response = await _backendClient.Lobbies.SetPlayerConnected(request);
                Debug.Log($"Player authenticated for {clientId}");

                // TODO: is this right?
                // In FishNet, you don't need to explicitly complete authentication
                // The client is considered authenticated once connected
            }
            catch (Exception error)
            {
                Debug.LogWarning($"Player authentication failed for {clientId}: {error}");
                if (_networkManager.ServerManager.Clients[clientId] is { } value)
                {
                    Debug.LogWarning($"Cannot kick {clientId}, client is missing");
                    value.Kick(KickReason.UnusualActivity);
                }
            }
        }

        private async Task HandleClientDisconnect(int clientId)

        {
            if (_playerTokens.TryGetValue(clientId, out string playerToken))
            {
                _playerTokens.Remove(clientId);
                Debug.Log($"Removing player {playerToken}");

                var request = new SetPlayerDisconnectedRequest
                {
                    LobbyId = _lobbyId,
                    LobbyToken = _lobbyToken,
                    PlayerTokens = new List<string> { playerToken }
                };

                try
                {
                    var response = await _backendClient.Lobbies.SetPlayerDisconnected(request);
                    Debug.LogWarning($"Player disconnected without player token: {clientId}");
                }
                catch (Exception error)
                {
                    Debug.LogWarning($"Player disconnect failed for {clientId}: {error}");
                }
            }
        }
    }
}