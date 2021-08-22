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
		public Sprite CarImage;

		public string SuitsName;
		public Sprite SuitsImage;

		public Sprite JewleryCuffs;
		public Sprite JewleryPen;
		public Sprite JewleryRing;
		public Sprite JewleryWatch;
	}
}