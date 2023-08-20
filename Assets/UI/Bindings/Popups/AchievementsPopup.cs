using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsPopup : MonoBehaviour
{
	[SerializeField] private AchievementsPopupEntry AchievementPopupPrefab;
	[SerializeField] private Transform EntriesParent;

	public void Setup(List<Achievement> achievements)
	{
		foreach (var achievement in achievements)
		{
			var entryDisplay = GameObject.Instantiate(AchievementPopupPrefab, EntriesParent);
			entryDisplay.Setup(achievement);
		}
	}

	public void ClosePopup()
	{
		GameObject.Destroy(transform.parent.gameObject);
	}
}
