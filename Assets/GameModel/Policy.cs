using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class Policy : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;

		public Texture2D Image;

		[TextArea(15, 20)]
		public string Description;

		public ActionRequirements Requirements;
		public ActionCost Cost;

		public Effect Effect;

		[HideInInspector]
		public bool Active;

		public void Setup()
		{
			Active = false;
		}
	}
}