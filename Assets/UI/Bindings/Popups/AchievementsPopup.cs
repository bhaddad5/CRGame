using Assets.GameModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsPopup : MonoBehaviour
{
	[SerializeField] private AchievementsPopupEntry AchievementPopupPrefab;
	[SerializeField] private Transform EntriesParent;

	public void Setup(MainGameManager mgm)
	{
		foreach (var achievement in mgm.Data.Achievements)
		{
			var entryDisplay = GameObject.Instantiate(AchievementPopupPrefab, EntriesParent);
			entryDisplay.Setup(achievement, mgm);
		}
	}

	public void ClosePopup()
	{
		GameObject.Destroy(transform.parent.gameObject);
	}
}
