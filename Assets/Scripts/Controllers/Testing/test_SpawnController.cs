using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_SpawnController : MonoBehaviour, ISpawnController
{
    public GameObject playerPrefab;
    public GameObject player;
    public PlayerMovement playerMovement;
    public PlayerStyle.Character character;
    public float respawnDuration;
    private float respawnTime = 0;
    private bool playerHasSpawned = false;

    public void Start()
    {
        player.GetComponent<PlayerStyle>().SetCharacterStyle(character);
    }

    public void Update()
    {
        if (Time.time - respawnDuration < respawnTime)
        {
            GetComponent<test_MovementController>().enabled = false;
            RespawnTime();
        }
        else if (playerHasSpawned == false)
        {
            playerHasSpawned = true;
            GetComponent<test_MovementController>().enabled = true;
            foreach (SpriteRenderer spriteRenderer in player.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
                spriteRenderer.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    public void SpawnPlayer(GameObject player)
    {
        Destroy(player);
        this.player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        this.player.GetComponent<PlayerStyle>().SetCharacterStyle(character);
        playerMovement = this.player.GetComponentInChildren<PlayerMovement>();
        respawnTime = Time.time;
        playerHasSpawned = false;
        foreach (SpriteRenderer spriteRenderer in this.player.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            spriteRenderer.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void RespawnTime()
    {
        test_RaceStartController raceStartController = GetComponent<test_RaceStartController>();
        float distanceFromStartHeight = raceStartController.raceStartHeight - player.GetComponentInChildren<PlayerMovement>().transform.position.y;
        float upwardForce = raceStartController.neutralForce * ((distanceFromStartHeight > 0) ? 1 : 0f);
        player.GetComponentInChildren<PlayerMovement>().gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upwardForce);
        float rotationFromNeutral = (player.GetComponentInChildren<PlayerMovement>().transform.rotation.eulerAngles.z <= 180) ? -player.GetComponentInChildren<PlayerMovement>().transform.rotation.eulerAngles.z : 360 - player.GetComponentInChildren<PlayerMovement>().transform.rotation.eulerAngles.z;
        float torqueForce = raceStartController.rotationForce * rotationFromNeutral;
        player.GetComponentInChildren<PlayerMovement>().gameObject.GetComponent<Rigidbody2D>().AddTorque(torqueForce, ForceMode2D.Force);
    }
}