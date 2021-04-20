using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameModel
{
	public static class ActionLookup
	{
		public static Dictionary<string, Action> EndOfTurnActions = new Dictionary<string, Action>();
		public static Dictionary<string, Action<Npc>> CharacterActions = new Dictionary<string, Action<Npc>>();
	}
}
