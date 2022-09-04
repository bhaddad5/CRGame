using System.Collections;
using System.Collections.Generic;
using Assets.GameModel;
using UnityEngine;

public class DataUpgradeRefs : MonoBehaviour
{
	public static DataUpgradeRefs Instance
	{
		get
		{
			if (instance == null)
				instance = Resources.Load<DataUpgradeRefs>("DataUpgradeRefs");
			return instance;
		}
	}
	private static DataUpgradeRefs instance;

    public InventoryItem Hornical;
}
