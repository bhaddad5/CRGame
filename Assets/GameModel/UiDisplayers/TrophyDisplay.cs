using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophyDisplay : MonoBehaviour
{
	[SerializeField] private Image TrophyImage;
	[SerializeField] private TMP_Text TrophyName;

	public void Setup(Trophy trophy)
	{
		TrophyImage.sprite = trophy.Image;
		TrophyName.text = trophy.Name;
	}
}
