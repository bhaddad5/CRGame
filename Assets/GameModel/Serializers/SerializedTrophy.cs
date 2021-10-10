using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedTrophy
	{
		public string Id;
		public string Name;
		public Texture2D Image;
		public bool Owned;

		public static SerializedTrophy Serialize(Trophy ob)
		{
			return new SerializedTrophy()
			{
				Id = ob.Id,
				Name = ob.Name,
				Image = ob.Image,
				Owned = ob.Owned,
			};
		}

		public static Trophy Deserialize(SerializedTrophy ob)
		{
			var res = ScriptableObject.CreateInstance<Trophy>();
			res.Id = ob.Id;
			res.Name = ob.Name;
			res.Image = ob.Image;
			res.Owned = ob.Owned;

			return res;
		}

		public static Trophy ResolveReferences(DeserializedDataAccessor deserializer, Trophy data, SerializedTrophy ob)
		{
			return data;
		}
	}
}