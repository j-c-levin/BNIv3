using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_MovementController : MonoBehaviour
{
    public PlayerMovement player;
    private bool readyToJump = true;

    // Update is called once per frame
    void Update()
    {
        bool drop = Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow);
        if (drop && readyToJump)
        {
            player.Drop();
            readyToJump = false;
            return;
        }
        float movement = Input.GetAxisRaw("Horizontal");
        if (movement > 0 && readyToJump)
        {
            player.JumpRight();
            readyToJump = false;
        }
        else if (movement < 0 && readyToJump)
        {
            player.JumpLeft();
            readyToJump = false;
        }
        else if (movement == 0 && drop == false)
        {
            readyToJump = true;
        }
    }
}
