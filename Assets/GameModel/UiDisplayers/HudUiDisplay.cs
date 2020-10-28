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
	[SerializeField] private TMP_Text Culture;
	[SerializeField] private TMP_Text Power;
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
		Actions.text = $"Actions: {mgm.Data.Actions}";
		Ego.text = $"Ego: {mgm.Data.Ego}";
		Funds.text = $"Funds: ${mgm.Data.Funds}";
		Culture.text = $"Corp Culture: {mgm.Data.CorporateCulture}";
		Power.text = $"Power: {mgm.Data.Power}";
		TurnNumber.text = $"{mgm.Data.TurnNumber}";
	}
}
