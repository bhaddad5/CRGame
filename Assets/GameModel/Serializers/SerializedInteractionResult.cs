using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using GameModel.Serializers;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedInteractionResult
	{
		public List<SerializedDialogEntry> Dialogs;
		public List<SerializedPopup> OptionalPopups;
		public List<SerializedEffect> Effects;
		public int Probability;

		public static SerializedInteractionResult Serialize(InteractionResult ob)
		{
			List<SerializedEffect> effects = new List<SerializedEffect>();
			foreach (var effect in ob.Effects)
			{
				effects.Add(SerializedEffect.Serialize(effect));
			}

			List<SerializedPopup> popups = new List<SerializedPopup>();
			foreach (var popup in ob.OptionalPopups)
			{
				popups.Add(SerializedPopup.Serialize(popup));
			}

			List<SerializedDialogEntry> dialogs = new List<SerializedDialogEntry>();
			foreach (var dialog in ob.Dialogs)
			{
				dialogs.Add(SerializedDialogEntry.Serialize(dialog));
			}

			return new SerializedInteractionResult()
			{
				Probability = ob.Probability,
				Effects = effects,
				OptionalPopups = popups,
				Dialogs = dialogs,
			};
		}

		public static InteractionResult Deserialize(SerializedInteractionResult ob)
		{
			List<Effect> effects = new List<Effect>();
			foreach (var effect in ob.Effects)
			{
				effects.Add(SerializedEffect.Deserialize(effect));
			}

			List<Popup> popups = new List<Popup>();
			foreach (var popup in ob.OptionalPopups)
			{
				popups.Add(SerializedPopup.Deserialize(popup));
			}

			List<DialogEntry> dialogs = new List<DialogEntry>();
			foreach (var dialog in ob.Dialogs)
			{
				dialogs.Add(SerializedDialogEntry.Deserialize(dialog));
			}

			var res = new InteractionResult()
			{
				Probability = ob.Probability,
				Effects = effects,
				OptionalPopups = popups,
				Dialogs = dialogs,
			};

			return res;
		}

		public static InteractionResult ResolveReferences(DeserializedDataAccessor deserializer, InteractionResult data, SerializedInteractionResult ob)
		{
			for (int i = 0; i < data.Effects.Count; i++)
			{
				data.Effects[i] = SerializedEffect.ResolveReferences(deserializer, data.Effects[i], ob.Effects[i]);
			}
			for (int i = 0; i < data.OptionalPopups.Count; i++)
			{
				data.OptionalPopups[i] = SerializedPopup.ResolveReferences(deserializer, data.OptionalPopups[i], ob.OptionalPopups[i]);
			}
			for (int i = 0; i < data.Dialogs.Count; i++)
			{
				data.Dialogs[i] = SerializedDialogEntry.ResolveReferences(deserializer, data.Dialogs[i], ob.Dialogs[i]);
			}


			return data;
		}
	}
}