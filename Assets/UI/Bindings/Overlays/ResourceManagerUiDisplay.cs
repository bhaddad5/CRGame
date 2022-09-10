using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.UI_System;
using TMPro;
using UnityEngine;

public class ResourceManagerUiDisplay : MonoBehaviour
{
	[SerializeField] private TMP_Text ValueText;

	private float lastResourceValue = -1;
	public void RefreshResourceDisplay(float resourceValue, MainGameManager mgm)
	{
		if (lastResourceValue >= 0 && lastResourceValue < resourceValue)
		{
			StartCoroutine(UpdateResourceValue(lastResourceValue, resourceValue, mgm));
		}
		else
		{
			ValueText.text = $"{(int)resourceValue}";
		}

		lastResourceValue = resourceValue;
	}

	private IEnumerator UpdateResourceValue(float currValue, float endingValue, MainGameManager mgm)
	{
		var tick = (endingValue - currValue) * .15f;
		while (currValue < endingValue)
		{
			currValue = Mathf.Clamp(currValue + tick, currValue, endingValue);
			ValueText.text = $"{(int)currValue}";
			AudioHandler.Instance.PlayEffectClip(mgm.ResourceTickAudio);
			yield return new WaitForSeconds(0.05f);
		}
		ValueText.text = $"{(int)endingValue}";
	}
}
