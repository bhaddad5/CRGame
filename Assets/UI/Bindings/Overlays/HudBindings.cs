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

	[SerializeField] private Button MyOffice;

	[SerializeField] private Button MainMenuButton;
	[SerializeField] private MainMenuBindings MainMenuPrefab;
	private MainMenuBindings mainMenu;

	private MainGameManager mgm;
	private MainMapScreenBindings mapDisplay;
	public void Setup(MainGameManager mgm, MainMapScreenBindings mapDisplay)
	{
		this.mapDisplay = mapDisplay;
		this.mgm = mgm;
		
		MainMenuButton.onClick.AddListener(OpenMainMenu);
	}

	public void Rest()
	{
		mgm.Data.Ego += 5;
		mgm.HandleTurnChange();
	}

	public void OpenMyOffice()
	{
		mapDisplay.ShowDepartment(mgm.Data.MyOffice, mgm);
	}

	public void OpenMyHome()
	{
		mapDisplay.ShowDepartment(mgm.Data.MyHome, mgm);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(mainMenu != null)
				CloseMainMenu();
			else if (mainMenu == null)
				OpenMainMenu();
		}
	}

	public void OpenMainMenu()
	{
		var popupParent = GameObject.Instantiate(UiPrefabReferences.Instance.PopupOverlayParent);
		mainMenu = GameObject.Instantiate(MainMenuPrefab, popupParent.transform);
		mainMenu.Setup(mgm);
	}

	public void CloseMainMenu()
	{
		GameObject.Destroy(mainMenu.transform.parent.gameObject);
		mainMenu = null;
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		PlayerName.text = $"{mgm.Data.FirstName} {mgm.Data.LastName}";
		PlayerTitle.text = mgm.Data.PlayerPromotionLevels[mgm.Data.Promotion].Title;

		Ego.RefreshResourceDisplay(mgm.Data.Ego);
		Funds.RefreshResourceDisplay(mgm.Data.Funds);
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

		MyOffice.interactable = mgm.Data.MyOffice.IsAccessible(mgm);
	}
}
