using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdJump : ICommand
{
	private ICharacter _character;
	private Vector3 _originalPosition;
	private Vector3 _originalVelocity;

	public CmdJump(ICharacter character)
	{
		_character = character;
		_originalPosition = _character.GetPosition();
		_originalVelocity = _character.GetVelocity();
	}

	public void Execute(float deltaTime)
	{
		_character.SetVelocityVertical(_character.GetVelocity().y + _character.GetJumpForce());
	}

	public void Undo()
	{
		_character.SetPosition(_originalPosition);
		_character.SetVelocityHorizontal(_originalVelocity);
	}
}
