using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameModel.Save
{
	[Serializable]
	public struct SaveGameState
	{
		public int SaveGameVersion;

		public string FirstName;
		public string LastName;
		public int TurnNumber;
		public float Ego;
		public float Funds;
		public float Power;
		public float Patents;
		public float CorporateCulture;
		public float Spreadsheets;
		public float Brand;
		public float Revenue;

		//LEGACY
		public int Hornical;
		//END LEGACY

		public int Promotion;
		public int Home;

		public int Car;
		public int Suits;
		public bool JewleryCuffs;
		public bool JewleryPen;
		public bool JewleryRing;
		public bool JewleryWatch;

		public List<SavedInventoryItemState> Inventory;
		public List<SavedLocationState> Locations;
		public List<SavedInteractionState> StartTurnInteractions;
		public List<SavedAchievementState> Achievements;

		public static SaveGameState FromData(GameData data, int version)
		{
			var res = new SaveGameState();

			res.SaveGameVersion = version;
			res.FirstName = data.FirstName ?? "Hunter";
			res.LastName = data.LastName ?? "Downe";
			res.TurnNumber = data.TurnNumber;
			res.Ego = data.Ego;
			res.Funds = data.Funds;
			res.Power = data.Power;
			res.Patents = data.Patents;
			res.CorporateCulture = data.CorporateCulture;
			res.Spreadsheets = data.Spreadsheets;
			res.Brand = data.Brand;
			res.Revenue = data.Revenue;
			res.Promotion = data.Promotion;
			res.Home = data.Home;
			res.Car = data.Car;
			res.Suits = data.Suits;
			res.JewleryCuffs = data.JewleryCuffs;
			res.JewleryPen = data.JewleryPen;
			res.JewleryRing = data.JewleryRing;
			res.JewleryWatch = data.JewleryWatch;

			res.Inventory = new List<SavedInventoryItemState>();
			foreach (var inventoryItemType in data.Inventory)
			{
				for (int i = 0; i < inventoryItemType.Value; i++)
				{
					res.Inventory.Add(SavedInventoryItemState.FromData(inventoryItemType.Key));
				}
			}

			res.Locations = new List<SavedLocationState>();
			foreach (var dataRegion in data.Regions)
			{
				foreach (var dataLocation in dataRegion.Locations)
				{
					if (dataLocation != null)
						res.Locations.Add(SavedLocationState.FromData(dataLocation));
				}
			}

			res.StartTurnInteractions = new List<SavedInteractionState>();
			foreach (var startOfTurnInteraction in data.StartOfTurnInteractions)
			{
				if(startOfTurnInteraction != null)
					res.StartTurnInteractions.Add(SavedInteractionState.FromData(startOfTurnInteraction));
			}

			res.Achievements = new List<SavedAchievementState>();
			foreach (var achievement in data.Achievements)
			{
				if(achievement != null)
					res.Achievements.Add(SavedAchievementState.FromData(achievement));
			}

			return res;
		}

		public void ApplyToData(GameData data)
		{
			data.FirstName = FirstName ?? "Hunter";
			data.LastName = LastName ?? "Downe";
			data.TurnNumber = TurnNumber;
			data.Ego = Ego;
			data.Funds = Funds;
			data.Power = Power;
			data.Patents = Patents;
			data.CorporateCulture = CorporateCulture;
			data.Spreadsheets = Spreadsheets;
			data.Brand = Brand;
			data.Revenue = Revenue;
			data.Promotion = Promotion;
			data.Home = Home;
			data.Car = Car;
			data.Suits = Suits;
			data.JewleryCuffs = JewleryCuffs;
			data.JewleryPen = JewleryPen;
			data.JewleryRing = JewleryRing;
			data.JewleryWatch = JewleryWatch;

			data.Inventory = new Dictionary<InventoryItem, int>();
			foreach (var item in Inventory)
			{
				var foundItem = data.InventoryItemOptions.FirstOrDefault(d => d?.Id == item.Id);
				if(foundItem == null)
					Debug.LogError($"Failed to find item with id {item.Id}");
				else
					data.AddItemToInventory(foundItem);
			}

			foreach (var resRegion in data.Regions)
			{
				foreach (var resLocation in resRegion.Locations)
				{
					if (Locations.Any(d => d.Id == resLocation?.Id))
					{
						var dataLocation = Locations.FirstOrDefault(d => d.Id == resLocation?.Id);
						dataLocation.ApplyToData(resLocation, SaveGameVersion);
					}
					
				}
			}
			
			foreach (var startTurnInteraction in StartTurnInteractions)
			{
				startTurnInteraction.ApplyToData(data.StartOfTurnInteractions.FirstOrDefault(i => i?.Id == startTurnInteraction.Id));
			}

			foreach (var achievement in Achievements)
			{
				achievement.ApplyToData(data.Achievements.FirstOrDefault(a => a?.Id == achievement.Id));
			}

			if (SaveGameVersion <= 0)
			{
				for (int i = 0; i < Hornical; i++)
					data.AddItemToInventory(DataUpgradeRefs.Instance.Hornical);
			}
		}
	}

	[Serializable]
	public struct SavedInventoryItemState
	{
		public string Id;

		public static SavedInventoryItemState FromData(InventoryItem data)
		{
			var res = new SavedInventoryItemState();
			res.Id = data.Id;

			return res;
		}
	}

	[Serializable]
	public struct SavedLocationState
	{
		public string Id;

		public bool Controlled;

		public List<SavedNpcState> Npcs;
		public List<SavedPolicyState> Policies;
		public List<SavedMissionState> Missions;

		public static SavedLocationState FromData(Location data)
		{
			var res = new SavedLocationState();

			res.Id = data.Id;
			res.Controlled = data.Controlled;

			res.Npcs = new List<SavedNpcState>();
			foreach (var dataNpc in data.Npcs)
			{
				if(dataNpc != null)
					res.Npcs.Add(SavedNpcState.FromData(dataNpc));
			}

			res.Policies = new List<SavedPolicyState>();
			foreach (var dataPolicy in data.Policies)
			{
				if (dataPolicy != null)
					res.Policies.Add(SavedPolicyState.FromData(dataPolicy));
			}

			res.Missions = new List<SavedMissionState>();
			foreach (var dataMission in data.Missions)
			{
				if(dataMission != null)
					res.Missions.Add(SavedMissionState.FromData(dataMission));
			}


			return res;
		}

		public void ApplyToData(Location data, int saveVersion)
		{
			if (data == null)
			{
				Debug.Log($"Could not find location with id {Id}");
				return;
			}

			data.Controlled = Controlled;

			foreach (var npc in Npcs)
			{
				npc.ApplyToData(data.Npcs.FirstOrDefault(d => d?.Id == npc.Id), saveVersion);
			}
			foreach (var policy in Policies)
			{
				policy.ApplyToData(data.Policies.FirstOrDefault(d => d?.Id == policy.Id));
			}
			foreach (var mission in Missions)
			{
				mission.ApplyToData(data.Missions.FirstOrDefault(d => d?.Id == mission.Id));
			}
		}
	}

	[Serializable]
	public struct SavedPolicyState
	{
		public string Id;

		public bool Active;
		public bool Visited;

		public static SavedPolicyState FromData(Policy data)
		{
			var res = new SavedPolicyState();
			res.Id = data.Id;
			res.Active = data.Active;
			res.Visited = !data.New;

			return res;
		}

		public void ApplyToData(Policy data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find Policy with id {Id}");
				return;
			}
			data.Active = Active;
			data.New = !Visited;
		}
	}

	[Serializable]
	public struct SavedMissionState
	{
		public string Id;

		public bool Completed;

		public static SavedMissionState FromData(Mission data)
		{
			var res = new SavedMissionState();
			res.Id = data.Id;
			res.Completed = data.Completed;

			return res;
		}

		public void ApplyToData(Mission data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find Mission with id {Id}");
				return;
			}
			data.Completed = Completed;
		}
	}

	[Serializable]
	public struct SavedNpcState
	{
		public string Id;

		public float Ambition;
		public float Pride;
		public bool Controlled;
		public bool Exists;
		public bool Trained;

		public List<SavedInteractionState> Interactions;
		public List<SavedTrophyState> Trophies;

		public List<string> CurrentImageSets;
		public List<string> RemovedImages;

		public static SavedNpcState FromData(Npc data)
		{
			var res = new SavedNpcState();
			res.Id = data.Id;
			res.Ambition = data.Ambition;
			res.Controlled = data.Controlled;
			res.Pride = data.Pride;
			res.Exists = data.Exists;
			res.Trained = data.Trained;

			res.RemovedImages = data.RemovedImages;
			res.CurrentImageSets = new List<string>();
			foreach (var imageSet in data.CurrentImageSets)
				res.CurrentImageSets.Add(imageSet.Id);

			res.Interactions = new List<SavedInteractionState>();
			foreach (var dataInteraction in data.Interactions)
			{
				if (dataInteraction != null)
					res.Interactions.Add(SavedInteractionState.FromData(dataInteraction));
			}

			res.Trophies = new List<SavedTrophyState>();
			foreach (var dataTrophy in data.Trophies)
			{
				if (dataTrophy != null)
					res.Trophies.Add(SavedTrophyState.FromData(dataTrophy));
			}

			return res;
		}

		public void ApplyToData(Npc data, int saveVersion)
		{
			if (data == null)
			{
				Debug.Log($"Could not find npc with id {Id}");
				return;
			}

			data.Ambition = Ambition;
			data.Controlled = Controlled;
			data.Pride = Pride;
			data.Exists = Exists;
			data.Trained = Trained;

			data.RemovedImages = RemovedImages;

			foreach (var interaction in Interactions)
			{
				interaction.ApplyToData(data.Interactions.FirstOrDefault(d => d?.Id == interaction.Id));
			}
			foreach (var trophy in Trophies)
			{
				trophy.ApplyToData(data.Trophies.FirstOrDefault(d => d?.Id == trophy.Id));
			}

			data.CurrentImageSets.Clear();
			foreach (var imageSetId in CurrentImageSets)
			{
				var imageSet = data.AllImageSets.FirstOrDefault(i => i.Id == imageSetId);
				if (imageSet != null)
					data.CurrentImageSets.Add(imageSet);
				else
					Debug.Log($"Could not find image set with id {Id}");
			}

			//If we're on an old save then the list is empty
			if (saveVersion <= 0)
			{
				if (Trained)
				{
					data.CurrentImageSets.Add(data.Legacy_TrainedImages);
				}
				else if (Controlled)
				{
					data.CurrentImageSets.Add(data.Legacy_ControlledImages);
				}
				else
				{
					data.CurrentImageSets.Add(data.Legacy_IndependentImages);
				}
			}
		}
	}

	[Serializable]
	public struct SavedTrophyState
	{
		public string Id;

		public bool Owned;

		public static SavedTrophyState FromData(Trophy data)
		{
			var res = new SavedTrophyState();
			res.Id = data.Id;
			res.Owned = data.Owned;

			return res;
		}

		public void ApplyToData(Trophy data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find interaction with id {Id}");
				return;
			}
			data.Owned = Owned;
		}
	}

	[Serializable]
	public struct SavedInteractionState
	{
		public string Id;

		public int Completed;
		public int FailCount;
		public bool New;
		public int TurnCompletedOn;

		public static SavedInteractionState FromData(Interaction data)
		{
			var res = new SavedInteractionState();
			res.Id = data.Id;
			res.Completed = data.Completed;
			res.FailCount = data.FailCount;
			res.New = data.New;
			res.TurnCompletedOn = data.TurnCompletedOn;

			return res;
		}

		public void ApplyToData(Interaction data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find interaction with id {Id}");
				return;
			}
			data.Completed = Completed;
			data.FailCount = FailCount;
			data.New = New;
			data.TurnCompletedOn = TurnCompletedOn;
		}
	}

	[Serializable]
	public struct SavedAchievementState
	{
		public string Id;

		public bool Completed;

		public static SavedAchievementState FromData(Achievement data)
		{
			var res = new SavedAchievementState();
			res.Id = data.Id;
			res.Completed = data.Completed;

			return res;
		}

		public void ApplyToData(Achievement data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find Achievement with id {Id}");
				return;
			}
			data.Completed = Completed;
		}
	}
}
