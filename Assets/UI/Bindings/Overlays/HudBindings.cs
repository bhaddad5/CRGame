using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudBindings : MonoBehaviour
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
	[SerializeField] private Button MyOffice;

	[SerializeField] private Button MainMenuButton;
	[SerializeField] private MainMenuBindings MainMenuPrefab;
	private MainMenuBindings mainMenu;

	private MainGameManager mgm;
	public void Setup(MainGameManager mgm, MainMapScreenBindings mapDisplay)
	{
		this.mgm = mgm;
		Rest.onClick.AddListener(() =>
		{
			mgm.Data.Ego += 5;
			mgm.HandleTurnChange();
		});
		MyOffice.onClick.AddListener(() =>
		{
			mapDisplay.ShowDepartment(mgm.Data.MyOffice, mgm);
		});

		MainMenuButton.onClick.AddListener(OpenMainMenu);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(mainMenu != null)
				CloseMainMenu();
			else
				OpenMainMenu();
		}
	}

	public void OpenMainMenu()
	{
		mainMenu = Instantiate(MainMenuPrefab);
		mainMenu.Setup(mgm);
	}

	public void CloseMainMenu()
	{
		GameObject.Destroy(mainMenu.gameObject);
		mainMenu = null;
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		PlayerName.text = mgm.Data.PlayerName;
		PlayerTitle.text = mgm.Data.PlayerPromotionLevels[mgm.Data.Promotion].Title;

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
	}
}
