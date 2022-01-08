using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public class PoliciesPopupBindings : MonoBehaviour
	{
		[SerializeField] private PolicyEntryBindings PolicyPopupPrefab;
		[SerializeField] private Transform EntriesParent;

		public void Setup(Location location, MainGameManager mgm)
		{
			foreach (var policy in location.Policies)
			{
				var missionDisplay = GameObject.Instantiate(PolicyPopupPrefab, EntriesParent);
				missionDisplay.Setup(policy, mgm);
			}
		}

		public void ClosePopup()
		{
			GameObject.Destroy(transform.parent.gameObject);
		}
	}
}