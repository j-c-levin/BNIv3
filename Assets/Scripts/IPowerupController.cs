using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerupController {
	void CollectedPowerup(int playerId, Collider2D powerup);
}
