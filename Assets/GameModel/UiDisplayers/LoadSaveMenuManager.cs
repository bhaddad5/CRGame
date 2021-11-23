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
	[SerializeField] private Button cancelButton;

	[SerializeField] private Button fileButtonPrefab;

	string savesDir => Path.Combine(Application.streamingAssetsPath, "Saves");
	public void Setup(MainGameManager mgm)
	{
		loadButton.onClick.RemoveAllListeners();
		loadButton.onClick.AddListener(() =>
		{
			if (!CurrentDesiredFileIsValid())
				return;

			string path = Path.Combine(savesDir, $"{filenameInput.text}.sav");
			mgm.InitializeGame(path);

			gameObject.SetActive(false);
		});
		saveButton.onClick.RemoveAllListeners();
		saveButton.onClick.AddListener(() =>
		{
			if (!CurrentDesiredFileIsValid())
				return;
			
			string path = Path.Combine(savesDir, $"{filenameInput.text}.sav");
			File.WriteAllText(path, SaveLoadHandler.SaveToJson(mgm.Data));

			gameObject.SetActive(false);
		});
		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});

		gameObject.SetActive(false);
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

	public void Show(bool load)
	{
		gameObject.SetActive(true);
		loadButton.gameObject.SetActive(load);
		saveButton.gameObject.SetActive(!load);

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
		
		if (load)
		{
			if (files.Length > 0)
			{
				var directory = new DirectoryInfo(savesDir);
				var myFile = (from f in directory.GetFiles()
					orderby f.LastWriteTime descending
					select f).First();
				filenameInput.text = Path.GetFileNameWithoutExtension(myFile.Name);
			}
			else
				filenameInput.text = "";
		}
		else
		{
			string defaultSaveName = $"New Game";
			filenameInput.text = defaultSaveName;
		}
	}
}
