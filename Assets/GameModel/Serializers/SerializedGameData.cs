using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedGameData
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

		public SerializedPlayerStatysSymbols StatusSymbols;

		public List<SerializedLocation> Locations;

		public static SerializedGameData Serialize(GameData ob)
		{
			var locs = new List<SerializedLocation>();
			foreach (var location in ob.Locations)
			{
				locs.Add(SerializedLocation.Serialize(location));
			}

			return new SerializedGameData()
			{
				PlayerName = ob.PlayerName,
				TurnNumber = ob.TurnNumber,
				Ego = ob.Ego,
				Funds = ob.Funds,
				Power = ob.Power,
				Patents = ob.Patents,
				CorporateCulture = ob.CorporateCulture,
				Spreadsheets = ob.Spreadsheets,
				Brand = ob.Brand,
				Revenue = ob.Revenue,
				Hornical = ob.Hornical,
				StatusSymbols = SerializedPlayerStatysSymbols.Serialize(ob.StatusSymbols),
				Locations = locs,
			};
		}

		public static GameData Deserialize(SerializedGameData ob)
		{
			var locs = new List<Location>();
			foreach (var location in ob.Locations)
			{
				locs.Add(SerializedLocation.Deserialize(location));
			}

			var res = ScriptableObject.CreateInstance<GameData>();
			res.PlayerName = ob.PlayerName;
			res.TurnNumber = ob.TurnNumber;
			res.Ego = ob.Ego;
			res.Funds = ob.Funds;
			res.Power = ob.Power;
			res.Patents = ob.Patents;
			res.CorporateCulture = ob.CorporateCulture;
			res.Spreadsheets = ob.Spreadsheets;
			res.Brand = ob.Brand;
			res.Revenue = ob.Revenue;
			res.Hornical = ob.Hornical;
			res.StatusSymbols = SerializedPlayerStatysSymbols.Deserialize(ob.StatusSymbols);
			res.Locations = locs;

			return res;
		}

		public static GameData ResolveReferences(DeserializedDataAccessor deserializer, GameData data, SerializedGameData ob)
		{
			for (int i = 0; i < data.Locations.Count; i++)
			{
				data.Locations[i] = SerializedLocation.ResolveReferences(deserializer, data.Locations[i], ob.Locations[i]);
			}

			data.StatusSymbols = SerializedPlayerStatysSymbols.ResolveReferences(deserializer, data.StatusSymbols, ob.StatusSymbols);

			return data;
		}
	}
}