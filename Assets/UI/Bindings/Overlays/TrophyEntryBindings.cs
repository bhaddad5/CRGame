using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophyEntryBindings : MonoBehaviour
{
	[SerializeField] private Image TrophyImage;
	[SerializeField] private TMP_Text TrophyName;

	public void Setup(Trophy trophy)
	{
		TrophyImage.sprite = trophy.Image.ToSprite();
		TrophyName.text = trophy.Name;
	}
}
