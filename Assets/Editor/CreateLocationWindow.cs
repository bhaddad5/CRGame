using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateLocationWindow : EditorWindow
{
	private static CreateLocationWindow window;

	[MenuItem("Company Man/Create Location")]

	static void Init()
	{
		window = (CreateLocationWindow)EditorWindow.GetWindow(typeof(CreateLocationWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string locName;

	private RegionPicker regionPicker = new RegionPicker();

	private string errorMsg;

	void OnGUI()
	{
		locName = EditorGUILayout.TextField("Name:", locName);

		regionPicker.DrawRegionDropdown(data);

		if (GUILayout.Button("Create!"))
		{
			errorMsg = "";

			var reg = regionPicker.Region as Region;

			if (reg == null)
			{
				errorMsg = $"ERROR: YOU MUST SELECT A REGION";
			}

			CreateLocation(reg);

			window.Close();
		}

		if (!String.IsNullOrEmpty(errorMsg))
			GUILayout.Label(errorMsg, EditorStyles.boldLabel);
	}

	private void CreateLocation(Region region)
	{
		var regFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(region));

		Location loc = ScriptableObject.CreateInstance<Location>();
		loc.Name = locName;
		loc.Id = Guid.NewGuid().ToString();

		region.Locations.Add(loc);
		EditorUtility.SetDirty(region);

		string locFolder = Path.Combine($"{regFolder}", loc.Name.ToFolderName());
		AssetDatabase.CreateFolder($"{regFolder}", loc.Name.ToFolderName());
		AssetDatabase.CreateFolder(locFolder, "_Missions");
		AssetDatabase.CreateFolder(locFolder, "_Policies");

		AssetDatabase.CreateAsset(loc, $"{locFolder}/{loc.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}