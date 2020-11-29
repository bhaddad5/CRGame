using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class MainMapUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private Color MorningTint;
		[SerializeField] private Color AfternoonTint;
		[SerializeField] private Image MapImage;

		[SerializeField] private Transform DepartmentsParent;

		[SerializeField] private DepartmentSelectionUiDisplay DepartmentButtonPrefab;
		[SerializeField] private DepartmentUiDisplay DepartmentUiPrefab;

		public void Setup(MainGameManager mgm, List<Department> locations)
		{
			foreach (Department dept in locations)
			{
				var d = Instantiate(DepartmentButtonPrefab);
				d.Setup(dept, this, mgm);
				d.transform.SetParent(DepartmentsParent, false);
			}
		}

		private DepartmentUiDisplay currOpenDepartment = null;
		public void ShowDepartment(Department dept, MainGameManager mgm)
		{
			currOpenDepartment = Instantiate(DepartmentUiPrefab);
			currOpenDepartment.Setup(dept, this, mgm);
			currOpenDepartment.RefreshUiDisplay(mgm);
		}

		public void CloseCurrentDepartment()
		{
			if (currOpenDepartment != null)
			{
				currOpenDepartment.CloseCurrentFem();
				GameObject.Destroy(currOpenDepartment.gameObject);
				currOpenDepartment = null;
			}
		}
		
		public void RefreshUiDisplay(MainGameManager mgm)
		{
			foreach (var button in DepartmentsParent.GetComponentsInChildren<DepartmentSelectionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);

			if(currOpenDepartment != null)
				currOpenDepartment.RefreshUiDisplay(mgm);

			ShowTimeOfDay(mgm.Data.TurnNumber % 2 == 1);
		}

		public void ShowTimeOfDay(bool afternoon)
		{
			if (afternoon)
				MapImage.color = AfternoonTint;
			else
				MapImage.color = MorningTint;
		}
	}
}