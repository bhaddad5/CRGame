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
		[XmlAttribute] [DefaultValue(false)] public bool RequiredControl = false;
		[XmlAttribute] [DefaultValue("")] public string RequiredDepartmentsControled = "";
		[XmlAttribute] [DefaultValue(-1)] public float RequiredAmbition = -1;
		[XmlAttribute] [DefaultValue(-1)] public float RequiredPride = -1;

		public ActionRequirements FromXml()
		{
			return new ActionRequirements()
			{
				RequiredInteractions = RequiredInteractions.XmlStringToList(),
				RequiredDepartmentsControled = RequiredDepartmentsControled.XmlStringToList(),
				RequiredPolicies = RequiredPolicies.XmlStringToList(),
				RequiredTrophies = RequiredTrophies.XmlStringToList(),
				RequiredAmbition = RequiredAmbition,
				RequiredPride = RequiredPride,
				RequiredControl = RequiredControl,
			};
		}

		public static ActionRequirementsXml ToXml(ActionRequirements ob)
		{
			return new ActionRequirementsXml()
			{
				RequiredInteractions = ob.RequiredInteractions.ListToXmlString(),
				RequiredDepartmentsControled = ob.RequiredDepartmentsControled.ListToXmlString(),
				RequiredPolicies = ob.RequiredPolicies.ListToXmlString(),
				RequiredTrophies = ob.RequiredTrophies.ListToXmlString(),
				RequiredAmbition = ob.RequiredAmbition,
				RequiredPride = ob.RequiredPride,
				RequiredControl = ob.RequiredControl,
			};
		}
	}
}