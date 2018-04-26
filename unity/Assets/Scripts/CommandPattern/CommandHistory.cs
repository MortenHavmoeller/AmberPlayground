using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class CommandHistory
{
	public static event Action OnHistoryReachedMax = delegate { };

	private const int HISTORY_MAX_LENGTH = 2000;
	private const float HISTORY_EXPIRATION_TIME = 10f;

	private static int lastFrameCount = 0;
	private static List<Frame> _history = new List<Frame>(HISTORY_MAX_LENGTH);

	private static uint _historyMilliseconds = 0;

	public static void AddCommands(ReadOnlyCollection<ICommand> newCommands)
	{
		if (Time.frameCount != lastFrameCount)
		{
			TrimHistory();
			AddFrameToHistory(Time.deltaTime);
			lastFrameCount = Time.frameCount;
		}

		GetLastFrame().AddCommands(newCommands);
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

	private static Frame GetLastFrame()
	{
		return _history[_history.Count - 1];
	}

	private static void AddFrameToHistory(float deltaTime)
	{
		uint discreteTime = ToMilliseconds(deltaTime);
		_historyMilliseconds += discreteTime;

		_history.Add(new Frame(deltaTime));
	}

	private static void RemoveFrameFromHistory(int index)
	{
		float deltaTime = _history[index].deltaTime;
		_history.RemoveAt(0);

		uint discreteTime = ToMilliseconds(deltaTime);
		_historyMilliseconds -= discreteTime;
	}

	private static uint ToMilliseconds(float time)
	{
		return (uint)Mathf.FloorToInt(time * 1000f);
	}

	private static float FromMilliseconds(uint milliseconds)
	{
		return (float)milliseconds / 1000f;
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

	public static bool ReplayFrame(int frame)
	{
		if (frame < 0 || frame > _history.Count - 1)
			return false;

		_history[frame].ExecuteAll();

		return true;
	}

	private static int FindTimeFrame(float time)
	{
		// guess the starting point (assume all frames are same length)
		uint startTimeMilliseconds = ToMilliseconds(time);

		float progressFraction = startTimeMilliseconds / (float)_historyMilliseconds;

		int framePtr = Mathf.RoundToInt(_history.Count * progressFraction);

		float historyTime = 0;

		if (progressFraction < 0.5f)
		{
			// count forwards...
			for (int i = 0; i < _history.Count; i++)
			{
				if (i > framePtr)
					break;

				historyTime += _history[i].deltaTime;
			}
		}
		else
		{
			// count backwards...
			historyTime = FromMilliseconds(_historyMilliseconds);
			for (int i = _history.Count - 1; i >= 0; i--)
			{
				if (i < framePtr)
					break;

				historyTime -= _history[i].deltaTime;
			}
		}

		if (historyTime < time)
		{
			// go forward...
			while (historyTime < time)
			{
				framePtr++;
				historyTime += _history[framePtr].deltaTime;
			}

		}
		else
		{
			// go backward...
			while (historyTime > time)
			{
				framePtr--;
				historyTime -= _history[framePtr].deltaTime;
			}

			framePtr++;
		}

		return framePtr;
	}

	private static void TrimHistory()
	{
		if (_history.Count > HISTORY_MAX_LENGTH - 1)
			OnHistoryReachedMax();

		while (_history.Count > HISTORY_MAX_LENGTH - 1)
		{
			
			RemoveFrameFromHistory(0);
		}

		if (_historyMilliseconds > HISTORY_EXPIRATION_TIME * 1000)
			OnHistoryReachedMax();

		while (_historyMilliseconds > HISTORY_EXPIRATION_TIME * 1000)
		{
			RemoveFrameFromHistory(0);
		}
	}

	private class Frame
	{
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

		public void ExecuteAll()
		{
			for (int i = 0; i < commands.Count; i++)
			{
				commands[i].Execute(deltaTime);
			}
		}
	}
}
