using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSchemesReference : MonoBehaviour
{
	public enum ColorType
	{
		PanelBacking,
		PanelContent,
		ButtonBacking,
		ButtonContent,
		TransPanelBacking,
		TransPanelContent,
		DialogChoice
	}

	[Serializable]
	public struct ColorTypeDefinition
	{
		public ColorType ColorType;
		public ColorBlock Colors;
	}

	[SerializeField] private List<ColorTypeDefinition> ColorTypeDefinitions = new List<ColorTypeDefinition>();

	public static ColorSchemesReference Instance
	{
		get
		{
			if (instance == null)
				instance = Resources.Load<ColorSchemesReference>("UiColorSchemesReference");
			return instance;
		}
	}

	private static ColorSchemesReference instance;

	public ColorBlock GetColorBlockFromType(ColorType colorType)
	{
		foreach (var definition in ColorTypeDefinitions)
		{
			if (definition.ColorType == colorType)
				return definition.Colors;
		}

		throw new Exception($"Color type {colorType} has no defined color block in the UiDefinitions gameObject of the main scene.");
	}
}
