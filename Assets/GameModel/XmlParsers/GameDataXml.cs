using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.GameModel;
using Assets.GameModel.XmlParsers;
using UnityEngine;

[XmlRoot(ElementName = "GameData")]
public class GameDataXml
{
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
			Departments = depts,
		};
	}
}
