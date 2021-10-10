using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedEffect
	{
		public string ContextualNpcId;

		public float AmbitionEffect;
		public float PrideEffect;
		public float EgoEffect;
		public float FundsEffect;
		public float PowerEffect;
		public float PatentsEffect;
		public float CultureEffect;
		public float SpreadsheetsEffect;
		public float BrandEffect;
		public float RevanueEffect;
		public int HornicalEffect;
		public bool ControlEffect;
		public bool RemoveNpcFromGame;
		public List<string> TrophiesClaimed;

		public string ContextualLocationId;
		public Texture2D UpdateLocationBackground;
		public bool ShouldUpdateLocationMapPos;
		public Vector2 UpdateLocationMapPosition;

		public bool ShouldUpdateStatusSymbols;
		public SerializedPlayerStatysSymbols UpdateStatusSymbols;

		public static SerializedEffect Serialize(Effect ob)
		{
			List<string> trophiesClaimed = new List<string>();
			foreach (var trophy in ob.TrophiesClaimedReferences)
			{
				if (trophy == null)
					continue;
				trophiesClaimed.Add(trophy.Id);
			}
			
			return new SerializedEffect()
			{
				ContextualNpcId = ob.ContextualNpcReference?.Id,
				AmbitionEffect = ob.AmbitionEffect,
				PrideEffect = ob.PrideEffect,
				ControlEffect = ob.ControlEffect,
				RemoveNpcFromGame = ob.RemoveNpcFromGame,
				TrophiesClaimed = trophiesClaimed,

				ContextualLocationId = ob.ContextualLocationReference?.Id,
				UpdateLocationBackground = ob.UpdateLocationBackground,
				ShouldUpdateLocationMapPos = ob.ShouldUpdateLocationMapPos,
				UpdateLocationMapPosition = ob.UpdateLocationMapPosition,

				EgoEffect = ob.EgoEffect,
				FundsEffect = ob.FundsEffect,
				PowerEffect = ob.PowerEffect,
				HornicalEffect = ob.HornicalEffect,

				BrandEffect = ob.BrandEffect,
				CultureEffect = ob.CultureEffect,
				PatentsEffect = ob.PatentsEffect,
				SpreadsheetsEffect = ob.SpreadsheetsEffect,
				RevanueEffect = ob.RevanueEffect,

				ShouldUpdateStatusSymbols = ob.ShouldUpdateStatusSymbols,
				UpdateStatusSymbols = SerializedPlayerStatysSymbols.Serialize(ob.UpdateStatusSymbols),
			};
		}

		public static Effect Deserialize(SerializedEffect ob)
		{
			var effect = new Effect()
			{
				AmbitionEffect = ob.AmbitionEffect,
				PrideEffect = ob.PrideEffect,
				ControlEffect = ob.ControlEffect,
				RemoveNpcFromGame = ob.RemoveNpcFromGame,

				UpdateLocationBackground = ob.UpdateLocationBackground,
				ShouldUpdateLocationMapPos = ob.ShouldUpdateLocationMapPos,
				UpdateLocationMapPosition = ob.UpdateLocationMapPosition,

				EgoEffect = ob.EgoEffect,
				FundsEffect = ob.FundsEffect,
				PowerEffect = ob.PowerEffect,
				HornicalEffect = ob.HornicalEffect,

				BrandEffect = ob.BrandEffect,
				CultureEffect = ob.CultureEffect,
				PatentsEffect = ob.PatentsEffect,
				SpreadsheetsEffect = ob.SpreadsheetsEffect,
				RevanueEffect = ob.RevanueEffect,

				ShouldUpdateStatusSymbols = ob.ShouldUpdateStatusSymbols,
				UpdateStatusSymbols = SerializedPlayerStatysSymbols.Deserialize(ob.UpdateStatusSymbols),
			};
			
			return effect;
		}

		public static Effect ResolveReferences(DeserializedDataAccessor deserializer, Effect data, SerializedEffect ob)
		{
			data.ContextualNpcReference = deserializer.FindNpcById(ob.ContextualNpcId);
			data.ContextualLocationReference = deserializer.FindLocationById(ob.ContextualLocationId);
			data.TrophiesClaimedReferences = new List<Trophy>();
			data.UpdateStatusSymbols = SerializedPlayerStatysSymbols.ResolveReferences(deserializer, data.UpdateStatusSymbols, ob.UpdateStatusSymbols);
			foreach (var trophyId in ob.TrophiesClaimed)
			{
				data.TrophiesClaimedReferences.Add(deserializer.FindTrophyById(trophyId));
			}
			
			return data;
		}
	}
}