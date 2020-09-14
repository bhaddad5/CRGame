using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public class DepartmentUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private Transform FemOptionsParent;
		[SerializeField] private Transform PolicyOptionsParent;

		private Department dept;
		public void Setup(Department dept)
		{
			this.dept = dept;
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Name.text = dept.Name + (dept.Controlled() ? "(Controlled)" : "");
		}
	}
}