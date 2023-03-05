using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

	[SerializeField] private Transform InventoryMenu;
	[SerializeField] private Transform InventoryParent;
	[SerializeField] private InventoryItemBindings InventoryItemPrefab;

	[SerializeField] private TMP_Text Day;
	[SerializeField] private TMP_Text Time;
	[SerializeField] private TMP_Text Month;

	[SerializeField] private Button DowntimeButton;

	[SerializeField] private Button MainMenuButton;

	[SerializeField] private Interaction MyOfficeTutorialCompleteInteraction;
	[SerializeField] private Interaction DowntimeTutorialCompleteInteraction;
	[SerializeField] private MainMenuBindings MainMenuPrefab;
	private MainMenuBindings mainMenu;

	private MainGameManager mgm;
	public void Setup(MainGameManager mgm)
	{
		this.mgm = mgm;

		foreach (var itemOption in mgm.Data.InventoryItemOptions)
		{
			var item = Instantiate(InventoryItemPrefab, InventoryParent);
			item.Setup(itemOption);
			item.RefreshUiDisplay(mgm);
		}
	}

	public void Rest()
	{
		mgm.Data.Ego += 5;
		mgm.HandleTurnChange();
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

		Ego.RefreshResourceDisplay(mgm.Data.Ego, mgm);
		Funds.RefreshResourceDisplay(mgm.Data.Funds, mgm);

		string timeOfDay = mgm.Data.TurnNumber % 2 == 1 ? "Afternoon" : "Morning";
		var dateTime = mgm.GetDateFromTurnNumber();
		Time.text = $"{timeOfDay}";
		Day.text = $"{dateTime.DayOfWeek}";
		Month.text = $"{dateTime:MMMM} {dateTime.Day}";
		
		DowntimeButton.gameObject.SetActive(DowntimeTutorialCompleteInteraction.Completed > 0);

		InventoryMenu.gameObject.SetActive(mgm.Data.Inventory.Any(inv => inv.Value > 0));

		foreach (var inventoryItemVis in InventoryParent.GetComponentsInChildren<InventoryItemBindings>(true))
		{
			inventoryItemVis.RefreshUiDisplay(mgm);
		}
	}
}
