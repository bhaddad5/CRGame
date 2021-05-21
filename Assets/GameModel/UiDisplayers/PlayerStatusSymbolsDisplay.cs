using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusSymbolsDisplay : MonoBehaviour
{
	[SerializeField] private Image CarImage;
	[SerializeField] private Image SuitsImage;

	public void UpdateVisuals(MainGameManager mgm)
	{
		CarImage.sprite = mgm.Data.StatusSymbols.CarImage;
		CarImage.GetComponent<RectTransform>().ApplyLayout(mgm.Data.StatusSymbols.CarLayout);
	}
}
