using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SaveableClass : SaveableData
{
	public string text;
	public int number;

	public Vector3 vector;

	public PlayerCharacter player;
	
	public Test_SaveableClass(string text, int number, Vector3 vector, PlayerCharacter player)
	{
		this.text = text;
		this.number = number;
		this.vector = vector;
		this.player = player;
	}

}
