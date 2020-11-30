using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipDisplayer : MonoBehaviour
{
	[SerializeField] private TMP_Text TooltipText;

	public static string CurrentTooltip = null;

    // Update is called once per frame
    void Update()
    {
	    if (CurrentTooltip != null)
	    {
		    TooltipText.gameObject.SetActive(true);
			TooltipText.text = CurrentTooltip;

			
		    transform.position = Input.mousePosition + new Vector3(0, 20f, 0);
	    }
	    else
	    {
		    TooltipText.gameObject.SetActive(false);
	    }
    }
}
