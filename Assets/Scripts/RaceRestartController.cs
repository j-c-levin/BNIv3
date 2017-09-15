using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceRestartController : MonoBehaviour
{
    public void EndOfRace()
    {
        GetComponent<MovementController>().isRaceRunning = false;
        string finalScoresText = "";
        foreach (KeyValuePair<int, int> entry in GetComponent<ScoreController>().scores)
        {
            finalScoresText += "Player ";
            finalScoresText += entry.Key;
            finalScoresText += " : ";
            finalScoresText += entry.Value;
            finalScoresText += "\n";
        }
        Debug.Log(finalScoresText);
        RestartRace();
    }

    public void RestartRace()
    {
        // Reset the starting area
        GetComponent<RaceStartController>().ResetRace();
        // Reset scores
        GetComponent<ScoreController>().ResetRace();
        // Reset camera
        GetComponent<CameraController>().ResetRace();
        // Reset players
        GetComponent<MovementController>().ResetRace();
        // Reset the players and collectibles
        GetComponent<SpawnController>().ResetRace();
    }
}
