using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using Assets.GameModel.UiDisplayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemBindings : MonoBehaviour
{
	[SerializeField] private Image Image;
	[SerializeField] private TMP_Text Name;
	private InventoryItem item;

	public void Setup(InventoryItem item)
	{
		this.item = item;
		Image.sprite = item.Image.ToSprite();
		Name.text = item.Name;
	}

	public void RefreshUiDisplay(MainGameManager mgm)
	{
		var itemCount = 0;
		if (mgm.Data.Inventory.ContainsKey(item))
			itemCount = mgm.Data.Inventory[item];
		gameObject.SetActive(itemCount > 0);
		Name.text =$"{item.Name} x {itemCount}";
	}
}
