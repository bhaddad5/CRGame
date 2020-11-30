using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel
{
	public class DialogEntry
	{
		public bool IsPlayer = false;
		public string Text = "";

		//Should these be up a level in InteractionResult?
		public bool InPlayerOffice;
		public string NpcImage = "";
	}
}