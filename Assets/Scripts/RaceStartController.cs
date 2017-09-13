﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class RaceStartController : MonoBehaviour
{
    public float raceStartHeight;
    public float readyUpTimerDuration;
    public GameObject startArea;
    public float neutralForce;
    public float rotationForce;
    private MovementController movementController;
    private SpawnController spawnController;
    private Dictionary<int, float> playersReady;
    private Dictionary<int, bool> isPlayerReady;
    private bool isRaceRunning = false;

    void Start()
    {
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        movementController = GetComponent<MovementController>();
        if (movementController == null)
        {
            Debug.LogError("Movement controller not attached");
        }
        spawnController = GetComponent<SpawnController>();
        if (spawnController == null)
        {
            Debug.LogError("Spawn controller not attached");
        }
        movementController.SetRaceRunning(false);
        Debug.Log("Race NOT running");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().enabled = false;
        playersReady = new Dictionary<int, float>();
        isPlayerReady = new Dictionary<int, bool>();
    }

    public void Update()
    {
        if (isRaceRunning)
        {
            return;
        }
        bool allReadyForRace = true;
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            PlayerMovement(entry);
            if (playersReady[entry.Key] != readyUpTimerDuration)
            {
                allReadyForRace = false;
            }
        }
        if (allReadyForRace && spawnController.players.Count > 0)
        {
            Debug.Log("all ready for race");
            StartRace();
        }
    }

    private void StartRace()
    {
        movementController.SetRaceRunning(true);
        Debug.Log("Race running");
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            entry.Value.GetComponentInChildren<PlayerMovement>().JumpUp();
        }
        startArea.SetActive(false);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().enabled = true;
        this.enabled = false;
    }

    private void PlayerMovement(KeyValuePair<int, GameObject> entry)
    {
        PlayerMovement player = entry.Value.GetComponentInChildren<PlayerMovement>();
        MovementController.PlayerInput input = movementController.GetInputForPlayer(entry.Key);
        bool ready = input.leftButton && input.rightButton;
        if (ready)
        {
            isPlayerReady[entry.Key] = true;
        }
        else
        {
            if (input.rightButton)
            {
                isPlayerReady[entry.Key] = false;
                player.JumpRight();
            }
            else if (input.leftButton)
            {
                isPlayerReady[entry.Key] = false;
                player.JumpLeft();
            }
        }
        playerReadyUp(entry);
        input.leftButton = false;
        input.rightButton = false;
    }

    private void playerReadyUp(KeyValuePair<int, GameObject> entry)
    {
        if (isPlayerReady[entry.Key] == false)
        {
            playersReady[entry.Key] -= Time.deltaTime;
        }
        else
        {
            GameObject player = entry.Value.GetComponentInChildren<PlayerMovement>().gameObject;
            float distanceFromStartHeight = raceStartHeight - player.transform.position.y;
            float upwardForce = neutralForce * ((distanceFromStartHeight > 0) ? 1 : 0f);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upwardForce);
            float rotationFromNeutral = (player.transform.rotation.eulerAngles.z <= 180) ? -player.transform.rotation.eulerAngles.z : 360 - player.transform.rotation.eulerAngles.z;
            float torqueForce = rotationForce * rotationFromNeutral;
            player.GetComponent<Rigidbody2D>().AddTorque(torqueForce, ForceMode2D.Force);
            playersReady[entry.Key] += Time.deltaTime;
        }
        playersReady[entry.Key] = Mathf.Clamp(playersReady[entry.Key], 0, readyUpTimerDuration);
        Debug.Log(playersReady[entry.Key]);
    }


    private void OnConnect(int playerId)
    {
        playersReady.Add(playerId, 0);
        isPlayerReady.Add(playerId, false);
    }

    private void OnDisconnect(int playerId)
    {
        playersReady.Remove(playerId);
        isPlayerReady.Remove(playerId);
    }
}
