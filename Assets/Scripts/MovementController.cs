using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class MovementController : MonoBehaviour
{
    private class PlayerInput
    {
        public bool leftButton;
        public bool rightButton;
    }
    private Dictionary<int, GameObject> game;
    private Dictionary<int, PlayerInput> playerInput;

    public void Start()
    {
        game = GetComponent<SpawnController>().players;
        playerInput = new Dictionary<int, PlayerInput>();
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
    }

    public void Update()
    {
        foreach (KeyValuePair<int, GameObject> entry in game)
        {
            PlayerMovement player = entry.Value.GetComponent<PlayerMovement>();
            PlayerInput input = GetInputForPlayer(entry.Key);
            bool drop = input.leftButton && input.rightButton;
            if (drop)
            {
                player.Drop();
            }
            else
            {
                if (input.rightButton)
                {
                    player.JumpRight();
                }
                else if (input.leftButton)
                {
                    player.JumpLeft();
                }
            }
            input.leftButton = false;
            input.rightButton = false;
        }
    }

    private PlayerInput GetInputForPlayer(int playerId)
    {
        PlayerInput playerInput;
        if (this.playerInput.TryGetValue(playerId, out playerInput) == false)
        {
            Debug.LogError("Player id " + playerId + " not registered with movement controller");
        }
        return playerInput;
    }

    private void OnMessage(int playerId, JToken data)
    {
        string name = (string)data["element"];
        if (name == "Null")
        {
            return;
        }
        if (name == "left")
        {
            playerInput[playerId].leftButton = true;
        }
        else
        {
            playerInput[playerId].rightButton = true;
        }
    }

    private void OnConnect(int playerId) {
        PlayerInput newInput = new PlayerInput();
        playerInput.Add(playerId, newInput);
    }
}
