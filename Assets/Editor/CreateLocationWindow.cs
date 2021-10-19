using System.Collections;
using System.Collections.Generic;
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
		loc.Id = locName;

		data.Locations.Add(loc);
		EditorUtility.SetDirty(data);

		AssetDatabase.CreateFolder($"Assets/Data", loc.Id);
		string locFolder = $"Assets/Data/{loc.Id}";
		AssetDatabase.CreateFolder(locFolder, "_Missions");
		AssetDatabase.CreateFolder(locFolder, "_Policies");

		AssetDatabase.CreateAsset(loc, $"Assets/Data/{loc.Id}/{loc.Id}.asset");
		AssetDatabase.SaveAssets();
	}

}