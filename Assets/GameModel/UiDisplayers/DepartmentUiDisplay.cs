using System;
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
		[SerializeField] private Button BackButton;
		[SerializeField] private Transform PoliciesButton;
		[SerializeField] private Transform PoliciesPopup;
		[SerializeField] private Transform PolicyOptionsParent;
		[SerializeField] private Transform MissionsButton;
		[SerializeField] private Transform MissionsPopup;
		[SerializeField] private Transform MissionOptionsParent;

		[SerializeField] private FemSelectionUiDisplay FemButtonPrefab;
		[SerializeField] private FemUiDisplay FemUiPrefab;

		[SerializeField] private PolicySelectionUiDisplay policyPrefab;

		public Department Dept;
		public void Setup(Department dept, MainMapUiDisplay mguid, MainGameManager mgm)
		{
			this.Dept = dept;
			BackButton.onClick.AddListener(() => mguid.CloseCurrentDepartment(false));

			foreach (Fem fem in dept.Fems)
			{
				var f = Instantiate(FemButtonPrefab);
				f.Setup(fem, this, mgm);
				f.transform.SetParent(FemOptionsParent);
			}

			if (dept.Policies.Count == 0)
				PoliciesButton.gameObject.SetActive(false);
			foreach (Policy policy in dept.Policies)
			{
				var p = Instantiate(policyPrefab);
				p.Setup(policy, dept, mgm);
				p.transform.SetParent(PolicyOptionsParent);
			}

			ClosePolicies();
			CloseMissions();
		}

		public void OpenPolicies()
		{
			PoliciesPopup.gameObject.SetActive(true);
		}

		public void ClosePolicies()
		{
			PoliciesPopup.gameObject.SetActive(false);
		}

		public void OpenMissions()
		{
			MissionsPopup.gameObject.SetActive(true);
		}

		public void CloseMissions()
		{
			MissionsPopup.gameObject.SetActive(false);
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
			BackgroundImage.sprite = Dept.BackgroundImage;
			Name.text = Dept.Name;

			foreach (var button in FemOptionsParent.GetComponentsInChildren<FemSelectionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);

			foreach (var button in PolicyOptionsParent.GetComponentsInChildren<PolicySelectionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);

			if (currOpenFem != null)
				currOpenFem.RefreshUiDisplay(mgm);
		}
	}
}