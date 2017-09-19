using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitColliderToObstacle : MonoBehaviour
{
    void Start()
    {
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		GetComponent<BoxCollider2D>().size = sprite.size;
    }
}
