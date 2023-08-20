using Assets.GameModel;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsPopupEntry : MonoBehaviour
{
	[SerializeField] private TMP_Text Title;
	[SerializeField] private TMP_Text Description;
	[SerializeField] private Image Image;

	[SerializeField] private Sprite NotYetAchievedImage;

	public void Setup(Achievement achievement)
	{
		Title.text = $"{achievement.Name}";
		Description.text = $"{achievement.Description}";

		if (achievement.Completed)
			Image.sprite = achievement.Image.ToSprite();
		else
			Image.sprite = NotYetAchievedImage;
	}
}
