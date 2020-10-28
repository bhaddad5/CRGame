using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Assets.GameModel.XmlParsers
{
	[XmlRoot(ElementName = "GameData")]
	public class GameDataXml
	{
		[XmlAttribute] [DefaultValue("")] public string PlayerName = "";
		[XmlAttribute] [DefaultValue(0)] public int TurnNumber = 0;
		[XmlAttribute] [DefaultValue(0)] public int Actions = 0;
		[XmlAttribute] [DefaultValue(0)] public float Ego = 0;
		[XmlAttribute] [DefaultValue(0)] public float Funds = 0;
		[XmlAttribute] [DefaultValue(0)] public float Power = 0;
		[XmlAttribute] [DefaultValue(0)] public float CorporateCulture = 0;

		[XmlElement("Department", typeof(DepartmentXml))]
		public DepartmentXml[] Departments = new DepartmentXml[0];

		public GameData FromXml()
		{
			List<Department> depts = new List<Department>();
			foreach (var departmentXml in Departments)
			{
				depts.Add(departmentXml.FromXml());
			}

			return new GameData()
			{
				PlayerName = PlayerName,
				TurnNumber = TurnNumber,
				Actions = Actions,
				CorporateCulture = CorporateCulture,
				Ego = Ego,
				Funds = Funds,
				Power = Power,
				Departments = depts,
			};
		}
	}
}