using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using GameModel.Serializers;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedPolicy
	{
		public string Id;
		public string Name;
		public bool Active;
		public string Image;
		public string Description;

		public SerializedActionRequirements Requirements;
		public List<SerializedEffect> Effects;

		public static SerializedPolicy Serialize(Policy ob)
		{
			List<SerializedEffect> effects = new List<SerializedEffect>();
			foreach (var effect in ob.Effects)
			{
				effects.Add(SerializedEffect.Serialize(effect));
			}

			return new SerializedPolicy()
			{
				Id = ob.Id,
				Name = ob.Name,
				Active = ob.Active,
				Description = ob.Description,
				Image = ob.Image?.GetName(),
				Requirements = SerializedActionRequirements.Serialize(ob.Requirements),
				Effects = effects,
			};
		}

		public static Policy Deserialize(SerializedPolicy ob)
		{
			List<Effect> effects = new List<Effect>();
			foreach (var effect in ob.Effects)
			{
				effects.Add(SerializedEffect.Deserialize(effect));
			}

			var res = ScriptableObject.CreateInstance<Policy>();
			res.Id = ob.Id;
			res.Name = ob.Name;
			res.Active = ob.Active;
			res.Description = ob.Description;
			res.Image = ImageLookup.Policies.GetImage(ob.Image);
			res.Requirements = SerializedActionRequirements.Deserialize(ob.Requirements);
			res.Effects = effects;

			return res;
		}

		public static Policy ResolveReferences(DeserializedDataAccessor deserializer, Policy data, SerializedPolicy ob)
		{
			for (int i = 0; i < data.Effects.Count; i++)
			{
				data.Effects[i] = SerializedEffect.ResolveReferences(deserializer, data.Effects[i], ob.Effects[i]);
			}
			data.Requirements = SerializedActionRequirements.ResolveReferences(deserializer, data.Requirements, ob.Requirements);

			return data;
		}
	}
}