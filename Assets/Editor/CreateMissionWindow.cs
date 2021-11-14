using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateMissionWindow : EditorWindow
{
	private static CreateMissionWindow window;

	[MenuItem("Company Man/Create Mission")]

	static void Init()
	{
		window = (CreateMissionWindow)EditorWindow.GetWindow(typeof(CreateMissionWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string missionName;
	private LocationPicker locPicker = new LocationPicker();

	void OnGUI()
	{
		missionName = EditorGUILayout.TextField("Name:", missionName);

		locPicker.DrawLocationDropdown(data);

		if (GUILayout.Button("Create!"))
		{
			Create();

			window.Close();
		}
	}

	private void Create()
	{
		Mission mission = ScriptableObject.CreateInstance<Mission>();
		mission.MissionName = missionName;

		var foundLoc = data.FindLocation(locPicker.LocationId);

		if (foundLoc == null)
			return;


		foundLoc.Missions.Add(mission);
		EditorUtility.SetDirty(foundLoc);

		AssetDatabase.CreateAsset(mission, $"Assets/Data/{foundLoc.Id}/_Missions/{mission.MissionName}.asset");
		AssetDatabase.SaveAssets();
	}

}