using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_MovementController : MonoBehaviour, IMovementController
{
    public PlayerMovement player;
    private IPowerupController powerupController;
    private bool readyToJump = false;
    MovementController.PlayerInput input;

    void OnEnable()
    {
        powerupController = GetComponent<IPowerupController>();
        if (powerupController == null)
        {
            Debug.LogError("Power up controller not found");
        }
        input = new MovementController.PlayerInput();
    }

    // Update is called once per frame
    void Update()
    {
        bool powerup = Input.GetKey(KeyCode.Space);
        float movement = Input.GetAxisRaw("Horizontal");
        if (readyToJump && movement == 0 && powerup == false)
        {
            // not moving, do nothing
            return;
        }
        if (readyToJump == false && movement == 0 && powerup == false)
        {
            readyToJump = true;
            return;
        }
        if (readyToJump == false)
        {
            input.leftButton = false;
            input.rightButton = false;
            return;
        }
        if (powerup && powerupController.HasPowerup(0))
        {
            input.leftButton = true;
            input.rightButton = true;
            powerupController.UsePowerup(0);
        }
        if (movement > 0)
        {
            input.rightButton = true;
            player.JumpRight();
        }
        else if (movement < 0)
        {
            input.leftButton = true;
            player.JumpLeft();
        }
        readyToJump = false;
    }

    public MovementController.PlayerInput GetInputForPlayer(int playerId)
    {
        return input;
    }
}
