using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStartInput : InputCommand
{
    private PlayerMovement playerMovement;
    private float raceStartHeight = 0f;
    private float neutralForce = 10f;
    private float rotationForce = 10f;
    public override void Execute(PlayerDetail player)
    {
        playerMovement = player.body.GetComponent<PlayerMovement>();
        bool ready = player.input.leftButton && player.input.rightButton;
        if (ready)
        {
            Hover();
            player.observer(player, PlayerDetail.PlayerEvent.IsReadyForRace);
        }
        else
        {
            if (player.input.rightButton)
            {
                playerMovement.JumpRight();
            }
            else if (player.input.leftButton)
            {
                playerMovement.JumpLeft();
            }
            player.input.leftButton = player.input.rightButton = false;
        }
    }

    private void Hover()
    {
        Transform player = playerMovement.gameObject.transform;
        float distanceFromStartHeight = raceStartHeight - player.position.y;
        float upwardForce = neutralForce * ((distanceFromStartHeight > 0) ? 1 : 0f);
        playerMovement.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upwardForce);
        float rotationFromNeutral = (player.rotation.eulerAngles.z <= 180) ? -player.rotation.eulerAngles.z : 360 - player.rotation.eulerAngles.z;
        float torqueForce = rotationForce * rotationFromNeutral;
        playerMovement.GetComponent<Rigidbody2D>().AddTorque(torqueForce, ForceMode2D.Force);
    }
}
