using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public class TooltipProviderBasic : MonoBehaviour, ITooltipProvider
{
	public string Tooltip = null;
	public string GetTooltip()
	{
		return Tooltip;
	}
}
