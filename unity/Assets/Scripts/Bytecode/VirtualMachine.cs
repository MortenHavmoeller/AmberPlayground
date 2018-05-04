using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAmber.Bytecode
{
	public class VirtualMachine : MonoBehaviour
	{
		private Stack<int> stack = new Stack<int>();
		private Stack<int> flowStack = new Stack<int>();

		private int cnt = 0;

		void Start()
		{
			int[] instructions = Test_CreateBytecode();
			Interpret(instructions, instructions.Length);
		}

		private int[] Test_CreateBytecode()
		{
			List<int> instructionSet = new List<int>();

			instructionSet.Add((int)Instruction.Literal);
			instructionSet.Add(10);
			instructionSet.Add((int)Instruction.PeekValue);

			// LOOP
			instructionSet.Add((int)Instruction.StartLoop);
			instructionSet.Add((int)Instruction.Literal);
			instructionSet.Add(25);
			instructionSet.Add((int)Instruction.Add);
			instructionSet.Add((int)Instruction.PeekValue);
			instructionSet.Add((int)Instruction.EndLoop);


			instructionSet.Add((int)Instruction.Ping);
			instructionSet.Add((int)Instruction.PeekValue);

			return instructionSet.ToArray();
		}

		private void Interpret(int[] bytecode, int size)
		{
			for (int i = 0; i < size; i++)
			{
				int instruction = bytecode[i];

				switch ((Instruction)instruction)
				{
					case Instruction.Ping:
						Debug.Log("[" + i + "]" + "Instruction->Ping");
						break;

					case Instruction.Terminate:
						Debug.Log("[" + i + "]" + "Instruction->Terminate");
						stack.Clear();
						break;

					case Instruction.Literal:
						Debug.Log("[" + i + "]" + "Instruction->Literal: Value: " + bytecode[i + 1]);
						int literalValue = bytecode[++i]; // advance and read the value
						stack.Push(literalValue);
						break;

					case Instruction.Add:
						Debug.Log("[" + i + "]" + "Instruction->Add");
						// read in reverse order to respect stack order
						int addValueB = stack.Pop();
						int addValueA = stack.Pop();
						stack.Push(addValueA + addValueB);
						break;

					case Instruction.PeekValue:
						int displayValue = stack.Peek();
						Debug.Log("[" + i + "]" + "Instruction->PeekValue: Stack value: " + displayValue);
						break;

					case Instruction.StartLoop:
						Debug.Log("[" + i + "]" + "Instruction->StartLoop: Loop start index " + i);
						int loopStartIndex = i;
						flowStack.Push(loopStartIndex);
						break;

					case Instruction.EndLoop:

						if (cnt < 5) // if the loop should still run
						{
							Debug.Log("[" + i + "]" + "Instruction->EndLoop: Going back to instruction " + flowStack.Peek());
							cnt++;
							i = flowStack.Peek();
							break;
						}
						flowStack.Pop();
						Debug.Log("[" + i + "]" + "Instruction->EndLoop: LOOP DONE");
						break;

					default:
						Debug.LogWarning("[" + i + "]" + "Unrecognized instruction " + instruction);
						break;
				}
			}
		}
	}
}
