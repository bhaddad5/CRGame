using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOfficeUiDisplay : MonoBehaviour, IUiDisplay
{
	[SerializeField] private Button BackButton;
	[SerializeField] private Transform TrophyCaseParent;
	[SerializeField] private Image BackgroundImage;

	[SerializeField] private TrophyDisplay TrophyDisplayPrefab;

	public void Setup(HudUiDisplay hudUi, MainGameManager mgm)
	{
		BackButton.onClick.AddListener(() => hudUi.ClosePlayerOffice());

		RefreshUiDisplay(mgm);
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		BackgroundImage.sprite = ImageLookup.Backgrounds.GetImage(mgm.GetPlayerOfficeBackgroundId());

		for (int i = 0; i < TrophyCaseParent.childCount; i++)
		{
			GameObject.Destroy(TrophyCaseParent.GetChild(i).gameObject);
		}

		foreach (var trophy in mgm.Data.GetOwnedTrophies())
		{
			var trophyOb = Instantiate(TrophyDisplayPrefab);
			trophyOb.Setup(trophy);
			trophyOb.transform.SetParent(TrophyCaseParent);
		}
	}
}
