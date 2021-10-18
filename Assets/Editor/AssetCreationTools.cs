using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using GameModel.Serializers;
using UnityEditor;
using UnityEngine;

public class CreateNpcWindow : EditorWindow
{
	private static CreateNpcWindow window;

	[MenuItem("Tools/Create NPC")] 

	static void Init()
	{
		window = (CreateNpcWindow)EditorWindow.GetWindow(typeof(CreateNpcWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private string firstName;
	private string lastName;
	private GameData data;

	void OnGUI()
	{
		firstName = EditorGUILayout.TextField("First Name:", firstName);
		lastName = EditorGUILayout.TextField("Last Name:", lastName);

		GUILayout.Label("Location:", EditorStyles.boldLabel);
		foreach (var loc in data.Locations)
		{
			if (GUILayout.Button(loc.Name))
			{
				CreateNpc(loc);

				window.Close();
			}
		}
	}

	private void CreateNpc(Location loc)
	{
		Npc npc = ScriptableObject.CreateInstance<Npc>();
		npc.FirstName = firstName;
		npc.LastName = lastName;
		npc.Id = firstName + lastName;
		
		loc.Npcs.Add(npc);
		EditorUtility.SetDirty(loc);


		AssetDatabase.CreateFolder($"Assets/Data/{loc.Id}", npc.Id);
		AssetDatabase.CreateAsset(npc, $"Assets/Data/{loc.Id}/{npc.Id}/{npc.Id}.asset");
		string npcFolder = $"Assets/Data/{loc.Id}/{npc.Id}";
		AssetDatabase.CreateFolder(npcFolder, "Interactions");
		AssetDatabase.CreateFolder(npcFolder, "Pics-Controlled");
		AssetDatabase.CreateFolder(npcFolder, "Pics-Independent");
		AssetDatabase.CreateFolder(npcFolder, "Pics-Trained");
		AssetDatabase.CreateFolder(npcFolder, "Trophies");


		AssetDatabase.SaveAssets();
	}
}