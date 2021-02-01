using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	public class InteractionXml
	{
		[XmlAttribute] [DefaultValue("")] public string Id = "";
		[XmlAttribute] [DefaultValue("")] public string Name = "";

		[XmlAttribute] [DefaultValue("")] public string Category = "";

        [XmlAttribute] [DefaultValue(false)] public bool Repeatable = false;
        [XmlAttribute] [DefaultValue(false)] public bool Completed = false;

		[XmlElement("Cost", typeof(ActionCostXml))]
		public ActionCostXml[] Cost = new ActionCostXml[0];

		[XmlElement("Requirements", typeof(ActionRequirementsXml))]
		public ActionRequirementsXml[] Requirements = new ActionRequirementsXml[0];

		[XmlElement("InteractionResult", typeof(InteractionResultXml))]
		public InteractionResultXml[] InteractionResults = new InteractionResultXml[0];

		public Interaction FromXml()
		{
			List<InteractionResult> results = new List<InteractionResult>();
			foreach (var resultXml in InteractionResults)
			{
				results.Add(resultXml.FromXml());
			}

			return new Interaction()
			{
				Id = Id,
				Name = Name,
				Cost = Cost[0].FromXml(),
				Requirements = Requirements[0].FromXml(),
				Repeatable = Repeatable,
				Completed = Completed,
				InteractionResults = results,
				Category = (Interaction.InteractionCategory)Enum.Parse(typeof(Interaction.InteractionCategory), Category)
			};
		}

		public static InteractionXml ToXml(Interaction ob)
		{
			List<InteractionResultXml> results = new List<InteractionResultXml>(); 
			foreach (var result in ob.InteractionResults)
			{
				results.Add(InteractionResultXml.ToXml(result));
			}

			return new InteractionXml()
			{
				Id = ob.Id,
				Name = ob.Name,
				Cost = new []{ActionCostXml.ToXml(ob.Cost)},
				Requirements = new[] { ActionRequirementsXml.ToXml(ob.Requirements) },
				Repeatable = ob.Repeatable,
				Completed = ob.Completed,
				InteractionResults = results.ToArray(),
				Category = ob.Category.ToString(),
			};
		}
	}
}
