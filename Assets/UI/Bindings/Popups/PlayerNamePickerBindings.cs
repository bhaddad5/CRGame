using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNamePickerBindings : MonoBehaviour
{
	private Action<string, string> startGame;
	[SerializeField] private TMP_InputField FirstName;
	[SerializeField] private TMP_InputField LastName;

	public void Setup(string defaultFirstName, string defaultLastName, Action<string, string> startGame)
	{
		this.startGame = startGame;
		FirstName.text = defaultFirstName;
		LastName.text = defaultLastName;
	}

	public void StartGame()
	{
		string fn = FirstName.text;
		string ln = LastName.text;

		if (!String.IsNullOrEmpty(fn) && !String.IsNullOrEmpty(ln))
		{
			ClosePopup();
			startGame?.Invoke(fn, ln);
		}
	}

	public void ClosePopup()
	{
		GameObject.Destroy(transform.parent.gameObject);
	}
}
