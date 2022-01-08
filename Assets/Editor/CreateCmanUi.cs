using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CreateCmanUi
{
	[MenuItem("GameObject/UI/Button - CMan", false, 0)]
	public static void AddButton(MenuCommand cmd)
	{
		var cmanButton = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UI/CMan Button.prefab");
		GameObject.Instantiate(cmanButton, Selection.activeTransform);
	}

	[MenuItem("GameObject/UI/Panel - CMan", false, 1)]
	public static void AddPanel(MenuCommand cmd)
	{
		var cmanButton = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UI/CMan Panel.prefab");
		GameObject.Instantiate(cmanButton, Selection.activeTransform);
	}
}
