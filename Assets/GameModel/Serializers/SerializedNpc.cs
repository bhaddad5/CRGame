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
		public string Bio;

		public string RequiredVisibilityInteraction;

		public float PersonalLayoutXPos;
		public float PersonalLayoutYPos;
		public float PersonalLayoutWidth;

		public float LocationLayoutXPos;
		public float LocationLayoutYPos;
		public float LocationLayoutWidth;

		public Texture2D BackgroundImage;

		public List<SerializedInteraction> Interactions;
		public List<SerializedTrophy> Trophies;

		public List<Texture2D> IndependentImages;
		public List<Texture2D> ControlledImages;
		public List<Texture2D> TrainedImages;

		public static SerializedNpc Serialize(Npc ob)
		{
			List<SerializedInteraction> interactions = new List<SerializedInteraction>();
			foreach (var interaction in ob.Interactions)
			{
				if (interaction == null)
					continue;
				interactions.Add(SerializedInteraction.Serialize(interaction));
			}

			List<SerializedTrophy> trophies = new List<SerializedTrophy>();
			foreach (var trophy in ob.Trophies)
			{
				if(trophy == null)
					continue;
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
				BackgroundImage = ob.BackgroundImage,
				RequiredVisibilityInteraction = ob.RequiredVisibilityInteractionReference?.Id,
				IsControllable = ob.IsControllable,
				LocationLayoutXPos = ob.LocationLayoutXPos,
				LocationLayoutYPos = ob.LocationLayoutYPos,
				LocationLayoutWidth = ob.LocationLayoutWidth,
				PersonalLayoutXPos = ob.PersonalLayoutXPos,
				PersonalLayoutYPos = ob.PersonalLayoutYPos,
				PersonalLayoutWidth = ob.PersonalLayoutWidth,
				IndependentImages = ob.IndependentImages,
				ControlledImages = ob.ControlledImages,
				TrainedImages = ob.TrainedImages,
				Bio = ob.Bio,
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

			var res = ScriptableObject.CreateInstance<Npc>();
			res.Id = ob.Id;
			res.Ambition = ob.Ambition;
			res.Controlled = ob.Controlled;
			res.Pride = ob.Pride;
			res.FirstName = ob.FirstName;
			res.LastName = ob.LastName;
			res.Age = ob.Age;
			res.Interactions = interactions;
			res.Trophies = trophies;
			res.BackgroundImage = ob.BackgroundImage;
			res.IsControllable = ob.IsControllable;
			res.LocationLayoutXPos = ob.LocationLayoutXPos;
			res.LocationLayoutYPos = ob.LocationLayoutYPos;
			res.LocationLayoutWidth = ob.LocationLayoutWidth;
			res.PersonalLayoutXPos = ob.PersonalLayoutXPos;
			res.PersonalLayoutYPos = ob.PersonalLayoutYPos;
			res.PersonalLayoutWidth = ob.PersonalLayoutWidth;
			res.IndependentImages = ob.IndependentImages;
			res.ControlledImages = ob.ControlledImages;
			res.TrainedImages = ob.TrainedImages;
			res.Bio = ob.Bio;

			return res;
		}

		public static Npc ResolveReferences(DeserializedDataAccessor deserializer, Npc data, SerializedNpc ob)
		{
			for (int i = 0; i < data.Interactions.Count; i++)
			{
				data.Interactions[i] = SerializedInteraction.ResolveReferences(deserializer, data.Interactions[i], ob.Interactions[i]);
			}

			for (int i = 0; i < data.Trophies.Count; i++)
			{
				data.Trophies[i] = SerializedTrophy.ResolveReferences(deserializer, data.Trophies[i], ob.Trophies[i]);
			}

			data.RequiredVisibilityInteractionReference = deserializer.FindInteractionById(ob.RequiredVisibilityInteraction);

			return data;
		}
	}
}