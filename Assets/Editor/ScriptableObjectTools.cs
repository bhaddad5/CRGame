using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.GameModel;
using Assets.GameModel.XmlParsers;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectTools
{
	[MenuItem("Tools/Save GameData.xml To Scriptable Objects")]
	public static void SaveToScriptableObjects()
	{
		XmlResolver xmlResolver = new XmlResolver();
		
		var Data = xmlResolver.LoadXmlData(Path.Combine(Application.streamingAssetsPath, "GameData.xml"));

		foreach (var location in Data.Locations)
		{
			foreach (var npc in location.Npcs)
			{
				foreach (var interaction in npc.Interactions)
				{
					interaction.ResolveReferences(Data, npc);
				}
			}
		}

		var dataPath = $"Assets/Data";
		foreach (var location in Data.Locations)
		{
			var locPath = $"{dataPath}/{location.Id}";
			Directory.CreateDirectory(locPath);

			foreach (var npc in location.Npcs)
			{
				var npcPath = $"{locPath}/{npc.Id}";
				Directory.CreateDirectory(npcPath);

				var trophiesPath = $"{npcPath}/Trophies";
				Directory.CreateDirectory(trophiesPath);

				foreach (var trophy in npc.Trophies)
				{
					AssetDatabase.CreateAsset(trophy, $"{trophiesPath}/{trophy.Id}.asset");
				}

				var interactionsPath = $"{npcPath}/Interactions";
				Directory.CreateDirectory(interactionsPath);
				foreach (var interaction in npc.Interactions)
				{
					AssetDatabase.CreateAsset(interaction, $"{interactionsPath}/{interaction.Id}.asset");
				}

				AssetDatabase.CreateAsset(npc, $"{npcPath}/{npc.Id}.asset");
			}

			foreach (var mission in location.Missions)
			{
				AssetDatabase.CreateAsset(mission, $"{locPath}/{mission.MissionName}.asset");
			}

			foreach (var policy in location.Policies)
			{
				AssetDatabase.CreateAsset(policy, $"{locPath}/{policy.Id}.asset");
			}

			AssetDatabase.CreateAsset(location, $"{locPath}/{location.Id}.asset");
		}

		AssetDatabase.CreateAsset(Data, "Assets/Data/GameData.asset");

		Debug.Log("Conversion complete!");
	}


	[MenuItem("Tools/Save GameData to Json")]
	public static void WriteGateDataToJson()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		var jsonData = JsonUtility.ToJson(gameData);

		var savePath = Path.Combine(Application.streamingAssetsPath, "GameData.sav");
		var fs = File.Create(savePath);
		fs.Close();
		File.WriteAllText(savePath, jsonData);

		Debug.Log("Save Complete");
	}
}
