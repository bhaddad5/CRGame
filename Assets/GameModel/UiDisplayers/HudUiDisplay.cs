using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUiDisplay : MonoBehaviour, IUiDisplay
{
	[SerializeField] private TMP_Text Actions;
	[SerializeField] private TMP_Text Ego;
	[SerializeField] private TMP_Text Funds;
	[SerializeField] private TMP_Text Power;

	[SerializeField] private TMP_Text Spreadsheets;
	[SerializeField] private TMP_Text Culture;
	[SerializeField] private TMP_Text Brand;
	[SerializeField] private TMP_Text Revanue;
	[SerializeField] private TMP_Text Patents;

	[SerializeField] private TMP_Text TurnNumber;
	[SerializeField] private Button EndTurn;

	public void Setup(MainGameManager mgm)
	{
		EndTurn.onClick.AddListener(() =>
		{
			mgm.EndTurn();
		});
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		Actions.text = $"{mgm.Data.Actions}";
		Ego.text = $"{mgm.Data.Ego}";
		Funds.text = $"${mgm.Data.Funds}";
		Power.text = $"{mgm.Data.Power}";

		Culture.text = $"{mgm.Data.CorporateCulture}";
		Spreadsheets.text = $"{mgm.Data.Spreadsheets}";
		Patents.text = $"{mgm.Data.Patents}";
		Brand.text = $"{mgm.Data.Brand}";
		Revanue.text = $"{mgm.Data.Revenue}";
		//TurnNumber.text = $"{mgm.Data.TurnNumber}";
	}
}
