using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    public enum Powerup
    {
        None,
        JupiterJump,
        Noodler,
        Facelaser
    }
    public float generalPowerupDuration;
    public float jupiterJumpGravity;
    public float noodlerScale;
    public GameObject facelaserLazer;
    public float facelaserFireRate;
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

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == powerupLayer)
        {
            powerupController.CollectedPowerup(playerId, collider);
        }
    }

    public void UsePowerup(int castingPlayerId, Powerup power)
    {
        switch (power)
        {
            case Powerup.JupiterJump:
                JupiterJump(castingPlayerId);
                break;
            case Powerup.Noodler:
                Noodler(castingPlayerId);
                break;
            case Powerup.Facelaser:
                Facelaser(castingPlayerId);
                break;
            default:
                Debug.LogError("Powerup " + power + " not registered with player powerup");
                break;
        }
    }
    private void Facelaser(int castingPlayerId)
    {
        if (playerId == castingPlayerId)
        {
            StartCoroutine("FacelaserRoutine");
        }
    }

    private IEnumerator FacelaserRoutine()
    {
        float currentTime = Time.time;
        float lastShotTime = 0;
        IMovementController input = GameObject.FindGameObjectWithTag("GameController").GetComponent<IMovementController>();
        if (input == null)
        {
            Debug.LogError("No movement controller found");
        }
        MovementController.PlayerInput currentInput;
        while (Time.time - currentTime < generalPowerupDuration)
        {
            currentInput = input.GetInputForPlayer(playerId);
            bool readyToShoot = Time.time - lastShotTime >= facelaserFireRate;
            if (readyToShoot)
            {
                if (currentInput.rightButton)
                {
                    lastShotTime = Time.time;
                    float direction = (transform.rotation.eulerAngles.z <= 180) ? 180 : 0;
                    createLazer(direction);
                }
                if (currentInput.leftButton)
                {
                    lastShotTime = Time.time;
                    float direction = (transform.rotation.eulerAngles.z <= 180) ? 0 : 190;
                    createLazer(direction);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void createLazer(float direction)
    {
        GameObject laser = Instantiate(facelaserLazer, Vector3.zero, Quaternion.identity);
        laser.transform.SetParent(transform);
        laser.transform.localPosition = Vector3.zero;
        laser.transform.rotation = transform.rotation;
        laser.transform.localScale = Vector3.one;
        laser.transform.SetParent(null);
        laser.transform.Rotate(0, 0, direction);
        laser.GetComponent<Facelaser>().castingPlayerId = playerId;
    }

    private void JupiterJump(int castingPlayerId)
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
        yield return new WaitForSeconds(generalPowerupDuration);
        player.gravityScale = originalGravity;
    }

    private void Noodler(int castingPlayerId)
    {
        if (playerId != castingPlayerId)
        {
            StartCoroutine("NoodlerRoutine");
        }
    }

    private IEnumerator NoodlerRoutine()
    {
        float originalScale = transform.localScale.y;
        transform.localScale = new Vector2(transform.localScale.x, noodlerScale);
        yield return new WaitForSeconds(generalPowerupDuration);
        transform.localScale = new Vector2(transform.localScale.x, originalScale);
    }
}
