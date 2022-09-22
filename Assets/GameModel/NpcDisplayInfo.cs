using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
    [Serializable]
	public struct NpcLayout
	{
		public float xPos;
		public float yPos;
		public float width;

		public void ApplyToRectTransform(RectTransform rt)
		{
			var ratio = 2;
			rt.anchorMin = new Vector2(xPos, yPos);
			rt.anchorMax = new Vector2(xPos, yPos);
			rt.sizeDelta = new Vector2(width, width * ratio);
			rt.anchoredPosition = Vector2.zero;
		}
	}

	[Serializable]
	public struct NpcDisplayInfo
	{
		public Texture2D Picture;
		public Texture2D Background;
		public NpcLayout Layout;
	}
}
