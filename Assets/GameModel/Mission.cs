using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	public class Mission : ScriptableObject
	{
		public string MissionName;
		[TextArea(15, 20)]
		public string MissionDescription;
		public Sprite MissionImage;

		public Interaction CompletionInteractionReference;
		public List<Effect> Rewards;

		public bool IsComplete()
		{
			return CompletionInteractionReference?.Completed ?? true;
		}
	}
}
