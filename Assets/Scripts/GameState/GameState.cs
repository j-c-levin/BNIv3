using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum RaceEvent
    {
        ReadyToRace
    }
    private enum State
    {
        WaitingToStart,
        Racing
    }
    private State state;

    public void Start()
    {
        GetComponent<RaceStartController>().observer = ObserverCallback;
    }

    private void ObserverCallback(RaceEvent raceEvent)
    {
        Debug.Log(raceEvent);
    }
}
