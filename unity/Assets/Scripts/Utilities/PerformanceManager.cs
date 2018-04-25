using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
	private void Start()
	{
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 1;
	}

}
