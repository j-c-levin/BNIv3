using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class RaceStartController : MonoBehaviour
{
    public float raceStartHeight;
    public float raceStartCounterSpeed;
    public float raceStartLerpSpeed;
    public float raceStartCounterMax;
    public float raceStartCounterMin;
    public GameObject startArea;
    private MovementController movementController;
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
        movementController.SetRaceRunning(false);
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
        foreach (KeyValuePair<int, GameObject> entry in movementController.Players)
        {
            PlayerMovement(entry);
            if (playersReady[entry.Key] != raceStartCounterMax)
            {
                allReadyForRace = false;
            }
        }
        if (allReadyForRace && movementController.Players.Count > 0)
        {
            StartRace();
        }
    }

    private void StartRace()
    {
        movementController.SetRaceRunning(true);
        foreach (KeyValuePair<int, GameObject> entry in movementController.Players)
        {
            entry.Value.GetComponent<Rigidbody2D>().simulated = true;
			entry.Value.GetComponent<PlayerMovement>().JumpUp();
        }
        startArea.SetActive(false);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>().enabled = true;
        this.enabled = false;
    }

    private void PlayerMovement(KeyValuePair<int, GameObject> entry)
    {
        PlayerMovement player = entry.Value.GetComponent<PlayerMovement>();
        MovementController.PlayerInput input = movementController.GetInputForPlayer(entry.Key);
        bool ready = input.leftButton && input.rightButton;
        if (ready)
        {
            isPlayerReady[entry.Key] = true;
            player.GetComponent<Rigidbody2D>().simulated = false;
        }
        else
        {
            if (input.rightButton)
            {
                isPlayerReady[entry.Key] = false;
                player.JumpRight();
                player.GetComponent<Rigidbody2D>().simulated = true;
            }
            else if (input.leftButton)
            {
                isPlayerReady[entry.Key] = false;
                player.JumpLeft();
                player.GetComponent<Rigidbody2D>().simulated = true;
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
            playersReady[entry.Key] -= raceStartCounterSpeed * Time.deltaTime;
        }
        else
        {
            GameObject player = entry.Value;
            Vector2 startPosition = new Vector2(player.transform.position.x, raceStartHeight);
            player.transform.position = Vector2.Lerp(player.transform.position, startPosition, raceStartLerpSpeed);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.identity, raceStartLerpSpeed);
            playersReady[entry.Key] += raceStartCounterSpeed * Time.deltaTime;
        }
        playersReady[entry.Key] = Mathf.Clamp(playersReady[entry.Key], raceStartCounterMin, raceStartCounterMax);
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
