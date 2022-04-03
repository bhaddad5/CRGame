using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameModel.UiDisplayers
{
	public class GenericPopupCloser : MonoBehaviour
	{
		public void ClosePopup()
		{
			GameObject.Destroy(transform.parent.gameObject);
		}
	}
}