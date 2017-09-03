using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed;
    public delegate void reachedDestination();
    public reachedDestination destinationReachedDelegate;
    private Transform target;
    public void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, target.position, movementSpeed);
            // Keep the camera at the same height and move only in a 2d plane
            newPosition.z = transform.position.z;
            transform.position = newPosition;
            if (CameraAtTarget() && DelegateAssigned())
            {
				target = null;
                destinationReachedDelegate();
            }
        }
    }

    public void SetTarget(GameObject target)
    {
		this.target = target.transform;
    }

    private bool CameraAtTarget()
    {
        // We only need to check x and y in 2d
        bool xMatches = ApproximateLocation(transform.position.x, target.position.x);
        bool yMatches = ApproximateLocation(transform.position.y, target.position.y);
        return xMatches && yMatches;
    }

    private bool ApproximateLocation(float source, float destination)
    {
        return Mathf.Abs(destination - source) <= 0.2;
    }

    private bool DelegateAssigned()
    {
        return destinationReachedDelegate != null;
    }
}
