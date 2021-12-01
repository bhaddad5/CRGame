using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	[CreateAssetMenu(fileName = "New Interaction", menuName = "Company Man Data/Policy", order = 11)]
	[Serializable]
	public class Policy : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;

		public bool Active;
		public Texture2D Image;

		[TextArea(15, 20)]
		public string Description;

		public ActionRequirements Requirements;
		public ActionCost Cost;
		public List<Effect> Effects;
	}
}