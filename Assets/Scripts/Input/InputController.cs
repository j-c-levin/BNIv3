using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class InputController : MonoBehaviour
{
    public delegate void Observer(PlayerDetail player, PlayerDetail.PlayerEvent playerEvent);
    public Observer observer;
    private Dictionary<int, PlayerDetail> playerDictionary;

    public void Start()
    {
        playerDictionary = new Dictionary<int, PlayerDetail>();
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    public void Update()
    {
        foreach (KeyValuePair<int, PlayerDetail> entry in playerDictionary)
        {
            entry.Value.Update();
        }
    }

    public void SetInputCommandForPlayer(int playerId, InputCommand newCommand)
    {
        PlayerDetail player;
        if (playerDictionary.TryGetValue(playerId, out player) == false)
        {
            Debug.LogError("Player id " + playerId + " not found in player dictionary");
        }
        playerDictionary[playerId].SetInputCommand(newCommand);
    }

    public void AddPlayer(int playerId, GameObject player)
    {
        PlayerDetail newPlayer = new PlayerDetail(playerId, player, new RaceStartInput());
        playerDictionary.Add(playerId, newPlayer);
        if (observer != null)
        {
            observer(newPlayer, PlayerDetail.PlayerEvent.NewPlayer);
        }
    }

    public void OnMessage(int playerId, JToken data)
    {
        if (data["handshake"] != null)
        {
            return;
        }
        string direction = (string)data["element"];
        PlayerDetail.PlayerInput newInput = playerDictionary[playerId].input;
        if (direction == "left")
        {
            newInput.leftButton = (bool)data["data"]["pressed"];
        }
        else if (direction == "right")
        {
            newInput.rightButton = (bool)data["data"]["pressed"];
        }
        playerDictionary[playerId].SetInput(newInput);
    }

    private void OnDisconnect(int playerId)
    {
        playerDictionary.Remove(playerId);
    }
}
