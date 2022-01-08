using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public class MissionsPopupBindings : MonoBehaviour
	{
		[SerializeField] private MissionEntryBindings MissionPopupPrefab;
		[SerializeField] private Transform EntriesParent;

		public void Setup(Location location)
		{
			foreach (var mission in location.Missions)
			{
				var missionDisplay = GameObject.Instantiate(MissionPopupPrefab, EntriesParent);
				missionDisplay.Setup(mission);
			}
		}

		public void ClosePopup()
		{
			GameObject.Destroy(transform.parent.gameObject);
		}
	}
}