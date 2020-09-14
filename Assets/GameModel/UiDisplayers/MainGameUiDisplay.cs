using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class MainGameUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private TMP_Text Actions;
		[SerializeField] private TMP_Text Ego;
		[SerializeField] private TMP_Text Funds;
		[SerializeField] private TMP_Text Culture;
		[SerializeField] private TMP_Text TurnNumber;
		[SerializeField] private Button EndTurn;
		[SerializeField] private Transform DepartmentsParent;

		[SerializeField] private DepartmentSelectionUiDisplay DepartmentButtonPrefab;
		[SerializeField] private DepartmentUiDisplay DepartmentUiPrefab;

		public void Setup(MainGameManager mgm, List<Department> locations)
		{
			EndTurn.onClick.AddListener(() =>
			{
				mgm.EndTurn();
			});

			foreach (Department dept in locations)
			{
				var d = Instantiate(DepartmentButtonPrefab);
				d.Setup(dept, this, mgm);
				d.transform.SetParent(DepartmentsParent);
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
				GameObject.Destroy(currOpenDepartment.gameObject);
				currOpenDepartment = null;
			}
		}


		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Actions.text = $"Actions: {mgm.RemainingTurnActions}";
			Ego.text = $"Ego: {mgm.Ego}";
			Funds.text = $"Funds: ${mgm.Funds}";
			Culture.text = $"Corp Culture: {mgm.CorporateCulture}";
			TurnNumber.text = $"{mgm.TurnNumber}";

			foreach (var button in DepartmentsParent.GetComponentsInChildren<DepartmentSelectionUiDisplay>(true))
				button.RefreshUiDisplay(mgm);

			if(currOpenDepartment != null)
				currOpenDepartment.RefreshUiDisplay(mgm);
		}
	}
}