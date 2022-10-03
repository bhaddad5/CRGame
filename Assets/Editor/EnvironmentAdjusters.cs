using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EnvironmentAdjusters
{
	[MenuItem("Company Man 3D/Fix Positions", false, 0)]
	public static void SnapEnvironmentPositions()
	{
		if (Selection.activeTransform == null)
		{
			Debug.LogError("Select a transform to adjust");
			return;
		}

		SnapTransformPositionsRecursive(Selection.activeTransform);
		EditorUtility.SetDirty(Selection.activeTransform);
		Debug.Log("Finished!");

	}

	private static void SnapTransformPositionsRecursive(Transform t)
	{
		if (t.name.ToLowerInvariant().Equals("props"))
			return;

		t.localPosition = new Vector3(
			Mathf.Round(t.localPosition.x/5) * 5f,
			Mathf.Round(t.localPosition.y/3) * 3f,
			Mathf.Round(t.localPosition.z/5) * 5f);
		for (int i = 0; i < t.childCount; i++)
			SnapTransformPositionsRecursive(t.GetChild(i));
	}
}
