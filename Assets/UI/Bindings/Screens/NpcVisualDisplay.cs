using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using Assets.UI_System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class NpcVisualDisplay : MonoBehaviour
{
    [SerializeField] private Image Picture;
	[SerializeField] private Image BackgroundImage;

    public void DisplayNpcInfo(NpcDisplayInfo displayInfo)
    {
        Picture.sprite = displayInfo.Picture.ToSprite();
		Picture.preserveAspect = true;

        BackgroundImage.sprite = displayInfo.Background.ToSprite();

        displayInfo.Layout.ApplyToRectTransform(Picture.GetComponent<RectTransform>());

    }
}
