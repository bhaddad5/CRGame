using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;

public class CreateTrophyWindow : EditorWindow
{
	private static CreateTrophyWindow window;

	[MenuItem("Company Man/Create Trophy")]

	static void Init()
	{
		window = (CreateTrophyWindow)EditorWindow.GetWindow(typeof(CreateTrophyWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;
	private string trophyName;
	private NpcPicker npcPicker = new NpcPicker();

	void OnGUI()
	{
		trophyName = EditorGUILayout.TextField("Name:", trophyName);

		npcPicker.DrawNpcDropdown(data);

		if (GUILayout.Button("Create!"))
		{
			CreateInteraction();

			window.Close();
		}
	}

	private void CreateInteraction()
	{
		Trophy trophy = ScriptableObject.CreateInstance<Trophy>();
		trophy.Name = trophyName;
		trophy.Id = Guid.NewGuid().ToString();

		var foundNpc = data.FindNpc(npcPicker.NpcId);

		if (foundNpc == null)
			return;


		foundNpc.Item2.Trophies.Add(trophy);
		EditorUtility.SetDirty(foundNpc.Item2);

		AssetDatabase.CreateAsset(trophy, $"Assets/Data/{foundNpc.Item1.Name}/{foundNpc.Item2.NpcFileName()}/Trophies/{trophy.Name}.asset");
		AssetDatabase.SaveAssets();
	}

}