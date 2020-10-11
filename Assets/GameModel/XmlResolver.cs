using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.GameModel.XmlParsers;
using UnityEngine;

namespace Assets.GameModel
{
	public class XmlResolver
	{
		private Dictionary<string, Interaction> interactionsLookup = new Dictionary<string, Interaction>();
		private Dictionary<string, Interaction> femsLookup = new Dictionary<string, Interaction>();

		[XmlInclude(typeof(InteractionXml))]
		[XmlInclude(typeof(EffectXml))]
		public class InteractionsList
		{
			[XmlElement("Interaction", typeof(InteractionXml))]
			public InteractionXml[] Interactions = new InteractionXml[0];
		}

		public void LoadXmlData()
		{
			var interactionsXml = (TextAsset) Resources.Load("XmlData/Interactions");

			var serializer = new XmlSerializer(typeof(InteractionsList));
			var interactions = (InteractionsList) serializer.Deserialize(new StringReader(interactionsXml.text));

			foreach (var interaction in interactions.Interactions)
			{
				interactionsLookup[interaction.Id] = interaction.FromXml();
			}

			Debug.Log(interactionsLookup.Count);
		}
	}
}
