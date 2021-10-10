using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using GameModel.Serializers;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectTools
{
	[MenuItem("Tools/Generate Default Save")]
	public static void WriteGateDataToJson()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		SerializedGameData serializedData = SerializedGameData.Serialize(gameData);

		var jsonData = JsonUtility.ToJson(serializedData);

		var savePath = Path.Combine(Application.streamingAssetsPath, "GameData.sav");
		var fs = File.Create(savePath);
		fs.Close();
		File.WriteAllText(savePath, jsonData);

		Debug.Log("Save Complete");
	}

	[MenuItem("Tools/Reorg Pics")]
	public static void MovePicsToFolders()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");


		foreach (var location in gameData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					foreach (var interactionResult in interaction.InteractionResults)
					{
						foreach (var dialog in interactionResult.Dialogs)
						{
							/*if (!String.IsNullOrEmpty(dialog.NpcImage))
							{
								string currImagePath = Path.Combine(Application.dataPath, "Resources", "NpcPics", $"{npc.Id}", dialog.NpcImage + "_1.png");

								if (!File.Exists(currImagePath))
								{
									Debug.LogError($"Shit, {dialog.NpcImage} either doesn't exist or was moved already");
									continue;
								}

								string destPath = Path.Combine(Application.dataPath, "Data", $"{location.Id}", $"{npc.Id}", "Interactions", interaction.Id + ".png");

								File.Move(currImagePath, destPath);

								//var texRef = AssetDatabase.LoadAssetAtPath<Texture2D>(destPath);

								//dialog.CustomNpcImage = texRef;
							}*/
						}
					}
				}
			}
		}
		
		Debug.Log("Move Complete");
	}
}
