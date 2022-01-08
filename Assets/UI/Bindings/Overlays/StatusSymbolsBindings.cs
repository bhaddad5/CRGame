using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusSymbolsBindings : MonoBehaviour
{

	[SerializeField] private TMP_Text CarName;
	[SerializeField] private Image CarImage;

	[SerializeField] private TMP_Text SuitsName;
	[SerializeField] private Image SuitsImage;

	[SerializeField] private Image JewleryCuffs;
	[SerializeField] private Image JewleryPen;
	[SerializeField] private Image JewleryRing;
	[SerializeField] private Image JewleryWatch;

	[SerializeField] private List<StatusSymbolOption> CarOptions;
	[SerializeField] private List<StatusSymbolOption> SuitOptions;

	[Serializable]
	public struct StatusSymbolOption
	{
		[SerializeField] public string Name;
		[SerializeField] public Sprite Pic;
	}

	public void UpdateVisuals(MainGameManager mgm)
	{
		if (mgm.Data.Car >= CarOptions.Count)
		{
			Debug.Log($"Current car index {mgm.Data.Car} is out of bounds.");
			mgm.Data.Car = CarOptions.Count - 1;
		}

		CarName.text = CarOptions[mgm.Data.Car].Name;
		CarImage.sprite = CarOptions[mgm.Data.Car].Pic;

		if (mgm.Data.Suits >= SuitOptions.Count)
		{
			Debug.Log($"Current suit index {mgm.Data.Suits} is out of bounds.");
			mgm.Data.Suits = SuitOptions.Count - 1;
		}

		SuitsName.text = SuitOptions[mgm.Data.Suits].Name;
		SuitsImage.sprite = SuitOptions[mgm.Data.Suits].Pic;

		JewleryCuffs.gameObject.SetActive(mgm.Data.JewleryCuffs);
		JewleryPen.gameObject.SetActive(mgm.Data.JewleryPen);
		JewleryRing.gameObject.SetActive(mgm.Data.JewleryRing);
		JewleryWatch.gameObject.SetActive(mgm.Data.JewleryWatch);


		JewleryCuffs.transform.parent.gameObject.SetActive(mgm.Data.JewleryCuffs ||
		                                                   mgm.Data.JewleryPen ||
		                                                   mgm.Data.JewleryRing ||
		                                                   mgm.Data.JewleryWatch);
	}
}
