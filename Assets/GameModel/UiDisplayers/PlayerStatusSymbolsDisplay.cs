using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusSymbolsDisplay : MonoBehaviour
{
	[SerializeField] private TMP_Text CarName;
	[SerializeField] private Image CarImage;

	public void UpdateVisuals(MainGameManager mgm)
	{
		CarImage.sprite = mgm.Data.StatusSymbols.CarImage;
		CarName.text = mgm.Data.StatusSymbols.CarName;
	}
}
