using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	public class DialogEntry
	{
		public enum Speaker
		{
			Player,
			Fem,
			Narrator,
			CustomFemId,
		}
		public Speaker CurrSpeaker = Speaker.Fem;
		public string CustomSpeakerId = "";
		public string Text = "";

		//Should these be up a level in InteractionResult?
		public bool InPlayerOffice;
		public string NpcImage = "";
	}
}