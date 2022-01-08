using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiPrefabReferences : MonoBehaviour
{
	public static UiPrefabReferences Instance;
	void Awake()
	{
		Instance = this;
	}

	public GameObject PopupOverlayParent;
	public List<GameObject> UiPrefabs;

	public GameObject GetPrefabByName(string name)
	{
		return UiPrefabs.FirstOrDefault(p => p.name == name);
	}
}