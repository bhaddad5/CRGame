using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedInteraction
	{
		public string Id;
		public string Name;

		public string Category;

		public SerializedActionRequirements Requirements;
		public SerializedActionCost Cost;

		public bool Repeatable;
		public bool Completed;

		public bool PreviewEffect;

		public List<SerializedInteractionResult> InteractionResults;

		public static SerializedInteraction Serialize(Interaction ob)
		{
			List<SerializedInteractionResult> results = new List<SerializedInteractionResult>();
			foreach (var interactionResult in ob.InteractionResults)
			{
				results.Add(SerializedInteractionResult.Serialize(interactionResult));
			}

			return new SerializedInteraction()
			{
				Id = ob.Id,
				Name = ob.Name,
				Cost = SerializedActionCost.Serialize(ob.Cost),
				Requirements = SerializedActionRequirements.Serialize(ob.Requirements),
				Repeatable = ob.Repeatable,
				Completed = ob.Completed,
				InteractionResults = results,
				PreviewEffect = ob.PreviewEffect,
				Category = ob.Category.ToString()
			};
		}

		public static Interaction Deserialize(SerializedInteraction ob)
		{
			List<InteractionResult> results = new List<InteractionResult>();
			foreach (var interactionResult in ob.InteractionResults)
			{
				results.Add(SerializedInteractionResult.Deserialize(interactionResult));
			}

			var res = new Interaction()
			{
				Id = ob.Id,
				Name = ob.Name,
				Cost = SerializedActionCost.Deserialize(ob.Cost),
				Requirements = SerializedActionRequirements.Deserialize(ob.Requirements),
				Repeatable = ob.Repeatable,
				Completed = ob.Completed,
				InteractionResults = results,
				PreviewEffect = ob.PreviewEffect,
				Category = (Interaction.InteractionCategory)Enum.Parse(typeof(Interaction.InteractionCategory), ob.Category)
			};

			return res;
		}

		public static Interaction ResolveReferences(DeserializedDataAccessor deserializer, Interaction data, SerializedInteraction ob)
		{
			for (int i = 0; i < data.InteractionResults.Count; i++)
			{
				data.InteractionResults[i] = SerializedInteractionResult.ResolveReferences(deserializer, data.InteractionResults[i], ob.InteractionResults[i]);
			}

			data.Cost = SerializedActionCost.ResolveReferences(deserializer, data.Cost, ob.Cost);
			data.Requirements = SerializedActionRequirements.ResolveReferences(deserializer, data.Requirements, ob.Requirements);

			return data;
		}
	}
}