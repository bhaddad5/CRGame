using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUiDisplay : MonoBehaviour, IUiDisplay
{
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

	public void Setup(MainGameManager mgm)
	{
		Rest.onClick.AddListener(() =>
		{
			mgm.Data.Ego += 5;
			mgm.HandleTurnChange();
		});
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
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
		Month.text = $"{DateTime:MMMM} {DateTime.Day}";}
}
