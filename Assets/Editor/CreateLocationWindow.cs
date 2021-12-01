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

	void OnGUI()
	{
		locName = EditorGUILayout.TextField("Name:", locName);

		if (GUILayout.Button("Create!"))
		{
			CreateLocation();

			window.Close();
		}
	}

	private void CreateLocation()
	{
		Location loc = ScriptableObject.CreateInstance<Location>();
		loc.Name = locName;
		loc.Id = Guid.NewGuid().ToString();

		data.Locations.Add(loc);
		EditorUtility.SetDirty(data);

		string locFolder = Path.Combine($"Assets/Data", loc.Name.ToFolderName());
		AssetDatabase.CreateFolder($"Assets/Data", loc.Name.ToFolderName());
		AssetDatabase.CreateFolder(locFolder, "_Missions");
		AssetDatabase.CreateFolder(locFolder, "_Policies");

		AssetDatabase.CreateAsset(loc, $"{locFolder}/{loc.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}