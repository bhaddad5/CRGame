using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class FemSelectionUiDisplay : MonoBehaviour, IUiDisplay
	{
		[SerializeField] private Button Button;
		[SerializeField] private TMP_Text Text;
		[SerializeField] private Image FemPic;

		private Fem fem;

		public void Setup(Fem fem, DepartmentUiDisplay deptUi, MainGameManager mgm)
		{
			this.fem = fem;
			Button.onClick.AddListener(() =>
			{
				deptUi.ShowFem(fem, mgm);
			});
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			Text.text = $"{fem.FirstName} {fem.LastName}";
			FemPic.sprite = FemPicManager.GetFemPicFromId(fem.Id, fem.DetermineCurrPictureId());
		}
	}
}