using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerup : MonoBehaviour
{
    public enum Powerup
    {
        JupiterJump,
        Noodler
    }
    // Facelaser removed pending rethink
    public float generalPowerupDuration;
    public float jupiterJumpGravity;
    public GameObject leftNoodlerArm;
    public GameObject rightNoodlerArm;
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
            default:
                Debug.LogError("Powerup " + power + " not registered with player powerup");
                break;
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
        Rigidbody2D[] bodyParts = transform.parent.GetComponentsInChildren<Rigidbody2D>();
        List<GameObject> arms = new List<GameObject>();
        foreach (Rigidbody2D arm in bodyParts)
        {
            if (arm.gameObject.tag == "player_arm")
            {
                // Disable the real arms
                arm.GetComponent<SpriteRenderer>().enabled = false;
                // Enable the noodlers
                GameObject armToSpawn = (arm.name == "l arm") ? leftNoodlerArm : rightNoodlerArm;
                GameObject noodler = Instantiate(armToSpawn, transform.position, armToSpawn.transform.rotation);
                // Position noodlers over arms;
                noodler.transform.SetParent(arm.transform);
                noodler.transform.localPosition = Vector3.zero;
                noodler.transform.localRotation = Quaternion.identity;
                noodler.transform.SetParent(transform.parent);
                noodler.transform.localScale = armToSpawn.transform.localScale;
                HingeJoint2D hinge = noodler.GetComponentInChildren<HingeJoint2D>();
                hinge.connectedBody = GetComponentInChildren<Rigidbody2D>();
                // set the hierarchy correctly
                hinge.transform.SetParent(arm.transform.parent);
                Destroy(noodler);
                arms.Add(hinge.gameObject);
            }
        }
        yield return new WaitForSeconds(generalPowerupDuration);
        foreach (Rigidbody2D r in bodyParts)
        {
            if (r.gameObject.tag == "player_arm")
            {
                // Disable the real arms
                r.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        foreach (GameObject arm in arms)
        {
            Destroy(arm);
        }
    }
}
