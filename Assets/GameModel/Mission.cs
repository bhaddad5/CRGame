using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	[CreateAssetMenu(fileName = "New Interaction", menuName = "Company Man Data/Mission", order = 10)]
	[Serializable]
	public class Mission : ScriptableObject
	{
		public string MissionName;
		[TextArea(15, 20)]
		public string MissionDescription;
		public Texture2D MissionImage;

		public Interaction CompletionInteractionReference;
		public List<Effect> Rewards;

		public bool IsComplete()
		{
			return (CompletionInteractionReference?.Completed ?? 0) > 0;
		}
	}
}
