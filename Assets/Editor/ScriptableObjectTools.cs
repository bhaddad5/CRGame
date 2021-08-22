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

		
		var dataPath = $"Assets/Data";
		foreach (var location in Data.Locations)
		{
			var locPath = $"{dataPath}/{location.Id}";
			Directory.CreateDirectory(locPath);

			foreach (var npc in location.Npcs)
			{
				var npcPath = $"{locPath}/{npc.Id}";
				Directory.CreateDirectory(npcPath);

				foreach (var interaction in npc.Interactions)
				{
					AssetDatabase.CreateAsset(interaction, $"{npcPath}/{interaction.Id}.asset");
				}

				AssetDatabase.CreateAsset(npc, $"{npcPath}/_{npc.Id}.asset");
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
	}
}
