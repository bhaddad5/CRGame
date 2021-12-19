using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.GameModel.Save;

public class LoadSaveMenuManager : MonoBehaviour
{
	[SerializeField] private Transform filesParent;
	[SerializeField] private TMP_InputField filenameInput;
	[SerializeField] private Button loadButton;
	[SerializeField] private Button saveButton;

	[SerializeField] private Button fileButtonPrefab;

	private MainGameManager mgm;
	private bool saving;

	string savesDir => Path.Combine(Application.streamingAssetsPath, "Saves");
	public void Setup(MainGameManager mgm, bool saving)
	{
		this.mgm = mgm;
		this.saving = saving;

		loadButton.gameObject.SetActive(!saving);
		saveButton.gameObject.SetActive(saving);

		ShowSaveGames();
	}

	public void LoadGame()
	{
		if (!CurrentDesiredFileIsValid())
			return;

		string path = Path.Combine(savesDir, $"{filenameInput.text}.sav");
		mgm.InitializeGame(path);

		GameObject.Destroy(transform.parent.gameObject);
	}

	public void SaveGame()
	{
		if (!CurrentDesiredFileIsValid())
			return;

		string path = Path.Combine(savesDir, $"{filenameInput.text}.sav");
		File.WriteAllText(path, SaveLoadHandler.SaveToJson(mgm.Data));

		GameObject.Destroy(gameObject);
	}

	public void Cancel()
	{
		GameObject.Destroy(gameObject);
	}

	public void DeleteSave()
	{
		if (!CurrentDesiredFileIsValid())
			return;

		string path = Path.Combine(savesDir, $"{filenameInput.text}.sav");
		File.Delete(path);

		filenameInput.text = "";

		ShowSaveGames();
	}

	private bool CurrentDesiredFileIsValid()
	{
		if (!Directory.Exists(savesDir))
			Directory.CreateDirectory(savesDir);

		if (filenameInput.text.EndsWith(".sav"))
			filenameInput.text = filenameInput.text.Remove(filenameInput.text.Length - 4);

		if (String.IsNullOrEmpty(filenameInput.text) ||
		    filenameInput.text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
		    filenameInput.text.IndexOfAny(new[] { '.' }) >= 0)
			return false;
		return true;
	}

	private void ShowSaveGames()
	{
		for (int i = 0; i < filesParent.childCount; i++)
		{
			GameObject.Destroy(filesParent.GetChild(i).gameObject);
		}

		if (!Directory.Exists(savesDir))
			Directory.CreateDirectory(savesDir);
		var files = Directory.GetFiles(savesDir, "*.sav");
		foreach (var file in files)
		{
			var button = Instantiate(fileButtonPrefab);
			button.transform.SetParent(filesParent);
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() =>
			{
				filenameInput.text = Path.GetFileNameWithoutExtension(file);
			});
			button.GetComponentInChildren<TMP_Text>(true).text = Path.GetFileNameWithoutExtension(file);
		}
		
		if (saving)
		{
			filenameInput.text = $"New Game";
		}
		else
		{
			filenameInput.text = "";
		}
	}
}
