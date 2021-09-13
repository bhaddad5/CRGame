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
	public class ActionRequirementsXml
	{
		[XmlAttribute] [DefaultValue("")] public string RequiredInteractions = "";
		[XmlAttribute] [DefaultValue("")] public string RequiredPolicies = "";
		[XmlAttribute] [DefaultValue("")] public string RequiredTrophies = "";
		[XmlAttribute] [DefaultValue("")] public string RequiredDepartmentsControlled = "";

		[XmlAttribute] [DefaultValue(-1)] public float RequiredPower = 0;
		[XmlAttribute] [DefaultValue(-1)] public float RequiredAmbition = -1;
		[XmlAttribute] [DefaultValue(-1)] public float RequiredPride = -1;
		[XmlAttribute] [DefaultValue(false)] public bool RequiredControl = false;


		public SerializedActionRequirements FromXml()
		{
			var requs = RequiredInteractions.XmlStringToList();

			List<string> requirements = new List<string>();
			List<string> notRequirements = new List<string>();

			foreach (var requ in requs)
			{
				if (requ.StartsWith("!"))
				{
					var resId = requ.Substring(1);
					if (resId.Contains('-'))
						resId = resId.Split('-')[1];

					notRequirements.Add(resId);
				}
				else
				{
					var resId = requ;
					if (requ.Contains('-'))
						resId = requ.Split('-')[1];

					requirements.Add(resId);
				}
			}

			return new SerializedActionRequirements()
			{
				RequiredInteractionsReferences = requirements,
				RequiredNotCompletedInteractionsReferences = notRequirements,
				RequiredDepartmentsControledReferences = RequiredDepartmentsControlled.XmlStringToList(),
				RequiredPoliciesReferences = RequiredPolicies.XmlStringToList(),
				RequiredTrophiesReferences = RequiredTrophies.XmlStringToList(),
				RequiresAmbitionAtOrBelowValue = RequiredAmbition >= 0,
				RequiredAmbition = RequiredAmbition,
				RequiresPrideAtOrBelowValue = RequiredPride >= 0,
				RequiredPride = RequiredPride,
				RequiredControl = RequiredControl,
				RequiredPower = RequiredPower,
			};
		}
	}
}