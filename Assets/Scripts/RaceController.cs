using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceController : MonoBehaviour
{
    public GameObject endOfRace;
    public Text scoreText;

    public void Start()
    {
        endOfRace.gameObject.SetActive(false);
    }

    public void EndOfRace()
    {
        endOfRace.gameObject.SetActive(true);
        string finalScoresText = "";
        foreach (KeyValuePair<int, int> entry in GetComponent<ScoreController>().scores)
        {
            finalScoresText += "Player ";
            finalScoresText += entry.Key;
            finalScoresText += " : ";
            finalScoresText += entry.Value;
            finalScoresText += "\n";
        }
        scoreText.text = finalScoresText;
    }

    public void RestartRace()
    {
        // Deactivate the button
        endOfRace.gameObject.SetActive(false);
        // Reset the collectables
        GetComponent<SpawnController>().ResetRace();
        // Reset scores
        GetComponent<ScoreController>().ResetRace();
        // Reset camera
        GetComponent<CameraController>().ResetRace();
        // Reset players
        GetComponent<MovementController>().ResetRace();
    }
}
