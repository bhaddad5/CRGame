using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipCanvas : MonoBehaviour
{
	public static TooltipCanvas Instance;

	void Awake()
	{
		Instance = this;
	}

	[SerializeField] private GameObject TooltipPrefab;

	private GameObject currTooltip = null;
	public void CreateTooltip(string tooltipText)
	{
		currTooltip = GameObject.Instantiate(TooltipPrefab, transform);
		currTooltip.GetComponentInChildren<TMP_Text>().text = tooltipText;
		currTooltip.transform.position = Input.mousePosition + new Vector3(0, 20f, 0);
	}

	public void ClearTooltip()
	{
		if(currTooltip != null)
			GameObject.Destroy(currTooltip);
	}
}