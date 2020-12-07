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

	public void Setup(HudUiDisplay hudUi, MainGameManager mgm)
	{
		BackButton.onClick.AddListener(() => hudUi.ClosePlayerOffice());

		
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		BackgroundImage.sprite = BackgroundImagesLookup.GetBackgroundImage(mgm.GetPlayerOfficeBackgroundId());
	}
}
