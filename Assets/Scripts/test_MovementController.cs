using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_MovementController : MonoBehaviour
{
    public PlayerMovement player;
    private IPowerupController powerupController;
    private bool readyToJump = true;

    void Start()
    {
        powerupController = GetComponent<IPowerupController>();
        if (powerupController == null)
        {
            Debug.LogError("Power up controller not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool drop = Input.GetKey(KeyCode.Space);
        float movement = Input.GetAxisRaw("Horizontal");
        if (readyToJump && movement == 0 && drop == false)
        {
            // not moving, do nothing
            return;
        }
        if (readyToJump == false && movement == 0 && drop == false)
        {
            readyToJump = true;
            return;
        }
        if (readyToJump == false)
        {
            return;
        }
        if (drop && powerupController.HasPowerup(0))
        {
            powerupController.UsePowerup(0);
        }
        else if (drop)
        {
            player.Drop();
        }
        if (movement > 0)
        {
            player.JumpRight();
        }
        else if (movement < 0)
        {
            player.JumpLeft();
        }
        readyToJump = false;
    }
}
