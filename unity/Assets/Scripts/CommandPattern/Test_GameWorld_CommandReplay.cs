using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GameWorld_CommandReplay : MonoBehaviour
{
	public Transform playerObject;

	private Vector3 _playerOrigPos;
	private Quaternion _playerOrigRot;

	private bool historyReachedMaxOnce = false;

	private Coroutine _replayCoroutine;

	private void Start()
	{
		_playerOrigPos = playerObject.position;
		_playerOrigRot = playerObject.rotation;
	}

	private void OnEnable()
	{
		CommandHistory.OnHistoryReachedMax += HistoryReachedMax;
	}

	private void OnDisable()
	{
		CommandHistory.OnHistoryReachedMax -= HistoryReachedMax;
	}

	private void HistoryReachedMax()
	{
		if (historyReachedMaxOnce)
			return;

		StartReplayCoroutine();
		historyReachedMaxOnce = true;
	}

	private void StartReplayCoroutine()
	{
		if (_replayCoroutine != null)
			return;

		_replayCoroutine = StartCoroutine(ReplayCoroutine(StartReplayCoroutine));
	}

	private IEnumerator ReplayCoroutine(Action onComplete = null)
	{
		PlayerCharacter player = playerObject.GetComponent<PlayerCharacter>();
		player.Lobotomize();

		playerObject.position = _playerOrigPos;
		playerObject.rotation = _playerOrigRot;

		int frame = 0;

		while (true)
		{
			if (CommandHistory.ReplayFrame(frame))
			{
				frame++;
				yield return null;
			}
			else
			{
				break;
			}
		}

		Debug.Log("Replay done");

		_replayCoroutine = null;

		if (onComplete != null)
			onComplete.Invoke();
	}
}
