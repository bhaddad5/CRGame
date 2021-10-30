using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using GameModel.Serializers;
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

	private string picsSrcInd;
	private string picsSrcCtrl;
	private string picsSrcTrn;

	private string department;

	private string errorMsg;

	void OnGUI()
	{
		firstName = EditorGUILayout.TextField("First Name:", firstName);
		lastName = EditorGUILayout.TextField("Last Name:", lastName);

		EditorGUILayout.PrefixLabel($"Independent Pics:{picsSrcInd}");
		if (GUILayout.Button("Choose Independent Pics Source Folder"))
		{
			picsSrcInd = EditorUtility.OpenFolderPanel("Choose Pics Source Folder", "", "");
		}

		EditorGUILayout.PrefixLabel($"Controlled Pics:{picsSrcCtrl}");
		if (GUILayout.Button("Choose Controlled Pics Source Folder"))
		{
			picsSrcCtrl = EditorUtility.OpenFolderPanel("Choose Pics Source Folder", "", "");
		}

		EditorGUILayout.PrefixLabel($"Trained Pics:{picsSrcTrn}");
		if (GUILayout.Button("Choose Trained Pics Source Folder"))
		{
			picsSrcTrn = EditorUtility.OpenFolderPanel("Choose Pics Source Folder", "", "");
		}

		DrawDropdown();

		GUILayout.Space(10);

		GUILayout.Label($"Finish:", EditorStyles.boldLabel);

		if (GUILayout.Button("Create NPC"))
		{
			var loc = data.Locations.FirstOrDefault(l => l.Name == department);

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

	private void DrawDropdown()
	{
		GUILayout.Label($"Location:", EditorStyles.boldLabel);

		var content = new GUIContent($"{department}");
		var dropdownPos = GUILayoutUtility.GetRect(content, GUIStyle.none);
		if (!EditorGUILayout.DropdownButton(content, FocusType.Passive))
		{
			return;
		}


		void handleItemClicked(object dept)
		{
			department = (dept as Location).Name;
		}

		GenericMenu menu = new GenericMenu();
		foreach (var loc in data.Locations)
		{
			if (loc == null)
				continue;

			menu.AddItem(new GUIContent(loc.Name), false, handleItemClicked, loc);
		}

		menu.DropDown(dropdownPos);
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
		string npcFolder = $"Assets/Data/{loc.Id}/{npc.Id}";
		AssetDatabase.CreateFolder(npcFolder, "Interactions");
		AssetDatabase.CreateFolder(npcFolder, "Pics-Controlled");
		AssetDatabase.CreateFolder(npcFolder, "Pics-Independent");
		AssetDatabase.CreateFolder(npcFolder, "Pics-Trained");
		AssetDatabase.CreateFolder(npcFolder, "Trophies");
		
		npc.IndependentImages = CopyPicsIntoFolder(picsSrcInd, $"{npcFolder}/Pics-Independent");
		npc.ControlledImages = CopyPicsIntoFolder(picsSrcCtrl, $"{npcFolder}/Pics-Controlled");
		npc.TrainedImages = CopyPicsIntoFolder(picsSrcTrn, $"{npcFolder}/Pics-Trained");

		AssetDatabase.CreateAsset(npc, $"Assets/Data/{loc.Id}/{npc.Id}/{npc.Id}.asset");
		AssetDatabase.SaveAssets();
	}

	private List<Texture2D> CopyPicsIntoFolder(string srcFolder, string relativeDestFolder)
	{
		List<Texture2D> res = new List<Texture2D>();

		if (String.IsNullOrEmpty(srcFolder))
			return res;

		foreach (var file in Directory.GetFiles(srcFolder, "*.png"))
		{
			//Chop "/Assets" off the end, it's already in the relative path
			var rootGameFolder = Application.dataPath.Substring(0, Application.dataPath.Length - 7);
			var relativeFilePath = Path.Combine(relativeDestFolder, Path.GetFileName(file));
			var destPath = Path.Combine(rootGameFolder, relativeFilePath);
			File.Copy(file, destPath);

			AssetDatabase.ImportAsset(relativeFilePath);
			var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(relativeFilePath);
			Debug.Assert(tex != null);
			res.Add(tex);
		}
		
		return res;
	}
}