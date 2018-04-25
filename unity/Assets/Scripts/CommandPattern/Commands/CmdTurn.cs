using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdTurn : ICommand
{
	private ICharacter _character;
	private Quaternion _originalOrientation;

	private float _amount;

	public CmdTurn(ICharacter character, float amount)
	{
		_character = character;
		_originalOrientation = _character.GetOrientation();
		_amount = amount;
	}

	public void Execute(float deltaTime)
	{
		Quaternion delta = Quaternion.Euler(0, _amount * _character.GetTurnSpeed() * deltaTime, 0);
		_character.SetOrientation(delta * _character.GetOrientation());
	}

	public void Undo()
	{
		_character.SetOrientation(_originalOrientation);
	}
}
