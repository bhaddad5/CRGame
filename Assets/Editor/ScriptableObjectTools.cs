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
				//string currImagesPath = Path.Combine(Application.dataPath, "Resources", "NpcPics", $"{npc.Id}");

				string independentDestPath = Path.Combine("Data", $"{location.Id}", $"{npc.Id}", "Pics-Independent");
				string controlledDestPath = Path.Combine("Data", $"{location.Id}", $"{npc.Id}", "Pics-Controlled");
				string trainedDestPath = Path.Combine("Data", $"{location.Id}", $"{npc.Id}", "Pics-Trained");

				npc.IndependentImages = new List<Texture2D>();
				npc.ControlledImages = new List<Texture2D>();
				npc.TrainedImages = new List<Texture2D>();

				foreach (var pic in Directory.GetFiles(Path.Combine(Application.dataPath, independentDestPath), "*.png"))
				{
					var picRef = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine("Assets", independentDestPath, Path.GetFileName(pic)));
					npc.IndependentImages.Add(picRef);
				}

				foreach (var pic in Directory.GetFiles(Path.Combine(Application.dataPath, controlledDestPath), "*.png"))
				{
					var picRef = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine("Assets", controlledDestPath, Path.GetFileName(pic)));
					npc.ControlledImages.Add(picRef);
				}

				foreach (var pic in Directory.GetFiles(Path.Combine(Application.dataPath, trainedDestPath), "*.png"))
				{
					var picRef = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine("Assets", trainedDestPath, Path.GetFileName(pic)));
					npc.TrainedImages.Add(picRef);
				}

				EditorUtility.SetDirty(npc);
			}
		}
		
		Debug.Log("Move Complete");
	}
}
