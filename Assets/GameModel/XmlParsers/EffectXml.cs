using System.ComponentModel;
using System.Xml.Serialization;
using GameModel.Serializers;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class EffectXml
	{
		[XmlAttribute] [DefaultValue("")] public string ContextualNpcId = "";

		[XmlAttribute] [DefaultValue(0)] public float AmbitionEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float PrideEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float EgoEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float FundsEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float PowerEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float PatentsEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float CultureEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float SpreadsheetsEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float BrandEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float RevanueEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public int HornicalEffect = 0;
		[XmlAttribute] [DefaultValue(false)] public bool ControlEffect = false;
		[XmlAttribute] [DefaultValue(false)] public bool RemoveNpcFromGame = false;
		[XmlAttribute] [DefaultValue("")] public string TraitsAdded = "";
		[XmlAttribute] [DefaultValue("")] public string TraitsRemoved = "";
		[XmlAttribute] [DefaultValue("")] public string TrophiesClaimed = "";

		[XmlAttribute] [DefaultValue("")] public string ContextualLocationId = "";
		[XmlAttribute] [DefaultValue("")] public string UpdateLocationBackground = "";
		[XmlAttribute] [DefaultValue(-1)] public float UpdateLocationMapPosX = -1;
		[XmlAttribute] [DefaultValue(-1)] public float UpdateLocationMapPosY = -1;

		[XmlElement("UpdateStatusSymbols", typeof(PlayerStatusSymbolsXml))]
		public PlayerStatusSymbolsXml[] UpdateStatusSymbols = new PlayerStatusSymbolsXml[0];

		public SerializedEffect FromXml()
		{
			SerializedPlayerStatysSymbols statusSymbols = new SerializedPlayerStatysSymbols();
			if (UpdateStatusSymbols != null && UpdateStatusSymbols.Length > 0)
				statusSymbols = UpdateStatusSymbols[0].FromXml();

			return new SerializedEffect()
			{
				ContextualNpcId = ContextualNpcId,
				AmbitionEffect = AmbitionEffect,
				ControlEffect = ControlEffect,
				RemoveNpcFromGame = RemoveNpcFromGame,
				EgoEffect = EgoEffect,
				FundsEffect = FundsEffect,
				PowerEffect = PowerEffect,
				PatentsEffect = PatentsEffect,
				CultureEffect = CultureEffect,
				SpreadsheetsEffect = SpreadsheetsEffect,
				BrandEffect = BrandEffect,
				RevanueEffect = RevanueEffect,
				HornicalEffect = HornicalEffect,
				PrideEffect = PrideEffect,
				TrophiesClaimed = TrophiesClaimed.XmlStringToList(),
				ContextualLocationId = ContextualLocationId,
				UpdateLocationBackground = UpdateLocationBackground,
				ShouldUpdateLocationMapPos = !UpdateLocationMapPosX.Equals(-1f) && !UpdateLocationMapPosY.Equals(-1f),
				ShouldUpdateStatusSymbols = UpdateStatusSymbols != null && UpdateStatusSymbols.Length > 0,
				UpdateStatusSymbols = statusSymbols,
				UpdateLocationMapPosition = new Vector2(UpdateLocationMapPosX, UpdateLocationMapPosY),
			};
		}
	}
}
