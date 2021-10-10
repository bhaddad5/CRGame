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
}
