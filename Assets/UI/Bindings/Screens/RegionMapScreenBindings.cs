using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameModel.UiDisplayers
{
	public class RegionMapScreenBindings : MonoBehaviour
	{
		[SerializeField] private Color MorningTint;
		[SerializeField] private Color AfternoonTint;
		[SerializeField] private Image MapImage;

		[SerializeField] private Transform LocationsParent;

		[SerializeField] private RegionMapLocationEntryBindings _locationButtonPrefab;
		[SerializeField] private LocationScreenBindings _locationUiPrefab;

		[SerializeField] private AudioClip OptionalBackgroundAudio;

		[SerializeField] private TMP_Text RegionName;

		[SerializeField] private GameObject RegionHud;

		private MainGameManager mgm;
		private Action onClose;
		private Region region;
		public void Setup(MainGameManager mgm, Region region, Action onClose)
		{
			this.onClose = onClose;
			this.mgm = mgm;
			this.region = region;

			CameraMover.Instance.ResetCameraPos();
			CameraMover.Instance.SetScreenSize(new Vector2(MapImage.mainTexture.width, MapImage.mainTexture.height));

			foreach (Location loc in region.Locations)
			{
				var d = Instantiate(_locationButtonPrefab);
				d.Setup(loc, this, mgm);
				d.transform.SetParent(LocationsParent, false);
			}

			if (OptionalBackgroundAudio != null)
				AudioHandler.Instance.PlayBackgroundClip(OptionalBackgroundAudio);
		}

		private LocationScreenBindings _currOpenLocation = null;
		public void ShowLocation(Location dept, MainGameManager mgm)
		{
			CloseCurrentDepartment(false);
			RegionHud.SetActive(false);
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

		public void CloseRegion()
		{
			GameObject.Destroy(gameObject);
			onClose?.Invoke();
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{
			RegionName.text = region.Name;

			foreach (var button in LocationsParent.GetComponentsInChildren<RegionMapLocationEntryBindings>(true))
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