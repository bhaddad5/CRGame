using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPrefabReferences : MonoBehaviour
{
	public GameObject PopupOverlayParent;

	public static UiPrefabReferences Instance;

	void Awake()
	{
		Instance = this;
	}
}