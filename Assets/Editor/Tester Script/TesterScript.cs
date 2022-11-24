using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameModel.UiDisplayers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Editor.Tester_Script
{
	public static class TesterScript
	{
		[MenuItem("Company Man Tester AI/Start Tester AI", false, 0)]
		public static void StartTesterAi()
		{
			if (GameObject.Find("AI") != null)
			{
				Debug.LogWarning("Cannot start a new Tester AI while one is already running.");
				return;
			}

			new GameObject("AI").AddComponent<TesterAI>();
		}

		[MenuItem("Company Man Tester AI/Stop Tester AI", false, 0)]
		public static void StopTesterAI()
		{
			if (GameObject.Find("AI") == null)
			{
				Debug.LogWarning("No running tester to stop.");
				return;
			}

			if(Application.isPlaying)
				GameObject.Destroy(GameObject.Find("AI"));
			else
				GameObject.DestroyImmediate(GameObject.Find("AI"));
		}
	}
}
