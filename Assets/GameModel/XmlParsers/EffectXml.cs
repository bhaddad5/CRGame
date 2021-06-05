using System.ComponentModel;
using System.Xml.Serialization;
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

		public Effect FromXml()
		{
			PlayerStatusSymbols statusSymbols = null;
			if (UpdateStatusSymbols != null && UpdateStatusSymbols.Length > 0)
				statusSymbols = UpdateStatusSymbols[0].FromXml();

			


			return new Effect()
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
				TraitsAdded = TraitsAdded.XmlStringToList(),
				TraitsRemoved = TraitsRemoved.XmlStringToList(),
				TrophiesClaimed = TrophiesClaimed.XmlStringToList(),
				ContextualLocationId = ContextualLocationId,
				UpdateLocationBackground = ImageLookup.Backgrounds.GetImage(UpdateLocationBackground),
				UpdateStatusSymbols = statusSymbols,
				UpdateLocationMapPosition = new Vector2(UpdateLocationMapPosX, UpdateLocationMapPosY),
			};
		}

		public static EffectXml ToXml(Effect ob)
		{
			PlayerStatusSymbolsXml[] statusSymbolsXml = null;
			if (ob.UpdateStatusSymbols != null)
				statusSymbolsXml = new []{PlayerStatusSymbolsXml.ToXml(ob.UpdateStatusSymbols)};
			return new EffectXml()
			{
				ContextualNpcId = ob.ContextualNpcId,
				AmbitionEffect = ob.AmbitionEffect,
				ControlEffect = ob.ControlEffect,
				RemoveNpcFromGame = ob.RemoveNpcFromGame,
				EgoEffect = ob.EgoEffect,
				FundsEffect = ob.FundsEffect,
				PowerEffect = ob.PowerEffect,
				PatentsEffect = ob.PatentsEffect,
				CultureEffect = ob.CultureEffect,
				SpreadsheetsEffect = ob.SpreadsheetsEffect,
				BrandEffect = ob.BrandEffect,
				RevanueEffect = ob.RevanueEffect,
				HornicalEffect = ob.HornicalEffect,
				PrideEffect = ob.PrideEffect,
				TraitsAdded = ob.TraitsAdded.ListToXmlString(),
				TraitsRemoved = ob.TraitsRemoved.ListToXmlString(),
				TrophiesClaimed = ob.TrophiesClaimed.ListToXmlString(),
				ContextualLocationId = ob.ContextualLocationId,
				UpdateLocationBackground = ob.UpdateLocationBackground?.name,
				UpdateStatusSymbols = statusSymbolsXml,
				UpdateLocationMapPosX = ob.UpdateLocationMapPosition.x,
				UpdateLocationMapPosY = ob.UpdateLocationMapPosition.y,
			};
		}
	}
}
