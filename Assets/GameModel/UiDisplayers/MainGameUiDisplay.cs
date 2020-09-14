using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public interface IUiDisplay
{
	void RefreshUiDisplay(MainGameManager mgm);
}

namespace Assets.GameModel.UiDisplayers
{
	public class MainGameUiDisplay : MonoBehaviour, IUiDisplay
	{
		private MainGameManager gameManager;

		public void Setup(MainGameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		public void RefreshUiDisplay(MainGameManager mgm)
		{

		}
	}
}