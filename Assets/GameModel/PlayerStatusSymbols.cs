using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel
{
	[Serializable]
	public struct PlayerStatusSymbols
	{
		public string CarName;
		public Texture2D CarImage;

		public string SuitsName;
		public Texture2D SuitsImage;

		public Texture2D JewleryCuffs;
		public Texture2D JewleryPen;
		public Texture2D JewleryRing;
		public Texture2D JewleryWatch;
	}
}