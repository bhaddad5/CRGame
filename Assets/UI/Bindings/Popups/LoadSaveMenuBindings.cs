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

public static class LoadSaveHelpers
{
	private static string savesDir => Path.Combine(Application.streamingAssetsPath, "Saves");

	public static void ValidateSaveFolder()
	{
		if (!Directory.Exists(savesDir))
			Directory.CreateDirectory(savesDir);
	}

	public static List<string> GetOrderedSaveFiles()
	{
		ValidateSaveFolder();

		var files = Directory.GetFiles(savesDir, "*.sav");
		
		return files.OrderByDescending(File.GetLastWriteTime).ToList();

	}

	public static string FileToValidPath(string file)
	{
		if (!Directory.Exists(savesDir))
			Directory.CreateDirectory(savesDir);

		if (file.EndsWith(".sav"))
			file = file.Remove(file.Length - 4);

		if (String.IsNullOrEmpty(file) ||
		    file.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
		    file.IndexOfAny(new[] { '.' }) >= 0)
			return null;


		string path = Path.Combine(savesDir, $"{file}.sav");

		return path;
	}
}

public class LoadSaveMenuBindings : MonoBehaviour
{
	[SerializeField] private Transform filesParent;
	[SerializeField] private TMP_InputField filenameInput;
	[SerializeField] private Button loadButton;
	[SerializeField] private Button saveButton;

	[SerializeField] private Button fileButtonPrefab;

	private MainGameManager mgm;
	private bool saving;

	public void Setup(MainGameManager mgm, bool saving)
	{
		LoadSaveHelpers.ValidateSaveFolder();

		this.mgm = mgm;
		this.saving = saving;

		loadButton.gameObject.SetActive(!saving);
		saveButton.gameObject.SetActive(saving);

		ShowSaveGames();
	}

	public void LoadGame()
	{
		string path = LoadSaveHelpers.FileToValidPath(filenameInput.text);
		if (path == null)
			return;
		mgm.InitializeGame(path, null, null);

		GameObject.Destroy(transform.parent.gameObject);
	}

	public void SaveGame()
	{
		string path = LoadSaveHelpers.FileToValidPath(filenameInput.text);
		if (path == null)
			return;
		File.WriteAllText(path, SaveLoadHandler.SaveToJson(mgm.Data));

		GameObject.Destroy(gameObject);
	}

	public void Cancel()
	{
		GameObject.Destroy(gameObject);
	}

	public void DeleteSave()
	{
		string path = LoadSaveHelpers.FileToValidPath(filenameInput.text);
		if (path == null)
			return;
		File.Delete(path);

		filenameInput.text = "";

		ShowSaveGames();
	}

	

	private void ShowSaveGames()
	{
		for (int i = 0; i < filesParent.childCount; i++)
		{
			GameObject.Destroy(filesParent.GetChild(i).gameObject);
		}

		var files = LoadSaveHelpers.GetOrderedSaveFiles();
		foreach (var file in files)
		{
			var button = Instantiate(fileButtonPrefab);
			button.transform.SetParent(filesParent);
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() =>
			{
				filenameInput.text = Path.GetFileNameWithoutExtension(file);
			});
			button.GetComponentsInChildren<TMP_Text>(true)[0].text = Path.GetFileNameWithoutExtension(file);
			button.GetComponentsInChildren<TMP_Text>(true)[1].text = File.GetLastWriteTime(file).ToString("MMM d, yyyy hh:mm tt");
		}
		
		if (saving)
		{
			filenameInput.text = $"New Save";
		}
		else
		{
			filenameInput.text = "";
		}
	}
}
