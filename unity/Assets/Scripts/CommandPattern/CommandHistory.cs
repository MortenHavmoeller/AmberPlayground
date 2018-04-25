using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class CommandHistory
{
	private const int HISTORY_MAX_LENGTH = 2000;
	private const float HISTORY_EXPIRATION_TIME = 10f;

	private static int lastFrame = 0;
	private static List<Frame> _history = new List<Frame>(HISTORY_MAX_LENGTH);

	private static uint _historyMilliseconds = 0;

	public static void AddCommands(ReadOnlyCollection<ICommand> newCommands)
	{
		TrimHistory();

		if (Time.frameCount != lastFrame)
		{
			AddFrameToHistory(Time.deltaTime);
			lastFrame = Time.frameCount;
		}

		_history[_history.Count - 1].AddCommands(newCommands);
	}

	public static int GetFrameCount()
	{
		return _history.Count;
	}

	public static int GetCommandCount()
	{
		int result = 0;
		for (int i = 0; i < _history.Count; i++)
		{
			result += _history[i].commands.Count;
		}

		return result;
	}

	private static void AddFrameToHistory(float deltaTime)
	{
		uint discreteTime = (uint)Mathf.RoundToInt(deltaTime * 1000f);
		_historyMilliseconds += discreteTime;

		_history.Add(new Frame(deltaTime));
	}

	private static void RemoveFrameFromHistory(int index)
	{
		float deltaTime = _history[index].deltaTime;
		_history.RemoveAt(0);

		uint discreteTime = (uint)Mathf.RoundToInt(deltaTime * 1000f);
		_historyMilliseconds -= discreteTime;
	}

	/// <summary>
	/// Undoes commands going back *at least* the specified amount of seconds.
	/// Returns the exact amount of seconds that was undone, -1 if none were undone, and -2 if all commands on record were undone.
	/// </summary>
	/// <param name="seconds"></param>
	/// <param name="overshootTarget"></param>
	/// <returns></returns>
	public static float UndoCommands(float seconds)
	{
		int framePtr = _history.Count - 1;
		float undoneTime = 0;

		while (undoneTime <= seconds)
		{
			if (framePtr < 0)
			{
				return -2;
			}

			Frame frame = _history[framePtr--];

			for (int i = frame.commands.Count - 1; i >= 0; i--)
			{
				frame.commands[i].Undo();
			}

			undoneTime += frame.deltaTime;
		}

		if (undoneTime == 0)
		{
			return -1;
		}

		return undoneTime;
	}

	public static void RedoCommands(float targetTime)
	{
		throw new System.NotImplementedException();
	}

	private static void TrimHistory()
	{
		while (_history.Count > HISTORY_MAX_LENGTH - 1)
		{
			RemoveFrameFromHistory(0);
		}

		while (_historyMilliseconds > HISTORY_EXPIRATION_TIME * 1000)
		{
			RemoveFrameFromHistory(0);
		}
	}
	
	private class Frame
	{
		//public float time;
		public float deltaTime;
		public List<ICommand> commands;

		public Frame(float deltaTime)
		{
			this.deltaTime = deltaTime;
			commands = new List<ICommand>();
		}

		public void AddCommands(ReadOnlyCollection<ICommand> newCommands)
		{
			commands.AddRange(newCommands);
		}
	}
}
