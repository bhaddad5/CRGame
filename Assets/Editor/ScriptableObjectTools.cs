using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using Assets.GameModel.Save;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectTools
{
	[MenuItem("Tools/Save Default Game Data")]
	public static void SaveGameState()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		string jsonData = SaveLoadHandler.SaveToJson(gameData);

		var savePath = Path.Combine(Application.streamingAssetsPath, "GameData.sav");
		var fs = File.Create(savePath);
		fs.Close();
		File.WriteAllText(savePath, jsonData);

		Debug.Log(jsonData);

		Debug.Log("Save Complete");
	}

	[MenuItem("Tools/Fix IDs")]
	public static void FixInteractions()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			if (location == null)
				continue;
			location.Id = Guid.NewGuid().ToString();
			EditorUtility.SetDirty(location);

			foreach (var npc in location.Npcs)
			{
				if (npc == null)
					continue;
				npc.Id = Guid.NewGuid().ToString();
				EditorUtility.SetDirty(npc);

				foreach (var interaction in npc.Interactions)
				{
					if (interaction == null)
						continue;

					interaction.Id = Guid.NewGuid().ToString();
					interaction.Completed = false;
					EditorUtility.SetDirty(interaction);
				}
			}
		}

		AssetDatabase.SaveAssets();
	}

	[MenuItem("Tools/Fix Folders")]
	public static void FixFolderNames()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			if (location == null)
				continue;

			string locFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(location));
			var folderName = Path.GetFileName(locFolder);
			if (folderName != location.Name.ToFolderName())
			{
				Directory.Move(locFolder, Path.Combine(Path.GetDirectoryName(locFolder), location.Name.ToFolderName()));
			}
			
			/*foreach (var npc in location.Npcs)
			{
				if (npc == null)
					continue;

				string npcFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(npc));
				if (Path.GetFileName(npcFolder) != (npc.FirstName + " " + npc.LastName))
				{
					Directory.Move(npcFolder, Path.Combine(Path.Combine(npcFolder, @"..\"), (npc.FirstName + " " + npc.LastName) + ".asset"));
				}
			}*/
		}
	}

	[MenuItem("Tools/Fix Files")]
	public static void FixFileNames()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var location in gameData.Locations)
		{
			if (location == null)
				continue;
			
			string locationFile = AssetDatabase.GetAssetPath(location);
			if (Path.GetFileNameWithoutExtension(locationFile) != location.Name)
			{
				File.Move(locationFile, Path.Combine(Path.GetDirectoryName(locationFile), location.Name + ".asset"));
			}

			/*foreach (var npc in location.Npcs)
			{
				if (npc == null)
					continue;
				
				string npcFile = AssetDatabase.GetAssetPath(npc);
				if (Path.GetFileNameWithoutExtension(npcFile) != (npc.FirstName + " " + npc.LastName))
				{
					File.Move(npcFile, Path.Combine(Path.GetDirectoryName(npcFile), (npc.FirstName + " " + npc.LastName) + ".asset"));
				}


				foreach (var interaction in npc.Interactions)
				{
					if (interaction == null)
						continue;

					string interactionFile = AssetDatabase.GetAssetPath(interaction);
					if (Path.GetFileNameWithoutExtension(interactionFile) != interaction.Name)
					{
						File.Move(interactionFile, Path.Combine(Path.GetDirectoryName(interactionFile), interaction.Name));
					}


				}
			}*/
		}
	}
}
