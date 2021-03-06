﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
	void Execute(float deltaTime);
	void Undo();
}
