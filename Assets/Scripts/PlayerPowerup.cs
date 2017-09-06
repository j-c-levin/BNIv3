using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    public float jupiterJumpGravity;
    public float jupiterJupDuration;
    private IPowerupController powerupController;
    private int powerupLayer = 9;
    private int playerId;
    public void Start()
    {
        powerupController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IPowerupController>();
        if (powerupController == null)
        {
            Debug.LogError("Powerup controller not found");
        }
        playerId = GetComponent<PlayerScore>().playerId;
    }

    public void JupiterJump(int castingPlayerId)
    {
        if (playerId != castingPlayerId)
        {
            StartCoroutine("JupiterJumpRoutine");
        }
    }

    private IEnumerator JupiterJumpRoutine()
    {
        Rigidbody2D player = GetComponent<Rigidbody2D>();
        float originalGravity = player.gravityScale;
        player.gravityScale = jupiterJumpGravity;
        yield return new WaitForSeconds(jupiterJupDuration);
        player.gravityScale = originalGravity;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == powerupLayer)
        {
            int playerId = GetComponent<PlayerScore>().playerId;
            powerupController.CollectedPowerup(playerId, collider);
        }
    }
}
