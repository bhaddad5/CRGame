using System.Collections;
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
		[SerializeField] private Image BackgroundImage;
		[SerializeField] private Transform FemOptionsParent;
		[SerializeField] private Transform PolicyOptionsParent;
		[SerializeField] private Button BackButton;

		[SerializeField] private FemSelectionUiDisplay FemButtonPrefab;
		[SerializeField] private FemUiDisplay FemUiPrefab;

		[SerializeField] private PolicyUiDisplay PolicyButtonPrefab;

		private Department dept;
		public void Setup(Department dept, MainMapUiDisplay mguid, MainGameManager mgm)
		{
			this.dept = dept;
			BackButton.onClick.AddListener(() => mguid.CloseCurrentDepartment());

			foreach (Fem fem in dept.Fems)
			{
				var f = Instantiate(FemButtonPrefab);
				f.Setup(fem, this, mgm);
				f.transform.SetParent(FemOptionsParent);
			}

			foreach (Policy policy in dept.Policies)
			{
				var p = Instantiate(PolicyButtonPrefab);
				p.Setup(policy, dept, mgm);
				p.transform.SetParent(PolicyOptionsParent);
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
			BackgroundImage.sprite = dept.BackgroundImage;
			Name.text = dept.Name;

			foreach (var button in FemOptionsParent.GetComponentsInChildren<FemSelectionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);

			foreach (var button in PolicyOptionsParent.GetComponentsInChildren<PolicyUiDisplay>(true))
				button.RefreshUiDisplay(mgm);

			if (currOpenFem != null)
				currOpenFem.RefreshUiDisplay(mgm);
		}
	}
}