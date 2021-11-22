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
		public string PlayerName;
		public int TurnNumber;
		public float Ego;
		public float Funds;
		public float Power;
		public float Patents;
		public float CorporateCulture;
		public float Spreadsheets;
		public float Brand;
		public float Revenue;
		public int Hornical;

		public int Car;
		public int Suits;
		public bool JewleryCuffs;
		public bool JewleryPen;
		public bool JewleryRing;
		public bool JewleryWatch;

		public List<SavedLocationState> Locations;

		public static SaveGameState FromData(GameData data)
		{
			var res = new SaveGameState();

			res.PlayerName = data.PlayerName;
			res.TurnNumber = data.TurnNumber;
			res.Ego = data.Ego;
			res.Funds = data.Funds;
			res.Power = data.Power;
			res.Patents = data.Patents;
			res.CorporateCulture = data.CorporateCulture;
			res.Spreadsheets = data.Spreadsheets;
			res.Brand = data.Brand;
			res.Revenue = data.Revenue;
			res.Hornical = data.Hornical;

			res.Car = data.Car;
			res.Suits = data.Suits;
			res.JewleryCuffs = data.JewleryCuffs;
			res.JewleryPen = data.JewleryPen;
			res.JewleryRing = data.JewleryRing;
			res.JewleryWatch = data.JewleryWatch;

			res.Locations = new List<SavedLocationState>();
			foreach (var dataLocation in data.Locations)
			{
				if (dataLocation != null)
					res.Locations.Add(SavedLocationState.FromData(dataLocation));
			}

			return res;
		}

		public void ApplyToData(GameData data)
		{
			data.PlayerName = PlayerName;
			data.TurnNumber = TurnNumber;
			data.Ego = Ego;
			data.Funds = Funds;
			data.Power = Power;
			data.Patents = Patents;
			data.CorporateCulture = CorporateCulture;
			data.Spreadsheets = Spreadsheets;
			data.Brand = Brand;
			data.Revenue = Revenue;
			data.Hornical = Hornical;

			data.Car = Car;
			data.Suits = Suits;
			data.JewleryCuffs = JewleryCuffs;
			data.JewleryPen = JewleryPen;
			data.JewleryRing = JewleryRing;
			data.JewleryWatch = JewleryWatch;

			foreach (var location in Locations)
			{
				location.ApplyToData(data.Locations.FirstOrDefault(d => d?.Id == location.Id));
			}
		}
	}

	[Serializable]
	public struct SavedLocationState
	{
		public string Id;

		public List<SavedNpcState> Npcs;
		public List<SavedPolicyState> Policies;

		public static SavedLocationState FromData(Location data)
		{
			var res = new SavedLocationState();

			res.Id = data.Id;

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


			return res;
		}

		public void ApplyToData(Location data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find location with id {Id}");
				return;
			}

			foreach (var npc in Npcs)
			{
				npc.ApplyToData(data.Npcs.FirstOrDefault(d => d?.Id == npc.Id));
			}
			foreach (var policy in Policies)
			{
				policy.ApplyToData(data.Policies.FirstOrDefault(d => d?.Id == policy.Id));
			}
		}
	}

	[Serializable]
	public struct SavedPolicyState
	{
		public string Id;

		public bool Active;

		public static SavedPolicyState FromData(Policy data)
		{
			var res = new SavedPolicyState();
			res.Id = data.Id;
			res.Active = data.Active;

			return res;
		}

		public void ApplyToData(Policy data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find interaction with id {Id}");
				return;
			}
			data.Active = Active;
		}
	}

	[Serializable]
	public struct SavedNpcState
	{
		public string Id;

		public float Ambition;
		public float Pride;
		public bool Controlled;

		public List<SavedInteractionState> Interactions;
		public List<SavedTrophyState> Trophies;

		public static SavedNpcState FromData(Npc data)
		{
			var res = new SavedNpcState();
			res.Id = data.Id;
			res.Ambition = data.Ambition;
			res.Controlled = data.Controlled;
			res.Pride = data.Pride;

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

		public void ApplyToData(Npc data)
		{
			if (data == null)
			{
				Debug.Log($"Could not find npc with id {Id}");
				return;
			}

			data.Ambition = Ambition;
			data.Controlled = Controlled;
			data.Pride = Pride;

			foreach (var interaction in Interactions)
			{
				interaction.ApplyToData(data.Interactions.FirstOrDefault(d => d?.Id == interaction.Id));
			}
			foreach (var trophy in Trophies)
			{
				trophy.ApplyToData(data.Trophies.FirstOrDefault(d => d?.Id == trophy.Id));
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

		public bool Completed;

		public static SavedInteractionState FromData(Interaction data)
		{
			var res = new SavedInteractionState();
			res.Id = data.Id;
			res.Completed = data.Completed;

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
		}
	}
}
