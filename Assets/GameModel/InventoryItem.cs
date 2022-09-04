using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class InventoryItem : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;
		public Texture2D Image;

		[TextArea(3, 10)]
		public string Description;
	}
}
