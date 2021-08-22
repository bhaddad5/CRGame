using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameModel
{
	public class Trait : ScriptableObject
	{
		public string Id;
		public string Name;

		public List<Effect> FreeEffects;
		public List<Effect> ControlledEffects;
	}
}
