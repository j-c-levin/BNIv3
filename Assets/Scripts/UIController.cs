using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button restartButton;
    
    public void EndOfRace()
    {
        restartButton.gameObject.SetActive(true);
    }

    public void RestartRace()
    {
        // Deactivate the button
        restartButton.gameObject.SetActive(false);
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
