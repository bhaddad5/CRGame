using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class MainMapScreenBindings : MonoBehaviour
	{
		[SerializeField] private Color MorningTint;
		[SerializeField] private Color AfternoonTint;
		[SerializeField] private Image MapImage;

		[SerializeField] private Transform DepartmentsParent;

		[SerializeField] private MainMapLocationEntryBindings _locationButtonPrefab;
		[SerializeField] private LocationScreenBindings _locationUiPrefab;

		[SerializeField] private AudioClip OptionalBackgroundAudio;

		private MainGameManager mgm;
		public void Setup(MainGameManager mgm, List<Location> locations)
		{
			this.mgm = mgm;
			foreach (Location dept in locations)
			{
				var d = Instantiate(_locationButtonPrefab);
				d.Setup(dept, this, mgm);
				d.transform.SetParent(DepartmentsParent, false);
			}

			if (OptionalBackgroundAudio != null)
				AudioHandler.Instance.PlayBackgroundClip(OptionalBackgroundAudio);
		}

		private LocationScreenBindings _currOpenLocation = null;
		public void ShowDepartment(Location dept, MainGameManager mgm)
		{
			CloseCurrentDepartment(false);
			_currOpenLocation = Instantiate(_locationUiPrefab);
			_currOpenLocation.Setup(dept, mgm, () =>
			{
				if (OptionalBackgroundAudio != null)
					AudioHandler.Instance.PlayBackgroundClip(OptionalBackgroundAudio);
				RefreshUiDisplay(mgm);
			});
			_currOpenLocation.RefreshUiDisplay(mgm);
		}

		public void CloseCurrentDepartment(bool onlyCloseIfInaccessable)
		{
			if (_currOpenLocation != null)
			{
				_currOpenLocation.CloseCurrentNpc();
				if (!onlyCloseIfInaccessable || !_currOpenLocation.IsAccessible(mgm))
				{
					GameObject.Destroy(_currOpenLocation.gameObject);
					_currOpenLocation = null;
				}
			}

			if (OptionalBackgroundAudio != null)
				AudioHandler.Instance.PlayBackgroundClip(OptionalBackgroundAudio);
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			foreach (var button in DepartmentsParent.GetComponentsInChildren<MainMapLocationEntryBindings>(true))
				button.RefreshUiDisplay(mgm);

			if(_currOpenLocation != null)
				_currOpenLocation.RefreshUiDisplay(mgm);

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