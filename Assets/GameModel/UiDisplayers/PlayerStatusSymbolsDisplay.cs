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

	[SerializeField] private TMP_Text SuitsName;
	[SerializeField] private Image SuitsImage;

	[SerializeField] private Image JewleryCuffs;
	[SerializeField] private Image JewleryPen;
	[SerializeField] private Image JewleryRing;
	[SerializeField] private Image JewleryWatch;
	public void UpdateVisuals(MainGameManager mgm)
	{
		CarName.text = mgm.Data.StatusSymbols.CarName;
		CarImage.sprite = mgm.Data.StatusSymbols.CarImage;

		SuitsName.text = mgm.Data.StatusSymbols.SuitsName;
		SuitsImage.sprite = mgm.Data.StatusSymbols.SuitsImage;

		JewleryCuffs.sprite = mgm.Data.StatusSymbols.JewleryCuffs;
		JewleryCuffs.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryCuffs != null);
		JewleryPen.sprite = mgm.Data.StatusSymbols.JewleryPen;
		JewleryPen.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryPen != null);
		JewleryRing.sprite = mgm.Data.StatusSymbols.JewleryRing;
		JewleryRing.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryRing != null);
		JewleryWatch.sprite = mgm.Data.StatusSymbols.JewleryWatch;
		JewleryWatch.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryWatch != null);


		JewleryCuffs.transform.parent.gameObject.SetActive(mgm.Data.StatusSymbols.JewleryCuffs != null ||
		                                                   mgm.Data.StatusSymbols.JewleryPen != null ||
		                                                   mgm.Data.StatusSymbols.JewleryRing != null ||
		                                                   mgm.Data.StatusSymbols.JewleryWatch != null);
	}
}
