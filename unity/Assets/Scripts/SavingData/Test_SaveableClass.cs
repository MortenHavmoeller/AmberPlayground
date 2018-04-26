using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SaveableClass : SaveableData
{
	public string text;
	public int number;

	public GuidRef<PlayerCharacter> player;
	
	public Test_SaveableClass(string text, int number, PlayerCharacter player)
	{
		this.text = text;
		this.number = number;
		this.player = GuidRef<PlayerCharacter>.Create(player);
	}

}
