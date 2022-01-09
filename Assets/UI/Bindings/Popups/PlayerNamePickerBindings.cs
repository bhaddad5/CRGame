using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNamePickerBindings : MonoBehaviour
{
	private Action<string> startGame;
	public void Setup(string defaultPlayerName, Action<string> startGame)
	{
		this.startGame = startGame;
		GetComponentInChildren<TMP_InputField>().text = defaultPlayerName;
	}

	public void StartGame()
	{
		string name = GetComponentInChildren<TMP_InputField>().text;

		if (!String.IsNullOrEmpty(name))
		{
			ClosePopup();
			startGame?.Invoke(name);
		}
	}

	public void ClosePopup()
	{
		GameObject.Destroy(transform.parent.gameObject);
	}
}
