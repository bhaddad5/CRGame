using System.ComponentModel;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class EffectXml
	{
		[XmlAttribute] [DefaultValue(0)] public float AmbitionEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float PrideEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float EgoEffect = 0;
		[XmlAttribute] [DefaultValue(0)] public float FundsEffect = 0;
		[XmlAttribute] [DefaultValue(false)] public bool ControlEffect = false;
		[XmlAttribute] [DefaultValue("")] public string TraitsAdded = "";
		[XmlAttribute] [DefaultValue("")] public string TraitsRemoved = "";

		public Effect FromXml()
		{
			return new Effect()
			{
				AmbitionEffect = AmbitionEffect,
				ControlEffect = ControlEffect,
				EgoEffect = EgoEffect,
				FundsEffect = FundsEffect,
				PrideEffect = PrideEffect,
				TraitsAdded = TraitsAdded.XmlStringToList(),
				TraitsRemoved = TraitsRemoved.XmlStringToList(),
			};
		}
	}
}
