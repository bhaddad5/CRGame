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
		TooltipDisplayer.CurrentTooltip = null;
	}

	private bool isPointedAt = false;
	private float enteredTime = 0;
	void Update()
	{
		if (isPointedAt)
		{
			if (Time.time - enteredTime > .2f)
			{
				string tooltip = GetComponentInParent<ITooltipProvider>()?.GetTooltip(MainGameManager.Manager);
				if (tooltip != null)
					TooltipDisplayer.CurrentTooltip = tooltip;
			}
		}
	}
}
