using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUiDisplay : MonoBehaviour, IUiDisplay
{
	[SerializeField] private TMP_Text PlayerName;
	[SerializeField] private TMP_Text PlayerTitle;

	[SerializeField] private TMP_Text Ego;
	[SerializeField] private TMP_Text Funds;
	[SerializeField] private TMP_Text Power;

	[SerializeField] private TMP_Text Spreadsheets;
	[SerializeField] private TMP_Text Culture;
	[SerializeField] private TMP_Text Brand;
	[SerializeField] private TMP_Text Revanue;
	[SerializeField] private TMP_Text Patents;

	[SerializeField] private TMP_Text Day;
	[SerializeField] private TMP_Text Time;
	[SerializeField] private TMP_Text Month;

	[SerializeField] private Button Rest;
	[SerializeField] private Button PlayerOffice;

	[SerializeField] private PlayerOfficeUiDisplay PlayerOfficeUiPrefab;

	[SerializeField] private Button MainMenuButton;
	[SerializeField] private Button SaveGameButton;
	[SerializeField] private Button LoadGameButton;
	[SerializeField] private Button ReturnToGameButton;
	[SerializeField] private Button QuitGameButton;

	[SerializeField] private Transform MainMenu;

	private MainGameManager mgm;
	public void Setup(MainGameManager mgm)
	{
		this.mgm = mgm;
		Rest.onClick.AddListener(() =>
		{
			mgm.Data.Ego += 5;
			mgm.HandleTurnChange();
		});
		PlayerOffice.onClick.AddListener(ShowPlayerOffice);

		MainMenuButton.onClick.AddListener(ShowMainMenu);
		ReturnToGameButton.onClick.AddListener(HideMainMenu);
		QuitGameButton.onClick.AddListener(Application.Quit);
		SaveGameButton.onClick.AddListener(SaveGame);
		LoadGameButton.onClick.AddListener(LoadGame);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(MainMenu.gameObject.activeSelf)
				HideMainMenu();
			else
				ShowMainMenu();
		}
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		PlayerName.text = mgm.Data.PlayerName;
		PlayerTitle.text = mgm.GetPlayerTitleFromPower();

		Ego.text = $"{mgm.Data.Ego}";
		Funds.text = $"${mgm.Data.Funds}";
		Power.text = $"{mgm.Data.Power}";

		Culture.text = $"{mgm.Data.CorporateCulture}";
		Spreadsheets.text = $"{mgm.Data.Spreadsheets}";
		Patents.text = $"{mgm.Data.Patents}";
		Brand.text = $"{mgm.Data.Brand}";
		Revanue.text = $"{mgm.Data.Revenue}";

		string timeOfDay = mgm.Data.TurnNumber % 2 == 1 ? "Afternoon" : "Morning";
		var DateTime = mgm.GetDateFromTurnNumber();
		Time.text = $"{timeOfDay}";
		Day.text = $"{DateTime.DayOfWeek}";
		Month.text = $"{DateTime:MMMM} {DateTime.Day}";

		playerOfficeDisplay?.RefreshUiDisplay(mgm);
	}

	private PlayerOfficeUiDisplay playerOfficeDisplay = null;
	public void ShowPlayerOffice()
	{
		playerOfficeDisplay = Instantiate(PlayerOfficeUiPrefab);
		playerOfficeDisplay.Setup(this, mgm);
		playerOfficeDisplay.RefreshUiDisplay(mgm);
		PlayerOffice.gameObject.SetActive(false);
	}

	public void ClosePlayerOffice()
	{
		if (playerOfficeDisplay != null)
		{
			GameObject.Destroy(playerOfficeDisplay.gameObject);
			playerOfficeDisplay = null;
			PlayerOffice.gameObject.SetActive(true);
		}
	}

	public void ShowMainMenu()
	{
		MainMenu.gameObject.SetActive(true);
	}

	public void HideMainMenu()
	{
		MainMenu.gameObject.SetActive(false);
	}

	string savesDir => Path.Combine(Application.streamingAssetsPath, "Saves");
	private void SaveGame()
	{
		if (!Directory.Exists(savesDir))
			Directory.CreateDirectory(savesDir);

		string defaultSaveName = $"CMan Save {DateTime.Now:yyyy-MM-dd-hh-mm-ss}.sav";
		string xmlGameData = mgm.GetXmlSaveData();
		string path = Path.Combine(savesDir, defaultSaveName);
		File.WriteAllText(path, xmlGameData);
	}

	private void LoadGame()
	{
		if (!Directory.Exists(savesDir))
			Directory.CreateDirectory(savesDir);
		var files = Directory.GetFiles(savesDir, "*.sav");
		if (files.Length > 0)
		{
			var saveFile = files[files.Length - 1];
			mgm.InitializeGame(saveFile);
		}
	}
}
