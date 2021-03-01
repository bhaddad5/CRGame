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

		//Dumb, but this doesn't work when called from Setup()
		void Start()
		{
			GetComponent<RectTransform>().anchorMin = new Vector2(fem.Layout.X, fem.Layout.Y);
			GetComponent<RectTransform>().anchorMax = new Vector2(fem.Layout.X, fem.Layout.Y);
			GetComponent<RectTransform>().sizeDelta = new Vector2(fem.Layout.Width, fem.Layout.Width * 2f);
			GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}

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