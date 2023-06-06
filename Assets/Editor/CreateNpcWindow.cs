using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

public class CreateNpcWindow : EditorWindow
{
	private static CreateNpcWindow window;

	[MenuItem("Company Man/Create NPC")] 

	static void Init()
	{
		window = (CreateNpcWindow)EditorWindow.GetWindow(typeof(CreateNpcWindow));
		window.data = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
		window.Show();
	}

	private GameData data;

	private string firstName;
	private string lastName;

	private string startingPicsSrc;

	private LocationPicker locPicker = new LocationPicker();

	private string errorMsg;

	void OnGUI()
	{
		firstName = EditorGUILayout.TextField("First Name:", firstName);
		lastName = EditorGUILayout.TextField("Last Name:", lastName);

		EditorGUILayout.PrefixLabel($"Starting Pics:{startingPicsSrc}");
		if (GUILayout.Button("Choose Starting Pics Source Folder"))
		{
			startingPicsSrc = EditorUtility.OpenFolderPanel("Choose Pics Source Folder", "", "");
		}

		locPicker.DrawLocationDropdown(data);

		GUILayout.Space(10);

		GUILayout.Label($"Finish:", EditorStyles.boldLabel);

		if (GUILayout.Button("Create NPC"))
		{
			var loc = locPicker.Location as Location;

			errorMsg = "";

			if (firstName == null || lastName == null)
			{
				errorMsg = $"ERROR: NPC not fully named!";
			}
			else if (loc == null)
			{
				errorMsg = $"ERROR: YOU MUST SELECT A LOCATION";
			}
			else
			{
				CreateNpc(loc);

				window.Close();
			}

			
		}

		if(!String.IsNullOrEmpty(errorMsg))
			GUILayout.Label(errorMsg, EditorStyles.boldLabel);
	}
	
	private void CreateNpc(Location loc)
	{
		Npc npc = ScriptableObject.CreateInstance<Npc>();
		npc.FirstName = firstName;
		npc.LastName = lastName;
		npc.Id = Guid.NewGuid().ToString();

		loc.Npcs.Add(npc);
		EditorUtility.SetDirty(loc);

		var locFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(loc));

		AssetDatabase.CreateFolder($"{locFolder}", npc.NpcFileName().ToFolderName());
		string npcFolder = Path.Combine(locFolder, npc.NpcFileName().ToFolderName());
		AssetDatabase.CreateFolder(npcFolder, "Interactions");
		AssetDatabase.CreateFolder(npcFolder, "Trophies");

		AssetDatabase.CreateAsset(npc, $"{npcFolder}/{npc.NpcFileName().ToFolderName()}.asset");
		AssetDatabase.SaveAssets();

		var startingImageSet = CreateImageSetWindow.CreateImageSet("Starting Pics", startingPicsSrc, npc);
		npc.StartingImageSets = new List<ImageSet>() { startingImageSet };
		EditorUtility.SetDirty(npc);
		AssetDatabase.SaveAssets();
	}
}