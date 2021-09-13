using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GameModel.Serializers;

namespace Assets.GameModel.XmlParsers
{
	public class InteractionXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";

		[XmlAttribute] [DefaultValue("")] public string Category = "";

        [XmlAttribute] [DefaultValue(false)] public bool Repeatable = false;
        [XmlAttribute] [DefaultValue(false)] public bool Completed = false;

        [XmlAttribute] [DefaultValue(false)] public bool PreviewEffect = false;
		
		[XmlElement("Cost", typeof(ActionCostXml))]
		public ActionCostXml[] Cost = new ActionCostXml[0];

		[XmlElement("Requirements", typeof(ActionRequirementsXml))]
		public ActionRequirementsXml[] Requirements = new ActionRequirementsXml[0];

		[XmlElement("InteractionResult", typeof(InteractionResultXml))]
		public InteractionResultXml[] InteractionResults = new InteractionResultXml[0];

		public SerializedInteraction FromXml()
		{
			List<SerializedInteractionResult> results = new List<SerializedInteractionResult>();
			foreach (var resultXml in InteractionResults)
			{
				results.Add(resultXml.FromXml());
			}

			return new SerializedInteraction()
			{
				Id = Id,
				Name = Name,
				Cost = Cost[0].FromXml(),
				Requirements = Requirements[0].FromXml(),
				Repeatable = Repeatable,
				Completed = Completed,
				InteractionResults = results,
				PreviewEffect = PreviewEffect,
				Category = Category
			};
		}
	}
}
