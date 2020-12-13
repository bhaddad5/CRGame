using System.ComponentModel;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class EffectXml
	{
		[XmlAttribute] [DefaultValue("")] public string ContextualFemId = "";

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

		public Effect FromXml()
		{
			return new Effect()
			{
				ContextualFemId = ContextualFemId,
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
			};
		}

		public static EffectXml ToXml(Effect ob)
		{
			return new EffectXml()
			{
				ContextualFemId = ob.ContextualFemId,
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
			};
		}
	}
}
