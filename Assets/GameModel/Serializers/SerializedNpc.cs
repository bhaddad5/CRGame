using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedNpc
	{
		public string Id;

		public bool IsControllable;
		public bool Controlled;

		public float Ambition;
		public float Pride;

		public string FirstName;
		public string LastName;
		public int Age;

		public string RequiredVisibilityInteraction;

		public float PersonalLayoutXPos;
		public float PersonalLayoutYPos;
		public float PersonalLayoutWidth;

		public float LocationLayoutXPos;
		public float LocationLayoutYPos;
		public float LocationLayoutWidth;

		public string BackgroundImage;

		public List<SerializedInteraction> Interactions;
		public List<SerializedTrophy> Trophies;

		public static SerializedNpc Serialize(Npc ob)
		{
			List<SerializedInteraction> interactions = new List<SerializedInteraction>();
			foreach (var interaction in ob.Interactions)
			{
				interactions.Add(SerializedInteraction.Serialize(interaction));
			}

			List<SerializedTrophy> trophies = new List<SerializedTrophy>();
			foreach (var trophy in ob.Trophies)
			{
				trophies.Add(SerializedTrophy.Serialize(trophy));
			}

			return new SerializedNpc()
			{
				Id = ob.Id,
				Ambition = ob.Ambition,
				Controlled = ob.Controlled,
				Pride = ob.Pride,
				FirstName = ob.FirstName,
				LastName = ob.LastName,
				Age = ob.Age,
				Interactions = interactions,
				Trophies = trophies,
				BackgroundImage = ob.BackgroundImage.name,
				RequiredVisibilityInteraction = ob.RequiredVisibilityInteraction,
				IsControllable = ob.IsControllable,
				LocationLayoutXPos = ob.LocationLayoutXPos,
				LocationLayoutYPos = ob.LocationLayoutYPos,
				LocationLayoutWidth = ob.LocationLayoutWidth,
				PersonalLayoutXPos = ob.PersonalLayoutXPos,
				PersonalLayoutYPos = ob.PersonalLayoutYPos,
				PersonalLayoutWidth = ob.PersonalLayoutWidth,
			};
		}

		public static Npc Deserialize(SerializedNpc ob)
		{
			List<Interaction> interactions = new List<Interaction>();
			foreach (var interaction in ob.Interactions)
			{
				interactions.Add(SerializedInteraction.Deserialize(interaction));
			}

			List<Trophy> trophies = new List<Trophy>();
			foreach (var trophy in ob.Trophies)
			{
				trophies.Add(SerializedTrophy.Deserialize(trophy));
			}

			var res = new Npc()
			{
				Id = ob.Id,
				Ambition = ob.Ambition,
				Controlled = ob.Controlled,
				Pride = ob.Pride,
				FirstName = ob.FirstName,
				LastName = ob.LastName,
				Age = ob.Age,
				Interactions = interactions,
				Trophies = trophies,
				BackgroundImage = ImageLookup.Backgrounds.GetImage(ob.BackgroundImage),
				RequiredVisibilityInteraction = ob.RequiredVisibilityInteraction,
				IsControllable = ob.IsControllable,
				LocationLayoutXPos = ob.LocationLayoutXPos,
				LocationLayoutYPos = ob.LocationLayoutYPos,
				LocationLayoutWidth = ob.LocationLayoutWidth,
				PersonalLayoutXPos = ob.PersonalLayoutXPos,
				PersonalLayoutYPos = ob.PersonalLayoutYPos,
				PersonalLayoutWidth = ob.PersonalLayoutWidth,
			};

			return res;
		}

		public static Npc ResolveReferences(DeserializedDataAccessor deserializer, Npc data, SerializedNpc ob)
		{
			for (int i = 0; i < data.Interactions.Count; i++)
			{
				data.Interactions[i] = SerializedInteraction.ResolveReferences(deserializer, data.Interactions[i], ob.Interactions[i]);
			}

			return data;
		}
	}
}