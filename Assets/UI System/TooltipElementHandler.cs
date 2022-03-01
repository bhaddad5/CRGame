using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipElementHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		enteredTime = Time.time;
		isPointedAt = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isPointedAt = false;
		tooltipIsCreated = false;
		TooltipCanvas.Instance.ClearTooltip();
	}

	private bool isPointedAt = false;
	private bool tooltipIsCreated = false;
	private float enteredTime = 0;
	void Update()
	{
		if (tooltipIsCreated)
			return;

		if (isPointedAt)
		{
			if (Time.time - enteredTime > .2f)
			{
				string tooltip = GetComponentInParent<ITooltipProvider>()?.GetTooltip();
				if (tooltip != null)
				{
					TooltipCanvas.Instance.CreateTooltip(tooltip);
					tooltipIsCreated = true;
				}
			}
		}
	}

	void OnDestroy()
	{
		if (tooltipIsCreated)
		{
			isPointedAt = false;
			tooltipIsCreated = false;
			TooltipCanvas.Instance.ClearTooltip();
		}
	}
}
