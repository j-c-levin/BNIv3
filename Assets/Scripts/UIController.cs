using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
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
        // Move camera and players back to the start
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cam.transform.position = new Vector3(0, 0, cam.transform.position.z);
        player.transform.position = Vector3.zero;
        // Give the player a starting boost
        player.GetComponent<PlayerMovement>().JumpUp();
        // Start the camera again
        GetComponent<CameraController>().Start();
    }
}
