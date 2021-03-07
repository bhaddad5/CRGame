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

	[SerializeField] private ResourceManagerUiDisplay Ego;
	[SerializeField] private ResourceManagerUiDisplay Funds;
	[SerializeField] private ResourceManagerUiDisplay Power;

	[SerializeField] private ResourceManagerUiDisplay Spreadsheets;
	[SerializeField] private ResourceManagerUiDisplay Culture;
	[SerializeField] private ResourceManagerUiDisplay Brand;
	[SerializeField] private ResourceManagerUiDisplay Revanue;
	[SerializeField] private ResourceManagerUiDisplay Patents;

	[SerializeField] private ResourceManagerUiDisplay Hornical;

	[SerializeField] private TMP_Text Day;
	[SerializeField] private TMP_Text Time;
	[SerializeField] private TMP_Text Month;

	[SerializeField] private Button Rest;
	[SerializeField] private Button PlayerOffice;

	[SerializeField] private PlayerOfficeUiDisplay PlayerOfficeUiPrefab;

	[SerializeField] private PopupUiDisplay PopupDisplay;

	[SerializeField] private Button MainMenuButton;
	[SerializeField] private Button SaveGameButton;
	[SerializeField] private Button LoadGameButton;
	[SerializeField] private Button ReturnToGameButton;
	[SerializeField] private Button QuitGameButton;

	[SerializeField] private Transform MainMenu;

	[SerializeField] private LoadSaveMenuManager LoadSaveMenuManager;

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

		LoadSaveMenuManager.Setup(mgm);

		HideMainMenu();
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

		Ego.RefreshResourceDisplay(mgm.Data.Ego);
		Funds.RefreshResourceDisplay(mgm.Data.Funds, funds => $"${funds}");
		Power.RefreshResourceDisplay(mgm.Data.Power);

		Culture.RefreshResourceDisplay(mgm.Data.CorporateCulture);
		Spreadsheets.RefreshResourceDisplay(mgm.Data.Spreadsheets);
		Patents.RefreshResourceDisplay(mgm.Data.Patents);
		Brand.RefreshResourceDisplay(mgm.Data.Brand);
		Revanue.RefreshResourceDisplay(mgm.Data.Revenue);

		Hornical.RefreshResourceDisplay(mgm.Data.Hornical);

		string timeOfDay = mgm.Data.TurnNumber % 2 == 1 ? "Afternoon" : "Morning";
		var DateTime = mgm.GetDateFromTurnNumber();
		Time.text = $"{timeOfDay}";
		Day.text = $"{DateTime.DayOfWeek}";
		Month.text = $"{DateTime:MMMM} {DateTime.Day}";

		playerOfficeDisplay?.RefreshUiDisplay(mgm);
	}

	public void ShowPopup(Popup popup, Action onPopupDone)
	{
		PopupDisplay.Show(popup, mgm, onPopupDone);
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

	
	private void SaveGame()
	{
		LoadSaveMenuManager.Show(false);
	}

	private void LoadGame()
	{
		LoadSaveMenuManager.Show(true);
	}
}
