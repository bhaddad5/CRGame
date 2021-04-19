using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class LocationUiDisplay : MonoBehaviour, IUiDisplay
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
		[SerializeField] private MissionUiDisplay misisonPrefab;

		public Location Dept;
		public void Setup(Location dept, MainMapUiDisplay mguid, MainGameManager mgm)
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

			if (dept.Missions.Count == 0)
				MissionsButton.gameObject.SetActive(false);
			foreach (Mission mission in dept.Missions)
			{
				var m = Instantiate(misisonPrefab);
				m.Setup(mission, dept, mgm);
				m.transform.SetParent(MissionOptionsParent);
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

			foreach (var fem in FemOptionsParent.GetComponentsInChildren<FemSelectionUiDisplay>(true))
				fem.RefreshUiDisplay(mgm);

			foreach (var policy in PolicyOptionsParent.GetComponentsInChildren<PolicySelectionUiDisplay>(true))
				policy.RefreshUiDisplay(mgm);

			foreach (var mission in MissionOptionsParent.GetComponentsInChildren<MissionUiDisplay>(true))
				mission.RefreshUiDisplay(mgm);

			if (currOpenFem != null)
				currOpenFem.RefreshUiDisplay(mgm);
		}
	}
}