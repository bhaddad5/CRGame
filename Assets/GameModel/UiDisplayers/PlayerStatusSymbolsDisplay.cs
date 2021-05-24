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

	[SerializeField] private Image JewleryCuffs;
	[SerializeField] private Image JewleryPen;
	[SerializeField] private Image JewleryRing;
	[SerializeField] private Image JewleryWatch;
	public void UpdateVisuals(MainGameManager mgm)
	{
		CarImage.sprite = mgm.Data.StatusSymbols.CarImage;
		CarImage.GetComponent<RectTransform>().ApplyLayout(mgm.Data.StatusSymbols.CarLayout);

		SuitsImage.sprite = mgm.Data.StatusSymbols.SuitsImage;

		JewleryCuffs.sprite = mgm.Data.StatusSymbols.JewleryCuffs;
		JewleryCuffs.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryCuffs != null);
		JewleryPen.sprite = mgm.Data.StatusSymbols.JewleryPen;
		JewleryPen.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryPen != null);
		JewleryRing.sprite = mgm.Data.StatusSymbols.JewleryRing;
		JewleryRing.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryRing != null);
		JewleryWatch.sprite = mgm.Data.StatusSymbols.JewleryWatch;
		JewleryWatch.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryWatch != null);
	}
}
