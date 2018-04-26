﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SaveSomeData : MonoBehaviour
{
	public PlayerCharacter player;

	private void Start()
	{
		Test_SaveableClass data = new Test_SaveableClass("test-text", 1336, player);

		Debug.Log("created data with text: " + data.text);

		bool didSave = data.Save("some-path", "filename", ".extension");

		Debug.Log((didSave ? "saved data" : "FAILED saving data"));
	}

}