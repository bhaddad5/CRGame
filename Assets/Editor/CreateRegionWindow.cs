using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateRegionWindow : EditorWindow
{
	private static CreateRegionWindow window;

	[MenuItem("Company Man/Create Region")]

	static void Init()
	{
		window = (CreateRegionWindow)EditorWindow.GetWindow(typeof(CreateRegionWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string locName;

	void OnGUI()
	{
		locName = EditorGUILayout.TextField("Name:", locName);

		if (GUILayout.Button("Create!"))
		{
			CreateRegion();

			window.Close();
		}
	}

	private void CreateRegion()
	{
		Region region = ScriptableObject.CreateInstance<Region>();
		region.Name = locName;
		region.Id = Guid.NewGuid().ToString();

		data.Regions.Add(region);
		EditorUtility.SetDirty(data);

		string locFolder = Path.Combine($"Assets/Data", region.Name.ToFolderName());
		AssetDatabase.CreateFolder($"Assets/Data", region.Name.ToFolderName());

		AssetDatabase.CreateAsset(region, $"{locFolder}/{region.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}
}
