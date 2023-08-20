using System;
using Assets.GameModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAchievementWindow : EditorWindow
{
	private static CreateAchievementWindow window;

	[MenuItem("Company Man/Create Achievement")]

	static void Init()
	{
		window = (CreateAchievementWindow)EditorWindow.GetWindow(typeof(CreateAchievementWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string achievementName;

	void OnGUI()
	{
		achievementName = EditorGUILayout.TextField("Name:", achievementName);

		if (GUILayout.Button("Create!"))
		{
			Create();

			window.Close();
		}
	}

	private void Create()
	{
		Achievement achievement = ScriptableObject.CreateInstance<Achievement>();
		achievement.Name = achievementName;
		achievement.Id = Guid.NewGuid().ToString();
		
		data.Achievements.Add(achievement);
		EditorUtility.SetDirty(data);

		var locFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(data));

		AssetDatabase.CreateAsset(achievement, $"{locFolder}/_Achievements/{achievement.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}