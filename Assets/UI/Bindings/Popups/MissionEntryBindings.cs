using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionEntryBindings : MonoBehaviour
{
	[SerializeField] private TMP_Text Title;
	[SerializeField] private TMP_Text Description;
	[SerializeField] private Image Image;
	[SerializeField] private GameObject Complete;
	
	public void Setup(Mission mission)
	{
		Title.text = $"{mission.MissionName}";
		Description.text = $"{mission.MissionDescription}";
		Image.sprite = mission.MissionImage.ToSprite();
		Complete.SetActive(mission.Completed);
	}
}
