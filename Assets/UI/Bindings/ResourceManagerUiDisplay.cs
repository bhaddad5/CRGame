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
	private Func<float, string> resourceValDisplayConverter = null;
	public void RefreshResourceDisplay(float resourceValue, Func<float, string> resourceValDisplayConverter = null)
	{
		this.resourceValDisplayConverter = resourceValDisplayConverter ?? new Func<float, string>(v => v.ToString());

		if (lastResourceValue >= 0 && lastResourceValue < resourceValue)
		{
			StartCoroutine(UpdateResourceValue(lastResourceValue, resourceValue));
		}
		else
		{
			ValueText.text = this.resourceValDisplayConverter(resourceValue);
		}

		lastResourceValue = resourceValue;
	}

	private IEnumerator UpdateResourceValue(float currValue, float endingValue)
	{
		audioSource.Play();
		var tick = (endingValue - currValue) * .3f;
		while (currValue < endingValue)
		{
			currValue = currValue + tick;
			ValueText.text = resourceValDisplayConverter((int)currValue);
			yield return new WaitForSeconds(0.1f);
		}

		ValueText.text = resourceValDisplayConverter(endingValue);
		audioSource.Stop();
	}
}
