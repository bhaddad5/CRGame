using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

		var foundNpc = npcPicker.Npc as Npc;

		if (foundNpc == null)
			return;

		foundNpc.Trophies.Add(trophy);
		EditorUtility.SetDirty(foundNpc);

		var npcFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(foundNpc));

		AssetDatabase.CreateAsset(trophy, $"{npcFolder}/Trophies/{trophy.Name.ToFolderName()}.asset");
		AssetDatabase.SaveAssets();
	}

}