﻿using System.Collections;
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

		public void Setup(Department dept, MainGameUiDisplay mainGameUi, MainGameManager mgm)
		{
			this.dept = dept;
			Button.onClick.AddListener(() =>
			{
				mainGameUi.ShowDepartment(dept, mgm);
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{dept.Name}";
			Button.interactable = dept.Accessible;
		}
	}
}