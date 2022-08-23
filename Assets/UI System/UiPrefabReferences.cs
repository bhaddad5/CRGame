using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiPrefabReferences : MonoBehaviour
{
	public static UiPrefabReferences Instance
	{
		get
		{
			if (instance == null)
				instance = Resources.Load<UiPrefabReferences>("UiPrefabReferences");
			return instance;
		}
	}
	private static UiPrefabReferences instance;
	
	public GameObject PopupOverlayParent;
	public List<GameObject> UiPrefabs;

	public GameObject GetPrefabByName(string name)
	{
		return UiPrefabs.FirstOrDefault(p => p.name == name);
	}
}