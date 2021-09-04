using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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


		public ActionRequirements FromXml()
		{
			return new ActionRequirements()
			{
				RequiredInt = RequiredInteractions.XmlStringToList(),
				RequiredDepts = RequiredDepartmentsControlled.XmlStringToList(),
				RequiredPols = RequiredPolicies.XmlStringToList(),
				RequiredTrps = RequiredTrophies.XmlStringToList(),
				RequiresAmbitionAtOrBelowValue = RequiredAmbition >= 0,
				RequiredAmbition = RequiredAmbition,
				RequiresPrideAtOrBelowValue = RequiredPride >= 0,
				RequiredPride = RequiredPride,
				RequiredControl = RequiredControl,
				RequiredPower = RequiredPower,
			};
		}

		public static ActionRequirementsXml ToXml(ActionRequirements ob)
		{
			return new ActionRequirementsXml()
			{
				RequiredInteractions = ob.RequiredInt.ListToXmlString(),
				RequiredDepartmentsControlled = ob.RequiredDepts.ListToXmlString(),
				RequiredPolicies = ob.RequiredPols.ListToXmlString(),
				RequiredTrophies = ob.RequiredTrps.ListToXmlString(),
				RequiredAmbition = ob.RequiredAmbition,
				RequiredPride = ob.RequiredPride,
				RequiredControl = ob.RequiredControl,
				RequiredPower = ob.RequiredPower,
			};
		}
	}
}