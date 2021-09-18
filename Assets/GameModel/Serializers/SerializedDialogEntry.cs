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
		public string CustomBackground;
		public string NpcImage;

		public static SerializedDialogEntry Serialize(DialogEntry ob)
		{
			return new SerializedDialogEntry()
			{
				CurrSpeaker = ob.CurrSpeaker.ToString(),
				CustomSpeakerReference = ob.CustomSpeakerReference?.Id,
				Text = ob.Text,
				InPlayerOffice = ob.InPlayerOffice,
				NpcImage = ob.NpcImage,
				CustomBackground = ob.CustomBackground?.GetName(),
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
				NpcImage = ob.NpcImage,
				CustomBackground = ImageLookup.Backgrounds.GetImage(ob.CustomBackground),
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