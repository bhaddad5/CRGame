using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUiDisplay : MonoBehaviour, IUiDisplay
{
	[SerializeField] private TMP_Text Title;
	[SerializeField] private TMP_Text Description;
	[SerializeField] private Image Image;
	[SerializeField] private GameObject Complete;

	private Mission mission;
	private Department dept;

	public void Setup(Mission mission, Department dept, MainGameManager mgm)
	{
		this.mission = mission;
		this.dept = dept;
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		Title.text = $"{mission.MissionName}";
		Description.text = $"{mission.MissionDescription}";
		Image.sprite = mission.MissionImage;
		Complete.SetActive(mission.IsComplete(mgm));
	}
}
