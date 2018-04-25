using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerInput
{
	private ICharacter _character;

	public PlayerInput(ICharacter character)
	{
		_character = character;
	}

	public ReadOnlyCollection<ICommand> HandleInput()
	{
		List<ICommand> commands = new List<ICommand>(3);

		float turningAmount = HandleTurning();
		if (!Mathf.Approximately(turningAmount, 0))
		{
			commands.Add(new CmdTurn(_character, turningAmount));
		}

		Vector3 moveAmount = HandleMovement();
		if (!Mathf.Approximately(moveAmount.sqrMagnitude, 0))
		{
			commands.Add(new CmdMove(_character, moveAmount));
		}

		bool jumping = HandleJumping();
		if (jumping)
		{
			commands.Add(new CmdJump(_character));
		}

		commands.TrimExcess();
		return commands.AsReadOnly();
	}

	private float HandleTurning()
	{
		float amount = 0;

		if (Input.GetKey(KeyCode.RightArrow))
		{
			amount += 1;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			amount -= 1;
		}

		return amount;
	}

	private Vector3 HandleMovement()
	{
		Vector3 amount = Vector3.zero;

		if (Input.GetKey(KeyCode.UpArrow))
		{
			amount += Vector3.forward;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			amount += Vector3.back;
		}

		return amount;
	}

	private bool HandleJumping()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			return true;
		}

		return false;
	}
	
}
