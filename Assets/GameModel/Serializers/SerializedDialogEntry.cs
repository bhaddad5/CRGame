using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace GameModel.Serializers
{
	[Serializable]
	public struct SerializedDialogEntry
	{
		public string CurrSpeaker;
		public string CustomSpeakerReference;
		public string Text;

		//Should these be up a level in InteractionResult?
		public bool InPlayerOffice;
		public Texture2D CustomBackground;
		public List<Texture2D> CustomNpcImageOptions;

		public static SerializedDialogEntry Serialize(DialogEntry ob)
		{
			return new SerializedDialogEntry()
			{
				CurrSpeaker = ob.CurrSpeaker.ToString(),
				CustomSpeakerReference = ob.CustomSpeakerReference?.Id,
				Text = ob.Text,
				InPlayerOffice = ob.InPlayerOffice,
				CustomBackground = ob.CustomBackground,
				CustomNpcImageOptions = ob.CustomNpcImageOptions,
			};
		}

		public static DialogEntry Deserialize(SerializedDialogEntry ob)
		{
			DialogEntry.Speaker.TryParse(ob.CurrSpeaker, out DialogEntry.Speaker speaker);
			var res = new DialogEntry()
			{
				CurrSpeaker = speaker,
				Text = ob.Text,
				InPlayerOffice = ob.InPlayerOffice,
				CustomBackground = ob.CustomBackground,
				CustomNpcImageOptions = ob.CustomNpcImageOptions,
			};
			
			return res;
		}

		public static DialogEntry ResolveReferences(DeserializedDataAccessor deserializer, DialogEntry data, SerializedDialogEntry ob)
		{
			data.CustomSpeakerReference = deserializer.FindNpcById(ob.CustomSpeakerReference);

			return data;
		}
	}
}