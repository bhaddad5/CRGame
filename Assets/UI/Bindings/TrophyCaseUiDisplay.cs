using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public class TrophyCaseUiDisplay : MonoBehaviour
{
	[SerializeField] private Transform TrophyCaseParent;
	[SerializeField] private TrophyDisplay TrophyDisplayPrefab;

	public void UpdateVisuals(MainGameManager mgm)
	{
		foreach (var trophy in mgm.Data.GetOwnedTrophies())
		{
			var trophyOb = Instantiate(TrophyDisplayPrefab);
			trophyOb.Setup(trophy);
			trophyOb.transform.SetParent(TrophyCaseParent);
		}
	}
}
