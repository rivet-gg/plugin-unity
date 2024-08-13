using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend;
using Backend.Modules.Lobbies;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public async void OnFindLobby()
    {
        var client = new BackendClient();
        var response = await client.Lobbies.FindOrCreate(new Backend.Model.Lobbies.FindOrCreateRequest(
            varVersion: "default",
            regions: new List<string> { "local" },
            tags: new Dictionary<string, string> { },
            players: new List<object> { new() },
            createConfig: new Backend.Model.Lobbies.FindOrCreateRequestCreateConfig(
                region: "local",
                tags: new Dictionary<string, string> { },
                maxPlayers: 32,
                maxPlayersDirect: 32
            )
        ));
        BackendMultiplayerManager.Instance.ConnectToLobby(response.Lobby, response.Players[0]);
    }
}
