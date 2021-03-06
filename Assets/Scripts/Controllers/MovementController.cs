﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class MovementController : MonoBehaviour, IMovementController
{
    public class PlayerInput
    {
        public bool leftButton;
        public bool rightButton;
    }
    public bool isRaceRunning = false;
    private SpawnController spawnController;
    private Dictionary<int, PlayerInput> playerInput;

    public void Start()
    {
        spawnController = GetComponent<SpawnController>();
        playerInput = new Dictionary<int, PlayerInput>();
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    public void Update()
    {
        if (isRaceRunning == false)
        {
            return;
        }
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            PlayerMovement player = entry.Value.GetComponentInChildren<PlayerMovement>();
            PlayerInput input = GetInputForPlayer(entry.Key);
            if (input.rightButton)
            {
                player.JumpRight();
            }
            else if (input.leftButton)
            {
                player.JumpLeft();
            }
            input.leftButton = false;
            input.rightButton = false;
        }
    }

    public PlayerInput GetInputForPlayer(int playerId)
    {
        PlayerInput playerInput;
        if (this.playerInput.TryGetValue(playerId, out playerInput) == false)
        {
            Debug.LogError("Player id " + playerId + " not registered with movement controller");
        }
        return playerInput;
    }

    public void ResetRace()
    {
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            entry.Value.transform.position = Vector3.zero;
        }
    }

    private void OnMessage(int playerId, JToken data)
    {
        if (data["handshake"] != null)
        {
            return;
        }
        string direction = (string)data["element"];
        if (direction == "left")
        {
            playerInput[playerId].leftButton = (bool)data["data"]["pressed"];
        }
        else if (direction == "right")
        {
            playerInput[playerId].rightButton = (bool)data["data"]["pressed"];
        }
    }

    private void OnConnect(int playerId)
    {
        PlayerInput newInput = new PlayerInput();
        playerInput.Add(playerId, newInput);
    }

    private void OnDisconnect(int playerId)
    {
        playerInput.Remove(playerId);
    }
}
