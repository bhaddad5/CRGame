using System.Collections.Generic;

namespace Assets.GameModel
{
	public class Fem
	{
		public string Id;

		public bool Controlled;

		public float Ambition;
		public float Pride;

		public string FirstName;
		public string LastName;
		public int Age;

		public List<Interaction> Interactions = new List<Interaction>();
		public List<Trait> Traits = new List<Trait>();

		public string DetermineCurrPictureId()
		{
			if (Controlled && Pride < 1)
				return "trained";
			else if (Controlled)
				return "controlled";
			else
				return "independent";
		}
	}
}
