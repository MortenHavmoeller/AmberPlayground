using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, ICharacter
{
	public float speed = 1;
	public float jumpForce = 1;
	public float turnSpeed = 10;

	private Vector3 velocity = Vector3.zero;

	public float GetSpeed() { return speed; }
	public float GetJumpForce() { return jumpForce; }
	public float GetTurnSpeed() { return turnSpeed; }
	public Vector3 GetPosition() { return transform.position; }
	public Vector3 GetVelocity() { return velocity; }
	public Quaternion GetOrientation() { return transform.rotation; }

	public void SetPosition(Vector3 position) { transform.position = position; }
	public void SetVelocityHorizontal(Vector3 velocity) { this.velocity = new Vector3(velocity.x, this.velocity.y, velocity.z); }
	public void SetVelocityVertical(float velocity) { this.velocity.y = velocity; }
	public void SetOrientation(Quaternion orientation) { transform.rotation = orientation; }

	PlayerInput playerInput;

	private void Start()
	{
		playerInput = new PlayerInput(this);
	}

	private void Update()
	{
		ReadOnlyCollection<ICommand> commands = playerInput.HandleInput();
		ExecuteCommands(commands);
		SaveCommands(commands);

		transform.position += velocity;

		if (transform.position.y > 0)
		{
			velocity.y -= 1.5f * Time.deltaTime;
		}
		else
		{
			velocity.y = 0;
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}

		velocity.x = 0;
		velocity.z = 0;
	}

	private void ExecuteCommands(ReadOnlyCollection<ICommand> commands)
	{
		for (int i = 0; i < commands.Count; i++)
		{
			commands[i].Execute(Time.deltaTime);
		}
	}

	private void SaveCommands(ReadOnlyCollection<ICommand> commands)
	{
		CommandHistory.AddCommands(commands);
		//Debug.Log("Command history frame count: " + CommandHistory.GetFrameCount() + "\nCommand history command count: " + CommandHistory.GetCommandCount());
	}
}
