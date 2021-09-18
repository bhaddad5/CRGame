using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class Trophy : ScriptableObject
	{
		public string Id;
		public string Name;
		public Texture2D Image;
		public bool Owned;
	}
}