using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class DepartmentSelectionUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;

		private Department dept;

		public void Setup(Department dept, MainMapUiDisplay mainMapUi, MainGameManager mgm)
		{
			this.dept = dept;
			Button.onClick.AddListener(() =>
			{
				mainMapUi.ShowDepartment(dept, mgm);
			});
			Button.transform.position = new Vector3(dept.UiPosition.x, dept.UiPosition.y, 0);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{dept.Name}";
			Button.interactable = dept.Accessible;
		}
	}
}