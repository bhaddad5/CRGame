using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameModel
{
	public class Policy
	{
		public string Id;

		public Action PerTurnAction;
		public bool Active;
	}
}
