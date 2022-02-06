using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManagerUiDisplay : MonoBehaviour
{
	[SerializeField] private TMP_Text ValueText;
	[SerializeField] private AudioSource audioSource;

	private float lastResourceValue = -1;
	public void RefreshResourceDisplay(float resourceValue)
	{
		if (lastResourceValue >= 0 && lastResourceValue < resourceValue)
		{
			StartCoroutine(UpdateResourceValue(lastResourceValue, resourceValue));
		}
		else
		{
			ValueText.text = $"{(int)resourceValue}";
		}

		lastResourceValue = resourceValue;
	}

	private IEnumerator UpdateResourceValue(float currValue, float endingValue)
	{
		audioSource.Play();
		var tick = (endingValue - currValue) * .3f;
		while (currValue < endingValue)
		{
			currValue = Mathf.Clamp(currValue + tick, currValue, endingValue);
			ValueText.text = $"{(int)currValue}";
			yield return new WaitForSeconds(0.1f);
		}

		ValueText.text = $"{(int)endingValue}";
		audioSource.Stop();
	}
}
