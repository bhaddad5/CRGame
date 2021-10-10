using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public class SerializedPlayerStatysSymbols
	{
		public string CarName;
		public Texture2D CarImage;

		public string SuitsName;
		public Texture2D SuitsImage;

		public Texture2D JewleryCuffs;
		public Texture2D JewleryPen;
		public Texture2D JewleryRing;
		public Texture2D JewleryWatch;

		public static SerializedPlayerStatysSymbols Serialize(PlayerStatusSymbols ob)
		{
			return new SerializedPlayerStatysSymbols()
			{
				CarImage = ob.CarImage,
				CarName = ob.CarName,
				SuitsImage = ob.SuitsImage,
				SuitsName = ob.SuitsName,
				JewleryCuffs = ob.JewleryCuffs,
				JewleryPen = ob.JewleryPen,
				JewleryRing = ob.JewleryRing,
				JewleryWatch = ob.JewleryWatch,
			};
		}

		public static PlayerStatusSymbols Deserialize(SerializedPlayerStatysSymbols ob)
		{
			var res = new PlayerStatusSymbols()
			{
				CarImage = ob.CarImage,
				CarName = ob.CarName,
				SuitsImage = ob.SuitsImage,
				SuitsName = ob.SuitsName,
				JewleryCuffs = ob.JewleryCuffs,
				JewleryPen = ob.JewleryPen,
				JewleryRing = ob.JewleryRing,
				JewleryWatch = ob.JewleryWatch,
			};
			
			return res;
		}

		public static PlayerStatusSymbols ResolveReferences(DeserializedDataAccessor deserializer, PlayerStatusSymbols data, SerializedPlayerStatysSymbols ob)
		{
			return data;
		}
	}

}
