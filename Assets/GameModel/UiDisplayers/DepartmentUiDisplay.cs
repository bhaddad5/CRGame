﻿using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class DepartmentUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private TMP_Text Name;
		[SerializeField] private Transform FemOptionsParent;
		[SerializeField] private Transform PolicyOptionsParent;
		[SerializeField] private Button BackButton;

		[SerializeField] private FemSelectionUiDisplay FemButtonPrefab;
		[SerializeField] private FemUiDisplay FemUiPrefab;

		private Department dept;
		public void Setup(Department dept, MainGameUiDisplay mguid, MainGameManager mgm)
		{
			this.dept = dept;
			BackButton.onClick.AddListener(() => mguid.CloseCurrentDepartment());

			foreach (Fem fem in dept.Fems)
			{
				var f = Instantiate(FemButtonPrefab);
				f.Setup(fem, this, mgm);
				f.transform.SetParent(FemOptionsParent);
			}
		}

		private FemUiDisplay currOpenFem;
		public void ShowFem(Fem fem, MainGameManager mgm)
		{
			currOpenFem = Instantiate(FemUiPrefab);
			currOpenFem.Setup(fem, mgm, this);
			currOpenFem.RefreshUiDisplay(mgm);
		}

		public void CloseCurrentFem()
		{
			if (currOpenFem != null)
			{
				GameObject.Destroy(currOpenFem.gameObject);
				currOpenFem = null;
			}
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Name.text = dept.Name + (dept.Controlled() ? "(Controlled)" : "");

			foreach (var button in FemOptionsParent.GetComponentsInChildren<FemSelectionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);

			if (currOpenFem != null)
				currOpenFem.RefreshUiDisplay(mgm);
		}
	}
}