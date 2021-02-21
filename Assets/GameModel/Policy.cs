using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	public class Policy
	{
		public string Id;
		public string Name;

		public bool Active;
		public Sprite Image;
		public string Description;

		public ActionRequirements Requirements;
		public List<Effect> Effects;
	}
}