using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public static class ProfilingHelpers
{
	#region Helpers

	private static GameData LoadGameData() => AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

	private static List<Npc> GetAllNpcs()
	{
		var gameData = LoadGameData();
		List<Npc> res = new List<Npc>();

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					if (npc != null)
						res.Add(npc);
				}
			}
		}

		return res;
	}

	public static List<Mission> GetAllMissions()
	{
		var gameData = LoadGameData();

		var res = new List<Mission>();
		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var m in location.Missions)
				{
					res.Add(m);
				}
			}
		}
			

		return res;
	}

	public static List<Policy> GetAllPolicies()
	{
		var gameData = LoadGameData();

		var res = new List<Policy>();

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var p in location.Policies)
				{
					res.Add(p);
				}
			}
		}

		return res;
	}

	public static List<Interaction> GetAllInteractions()
	{
		var gameData = LoadGameData();

		var interactions = new List<Interaction>();

		foreach (var startOfTurnInteraction in gameData.StartOfTurnInteractions)
		{
			if (startOfTurnInteraction == null)
				continue;
			interactions.Add(startOfTurnInteraction);
		}

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					foreach (var interaction in npc.Interactions)
					{
						if (interaction == null)
							continue;
						interactions.Add(interaction);
					}
				}
			}
		}
			

		return interactions;
	}

	public static List<KeyValuePair<string, Effect>> GetAllEffects()
	{
		var gameData = LoadGameData();

		List<KeyValuePair<string, Effect>> effects = new List<KeyValuePair<string, Effect>>();

		foreach (var interaction in GetAllInteractions())
		{
			effects.Add(new KeyValuePair<string, Effect>($"Interaction(\"{interaction.Name}\")", interaction.Result.Effect));
			if (interaction.CanFail)
				effects.Add(new KeyValuePair<string, Effect>($"InteractionFailed(\"{interaction.Name}\")", interaction.FailureResult.Effect));
		}

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var mission in location.Missions)
				{
					effects.Add(new KeyValuePair<string, Effect>($"Mission(\"{mission.MissionName}\")", mission.Effect));
				}

				foreach (var policy in location.Policies)
				{
					effects.Add(new KeyValuePair<string, Effect>($"Policy(\"{policy.Name}\")", policy.Effect));
				}
			}
		}

		return effects;
	}

	#endregion

	[MenuItem("Company Man Debugging/Print Video Lengths")]
	public static void PrintVideoLengths()
	{
		int numOfVideos = 0;
		double totalVideoLength = 0;
		double videoLengthCapped = 0;
		foreach (var interaction in GetAllInteractions())
		{
			foreach (var popup in interaction.Result.OptionalPopups)
			{
				foreach (var videoClip in popup.Videos)
				{
					if (videoClip == null)
						continue;
					numOfVideos++;
					totalVideoLength += videoClip.length;
					videoLengthCapped += Mathf.Min((float)videoClip.length, 20f);
					//if(videoClip.length > 20)
					//	Debug.Log($"{videoClip}: {videoClip.length} seconds, frame size {videoClip.width} {videoClip.height}");
				}
			}
		}

		Debug.Log($"Number of videos: {numOfVideos}");
		Debug.Log($"Total length of all videos: {totalVideoLength}");
		Debug.Log($"Total length of all videos if capped: {videoLengthCapped}");
	}

	[MenuItem("Company Man Debugging/Print Npc Control Interactions")]
	public static void PrintNpcControlInteractions()
	{
		var effects = GetAllEffects();
		var npcs = GetAllNpcs();

		Dictionary<Npc, List<string>> npcControllers = new Dictionary<Npc, List<string>>();

		foreach (var effect in effects)
		{
			foreach (var npc in effect.Value.NpcsToControl)
			{
				if(npc == null)
					continue;

				if (!npcControllers.ContainsKey(npc))
					npcControllers[npc] = new List<string>();
			}
		}

		foreach (var npc in npcs)
		{
			if(npc.IsControllable && !npcControllers.ContainsKey(npc))
				Debug.LogError($"{npc.FirstName} {npc.LastName} has no Control effect!");
		}

		foreach (var npcController in npcControllers)
		{
			string res = $"{npcController.Key.FirstName} {npcController.Key.LastName} controlled by:";
			foreach (var completer in npcController.Value)
			{
				res += $"{completer}, or";
			}

			res = res.Substring(0, res.Length - 4);
			Debug.Log(res);
		}
	}

	[MenuItem("Company Man Debugging/Print Mission Completion Interactions")]
	public static void PrintMissionCompletionInteractions()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		Dictionary<Mission, List<string>> missionCompleters = new Dictionary<Mission, List<string>>();

		var effects = GetAllEffects();
		foreach (var effect in effects)
		{
			foreach (var mission in effect.Value.MissionsToComplete)
			{
				if (mission == null)
					continue;

				if (!missionCompleters.ContainsKey(mission))
					missionCompleters[mission] = new List<string>();
				missionCompleters[mission].Add(effect.Key);
			}
		}


		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var mission in location.Missions)
				{
					if (!missionCompleters.ContainsKey(mission))
						Debug.LogError($"{mission.MissionName} has no completion effect!");
				}
			}
		}
			

		foreach (var missionCompleter in missionCompleters)
		{
			string res = $"{missionCompleter.Key.MissionName} completed by:";
			foreach (var completer in missionCompleter.Value)
			{
				res += $"{completer}, or";
			}

			res = res.Substring(0, res.Length - 4);
			Debug.Log(res);
		}
	}


	[MenuItem("Company Man Debugging/Get Control Interactions Of Selected Object")]
	public static void GetLocationInteractions()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		var loc = Selection.activeObject as Location;

		var selectedNpc = Selection.activeObject as Npc;

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					foreach (var interaction in npc.Interactions)
					{
						if (interaction.Result.Effect.LocationsToControl.Contains(loc))
							Debug.Log($"Controlled by: {interaction}");

						if (interaction.Result.Effect.NpcsToControl.Contains(selectedNpc))
							Debug.Log($"Controlled by: {interaction}");

						if (interaction.Result.Effect.NpcsToRemoveFromGame.Contains(selectedNpc))
							Debug.Log($"Removed From Game by: {interaction}");
					}

				}
			}
		}
			
	}

	[MenuItem("Company Man Debugging/Find Impossible Sub Interactions")]
	public static void FindImpossibleSubInteractions()
	{
		foreach (var interaction in GetAllInteractions())
		{
			foreach (var choice in interaction.Result.Choices)
			{
				if(choice.Requirements.RequiredInteractions.Contains(interaction))
					Debug.LogError($"{choice.name} requires it's parent interaction, {interaction.name}");
			}
		}
	}

	[MenuItem("Company Man Debugging/Print Control and Completion Interactions")]
	public static void PrintControlInteractions()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					foreach (var interaction in npc.Interactions)
					{
						foreach (var ob in interaction.Result.Effect.NpcsToControl)
							Debug.Log($"{ob} controlled by {interaction}");
						foreach (var ob in interaction.Result.Effect.NpcsToTrain)
							Debug.Log($"{ob} trained by {interaction}");
						foreach (var ob in interaction.Result.Effect.NpcsToRemoveFromGame)
							Debug.Log($"{ob} removed from game by {interaction}");
						foreach (var ob in interaction.Result.Effect.LocationsToControl)
							Debug.Log($"{ob} controlled by {interaction}");
						foreach (var ob in interaction.Result.Effect.TrophiesClaimedReferences)
							Debug.Log($"{ob} claimed by {interaction}");
						foreach (var ob in interaction.Result.Effect.MissionsToComplete)
							Debug.Log($"{ob} completed by {interaction}");
					}
				}
			}
		}
			
	}

	[MenuItem("Company Man Debugging/Copy All Text")]
	public static void CopyAllText()
	{
		var gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");

		string AllText = "";

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				foreach (var npc in location.Npcs)
				{
					AllText += $"NPC BIO - {npc.name}: {npc.Bio}\n\n";

					foreach (var interaction in npc.Interactions)
					{
						string interactionText = $"INTERACTION - {interaction.name}:\n";
						foreach (var dialog in interaction.Result.Dialogs)
						{
							interactionText += $"{dialog.Text}\n";
						}

						foreach (var popup in interaction.Result.OptionalPopups)
						{
							interactionText += $"POPUP: {popup.Text}";
						}

						if (interaction.CanFail)
							interactionText += $"Fail Result:\n";
						foreach (var dialog in interaction.FailureResult.Dialogs)
						{
							interactionText += $"{dialog.Text}\n";
						}



						AllText += $"{interactionText}\n\n";
					}
				}

				foreach (var policy in location.Policies)
				{
					AllText += $"POLICY - {policy.name}: {policy.Description}\n\n";
				}

				foreach (var mission in location.Missions)
				{
					AllText += $"MISSION - {mission.name}: {mission.MissionDescription}\n\n";
				}
			}
		}
			

		GUIUtility.systemCopyBuffer = AllText;
		Debug.Log("Copy Complete!  Paste it anywhere");
	}

	[MenuItem("Company Man Debugging/Resource Calculators/Calculate Power Totals")]
	public static void CalculatePowerTotals()
	{
		CalculateResourceTotals("Power", effect => effect.PowerEffect);
	}

	[MenuItem("Company Man Debugging/Resource Calculators/Calculate Revanue Totals")]
	public static void CalculateRevanueTotals()
	{
		CalculateResourceTotals("Revanue", effect => effect.RevanueEffect);
	}

	[MenuItem("Company Man Debugging/Resource Calculators/Calculate Brand Totals")]
	public static void CalculateBrandTotals()
	{
		CalculateResourceTotals("Brand", effect => effect.BrandEffect);
	}

	[MenuItem("Company Man Debugging/Resource Calculators/Calculate Culture Totals")]
	public static void CalculateCultureTotals()
	{
		CalculateResourceTotals("Culture", effect => effect.CultureEffect);
	}

	[MenuItem("Company Man Debugging/Resource Calculators/Calculate Patents Totals")]
	public static void CalculatePatentsTotals()
	{
		CalculateResourceTotals("Patents", effect => effect.PatentsEffect);
	}

	[MenuItem("Company Man Debugging/Resource Calculators/Calculate Spreadsheets Totals")]
	public static void CalculateSpreadsheetsTotals()
	{
		CalculateResourceTotals("Spreadsheets", effect => effect.SpreadsheetsEffect);
	}

	private static void CalculateResourceTotals(string resourceName, Func<Effect, float> resourceGetter)
	{
		var gameData = LoadGameData();

		float totalInGame = 0f;

		foreach (var region in gameData.Regions)
		{
			foreach (var location in region.Locations)
			{
				float locationTotalResource = 0f;
				string npcsString = "";
				float allNpcsResource = 0f;
				foreach (var npc in location.Npcs)
				{
					float npcTotalPower = 0f;
					foreach (var interaction in npc.Interactions)
					{
						if (interaction == null)
							continue;
						npcTotalPower += resourceGetter(interaction.Result.Effect);
					}

					locationTotalResource += npcTotalPower;
					allNpcsResource += npcTotalPower;
					npcsString += $" ({npc.FirstName} {npc.LastName} - {resourceName}: {npcTotalPower})";
				}

				float missionsTotal = 0;
				foreach (var mission in location.Missions)
				{
					locationTotalResource += resourceGetter(mission.Effect);
					missionsTotal += resourceGetter(mission.Effect);
				}
				float policiesTotal = 0;
				foreach (var policy in location.Policies)
				{
					locationTotalResource += resourceGetter(policy.Effect);
					policiesTotal += resourceGetter(policy.Effect);
				}

				totalInGame += locationTotalResource;

				if (locationTotalResource != 0)
					Debug.Log($"{location.Name}: Total {resourceName} = {locationTotalResource}, From Missions: {missionsTotal}, From Policies {policiesTotal}, From NPCs: {allNpcsResource} {npcsString}");
			}
		}
			

		Debug.Log($"Total {resourceName} In Game = {totalInGame}");
	}
}
