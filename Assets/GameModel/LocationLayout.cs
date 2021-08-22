using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationLayout : ScriptableObject
{
	public float X = 0.5f;
	public float Y = 0.5f;
	public float Width = 200f;

	public static void ApplyLayout(RectTransform transform, LocationLayout layout)
	{
		var ratio = 2;
		transform.anchorMin = new Vector2(layout.X, layout.Y);
		transform.anchorMax = new Vector2(layout.X, layout.Y);
		transform.sizeDelta = new Vector2(layout.Width, layout.Width * ratio);
		transform.anchoredPosition = Vector2.zero;
	}
}