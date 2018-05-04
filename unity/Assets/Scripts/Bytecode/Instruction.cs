using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAmber.Bytecode
{
	public enum Instruction : int // change to uint or ulong to increase capacity
	{
		Ping = 0x00,
		Terminate = 0x01,
		Literal = 0x03,
		Add = 0x04,
		PeekValue = 0x05,
		StartLoop = 0x06,
		EndLoop = 0x07
	}
}
