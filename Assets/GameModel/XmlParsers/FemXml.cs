using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel.XmlParsers
{
	public class FemXml
	{
		//Gameplay
		public string Id;
		public bool Controlled;
		public float Ambition;
		public float Pride;

		//Non-Gameplay
		public string Name;
		public int Age;

		public Fem FromXml()
		{
			return new Fem()
			{
				Id = Id,
				Ambition = Ambition,
				Controlled = Controlled,
				Pride = Pride,
				Name = Name,
				Age = Age
			};
		}
	}
}