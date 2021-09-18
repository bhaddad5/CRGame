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
		public string CarImage;

		public string SuitsName;
		public string SuitsImage;

		public string JewleryCuffs;
		public string JewleryPen;
		public string JewleryRing;
		public string JewleryWatch;

		public static SerializedPlayerStatysSymbols Serialize(PlayerStatusSymbols ob)
		{
			return new SerializedPlayerStatysSymbols()
			{
				CarImage = ob.CarImage?.GetName(),
				CarName = ob.CarName,
				SuitsImage = ob.SuitsImage?.GetName(),
				SuitsName = ob.SuitsName,
				JewleryCuffs = ob.JewleryCuffs?.GetName(),
				JewleryPen = ob.JewleryPen?.GetName(),
				JewleryRing = ob.JewleryRing?.GetName(),
				JewleryWatch = ob.JewleryWatch?.GetName(),
			};
		}

		public static PlayerStatusSymbols Deserialize(SerializedPlayerStatysSymbols ob)
		{
			var res = new PlayerStatusSymbols()
			{
				CarImage = ImageLookup.StatusSymbols.GetImage(ob.CarImage),
				CarName = ob.CarName,
				SuitsImage = ImageLookup.StatusSymbols.GetImage(ob.SuitsImage),
				SuitsName = ob.SuitsName,
				JewleryCuffs = ImageLookup.StatusSymbols.GetImage(ob.JewleryCuffs),
				JewleryPen = ImageLookup.StatusSymbols.GetImage(ob.JewleryPen),
				JewleryRing = ImageLookup.StatusSymbols.GetImage(ob.JewleryRing),
				JewleryWatch = ImageLookup.StatusSymbols.GetImage(ob.JewleryWatch),
			};
			
			return res;
		}

		public static PlayerStatusSymbols ResolveReferences(DeserializedDataAccessor deserializer, PlayerStatusSymbols data, SerializedPlayerStatysSymbols ob)
		{
			return data;
		}
	}

}
