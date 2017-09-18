using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_MovementController : MonoBehaviour, IMovementController
{
    private test_SpawnController spawnController;
    private IPowerupController powerupController;
    private bool readyToJump = false;
    MovementController.PlayerInput input;

    public void Start()
    {
        spawnController = GetComponent<test_SpawnController>();
    }

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
        if (movement > 0)
        {
            input.rightButton = true;
            spawnController.playerMovement.JumpRight();
        }
        else if (movement < 0)
        {
            input.leftButton = true;
            spawnController.playerMovement.JumpLeft();
        }
        readyToJump = false;
    }

    public MovementController.PlayerInput GetInputForPlayer(int playerId)
    {
        return input;
    }
}
