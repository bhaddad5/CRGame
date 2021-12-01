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
			if (Path.GetFileName(locFolder) != location.Name)
			{
				string locFolderParent = Path.Combine(locFolder, @"..\");
				Directory.Move(locFolder, Path.Combine(locFolderParent, location.Name));
			}

			

			


			/*foreach (var npc in location.Npcs)
			{
				if (npc == null)
					continue;
				

				foreach (var interaction in npc.Interactions)
				{
					if (interaction == null)
						continue;
					
				}
			}*/
		}
	}
}
