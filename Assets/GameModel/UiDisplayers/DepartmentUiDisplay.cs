using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public class DepartmentUiDisplay : MonoBehaviour, IUiDisplay
	{
		private Department dept;
		public void Setup(Department dept)
		{
			this.dept = dept;
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{

		}
	}
}