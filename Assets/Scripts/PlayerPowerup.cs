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
    public GameObject noodlerArms;
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
        Vector3 size = laser.transform.localScale;
        laser.transform.SetParent(transform);
        laser.transform.localPosition = Vector3.zero;
        laser.transform.rotation = transform.rotation;
        laser.transform.SetParent(null);
        laser.transform.localScale = size;
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
        Rigidbody2D[] rigidbodies = transform.parent.GetComponentsInChildren<Rigidbody2D>();
        float originalGravity = rigidbodies[0].gravityScale;
        foreach (Rigidbody2D r in rigidbodies)
        {
            r.gravityScale = jupiterJumpGravity;
        }
        yield return new WaitForSeconds(generalPowerupDuration);
        foreach (Rigidbody2D r in rigidbodies)
        {
            r.gravityScale = originalGravity;
        }
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
        float originalScale = 0;
        Rigidbody2D[] bodyParts = transform.parent.GetComponentsInChildren<Rigidbody2D>();
        List<GameObject> arms = new List<GameObject>();
        foreach (Rigidbody2D r in bodyParts)
        {
            if (r.gameObject.tag == "player_arm")
            {
                r.gameObject.SetActive(false);
                GameObject arm = Instantiate(noodlerArms, transform.position, noodlerArms.transform.rotation);
                arm.transform.SetParent(transform.parent);
                float armPosition = noodlerArms.transform.position.x * ((arms.Count == 0) ? 1: -1);
                arm.transform.localPosition = new Vector2(armPosition, noodlerArms.transform.position.y);
                HingeJoint2D hinge = arm.GetComponent<HingeJoint2D>();
                hinge.connectedBody = GetComponent<Rigidbody2D>();
                float armAnchor = hinge.anchor.y * ((arms.Count == 0) ? 1 : -1);
                hinge.anchor = new Vector2(hinge.anchor.x, armAnchor);
                arms.Add(arm);
            }
        }
        yield return new WaitForSeconds(generalPowerupDuration);
        foreach (Rigidbody2D r in bodyParts)
        {
            if (r.gameObject.tag == "player_arm")
            {
                r.gameObject.SetActive(true);
            }
        }
        foreach (GameObject arm in arms)
        {
            Destroy(arm);
        }
    }
}
