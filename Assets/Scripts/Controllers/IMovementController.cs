using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementController {
	MovementController.PlayerInput GetInputForPlayer(int playerId);
}
