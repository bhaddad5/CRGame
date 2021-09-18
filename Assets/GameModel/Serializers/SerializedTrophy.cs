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
		public string Image;
		public bool Owned;

		public static SerializedTrophy Serialize(Trophy ob)
		{
			return new SerializedTrophy()
			{
				Id = ob.Id,
				Name = ob.Name,
				Image = ob.Image?.GetName(),
				Owned = ob.Owned,
			};
		}

		public static Trophy Deserialize(SerializedTrophy ob)
		{
			var res = new Trophy()
			{
				Id = ob.Id,
				Name = ob.Name,
				Image = ImageLookup.Trophies.GetImage(ob.Image),
				Owned = ob.Owned,
			};

			return res;
		}

		public static Trophy ResolveReferences(DeserializedDataAccessor deserializer, Trophy data, SerializedTrophy ob)
		{
			return data;
		}
	}
}