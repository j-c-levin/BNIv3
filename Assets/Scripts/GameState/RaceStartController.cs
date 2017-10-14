using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class RaceStartController : MonoBehaviour
{
    public delegate void Observer(GameState.RaceEvent raceEvent);
    public Observer observer;
    private Dictionary<int, bool> playerReadyState;
    private float readyUpTime = 1.5f;
    private float startTime;

    public void Start()
    {
        playerReadyState = new Dictionary<int, bool>();
        GetComponent<InputController>().observer += EventObserver;
    }

    public void Update()
    {
        if (AllPlayersReady() && Time.time - readyUpTime > startTime)
        {
            // notify event to start race and then reset the timer
            if (once == false)
            {
                observer(GameState.RaceEvent.ReadyToRace);
                once = true;
            }
        }
    }
    bool once = false;

    private void EventObserver(PlayerDetail player, PlayerDetail.PlayerEvent playerEvent)
    {
        switch (playerEvent)
        {
            case PlayerDetail.PlayerEvent.NewPlayer:
                AddPlayer(player);
                break;
            case PlayerDetail.PlayerEvent.IsReadyForRace:
                ReadyPlayer(player);
                break;
        }
    }

    private void AddPlayer(PlayerDetail newPlayer)
    {
        playerReadyState.Add(newPlayer.playerId, false);
        newPlayer.observer += EventObserver;
    }

    private void ReadyPlayer(PlayerDetail player)
    {
        if (playerReadyState[player.playerId] == true)
        {
            return;
        }
        playerReadyState[player.playerId] = true;
        if (AllPlayersReady())
        {
            startTime = Time.time;
        }
    }

    private bool AllPlayersReady()
    {
        bool response = false;
        foreach (KeyValuePair<int, bool> entry in playerReadyState)
        {
            if (entry.Value == false)
            {
                return response;
            }
        }
        return true;
    }

    private void OnDisconnect(int playerId)
    {
        playerReadyState.Remove(playerId);
    }
}
