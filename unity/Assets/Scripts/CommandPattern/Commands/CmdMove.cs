using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdMove : ICommand
{
	private ICharacter _character;
	private Vector3 _originalPosition;
	private Vector3 _originalVelocity;

	private Vector3 _amount;

	public CmdMove(ICharacter character, Vector3 amount)
	{
		_character = character;
		_originalPosition = _character.GetPosition();
		_originalVelocity = _character.GetVelocity();

		_amount = amount;
	}

	public void Execute(float deltaTime)
	{
		_character.SetVelocityHorizontal(_character.GetOrientation() * _amount * _character.GetSpeed() * deltaTime);
	}

	public void Undo()
	{
		_character.SetPosition(_originalPosition);
		_character.SetVelocityHorizontal(_originalVelocity);
	}
}
