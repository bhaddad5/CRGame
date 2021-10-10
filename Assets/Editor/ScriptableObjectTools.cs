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

		SerializedGameData serializedData = SerializedGameData.Serialize(gameData);

		foreach (var location in serializedData.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					foreach (var interactionResult in interaction.InteractionResults)
					{
						foreach (var dialog in interactionResult.Dialogs)
						{
							if (dialog.NpcImage != null)
							{

							}
						}
					}
				}
			}

			//var locationImage = location.BackgroundImage;
			//File.Move(Path.Combine(Application.dataPath, "Resources", "OfficePics", locationImage+".png"), Path.Combine(Application.dataPath, "Data", ));
		}
		
		Debug.Log("Move Complete");
	}
}
