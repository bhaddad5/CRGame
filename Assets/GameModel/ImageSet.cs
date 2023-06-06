using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public class ImageSet : ScriptableObject
	{
		[HideInInspector]
		public string Id;

		public string Name;

		public List<Texture2D> Images = new List<Texture2D>();
	}
}