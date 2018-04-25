using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
	float GetSpeed();
	float GetJumpForce();

	float GetTurnSpeed();

	Vector3 GetPosition();
	Vector3 GetVelocity();
	Quaternion GetOrientation();

	void SetPosition(Vector3 position);
	void SetVelocityHorizontal(Vector3 velocity);
	void SetVelocityVertical(float velocity);
	void SetOrientation(Quaternion orientation);
}
