using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    public float jupiterJumpGravity;
    public float jupiterJupDuration;
    private IPowerupController scoreController;
    private int powerupLayer = 9;
    private int playerId;
    public void Start()
    {
        scoreController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IPowerupController>();
        if (scoreController == null)
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
            scoreController.CollectedPowerup(playerId, collider);
        }
    }
}
