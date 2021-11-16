using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using GameModel.Serializers;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedMission
	{
		public string MissionName;
		public string MissionDescription;
		public Texture2D MissionImage;

		public string CompletionInteractionId;
		public List<SerializedEffect> Rewards;

		public static SerializedMission Serialize(Mission ob)
		{
			List<SerializedEffect> effects = new List<SerializedEffect>();
			foreach (var effect in ob.Rewards)
			{
				effects.Add(SerializedEffect.Serialize(effect));
			}

			return new SerializedMission()
			{
				MissionName = ob.MissionName,
				MissionDescription = ob.MissionDescription,
				MissionImage = ob.MissionImage,
				CompletionInteractionId = ob.CompletionInteractionReference?.Id,
				Rewards = effects,
			};
		}

		public static Mission Deserialize(SerializedMission ob)
		{
			List<Effect> effects = new List<Effect>();
			foreach (var effect in ob.Rewards)
			{
				effects.Add(SerializedEffect.Deserialize(effect));
			}

			var res = ScriptableObject.CreateInstance<Mission>();
			res.MissionName = ob.MissionName;
			res.MissionDescription = ob.MissionDescription;
			res.MissionImage = ob.MissionImage;
			res.Rewards = effects;

			return res;
		}

		public static Mission ResolveReferences(DeserializedDataAccessor deserializer, Mission data, SerializedMission ob)
		{
			for (int i = 0; i < data.Rewards.Count; i++)
			{
				data.Rewards[i] = SerializedEffect.ResolveReferences(deserializer, data.Rewards[i], ob.Rewards[i]);
			}

			data.CompletionInteractionReference = deserializer.FindInteractionById(ob.CompletionInteractionId);

			return data;
		}
	}
}