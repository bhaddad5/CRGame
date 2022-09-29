using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiColorSchemeHandler : MonoBehaviour
{
	public ColorSchemesReference.ColorType Color;

	void Awake()
	{
		var colors = ColorSchemesReference.Instance.GetColorBlockFromType(Color);
		colors.colorMultiplier = 1f;

		if (gameObject.GetComponent<Selectable>() != null)
		{
			Debug.Log(colors);

			gameObject.GetComponent<Selectable>().colors = colors;
		}
		else
		{
			gameObject.GetComponent<Graphic>().color = colors.normalColor;
		}
	}
}