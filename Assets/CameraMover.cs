using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
	[SerializeField] private float moveScaler = .5f;
	private Vector3 prevMousePos = Vector3.zero;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 dragChange = Input.mousePosition - prevMousePos;
			var desiredPos = transform.position - (dragChange * moveScaler);
			transform.position = new Vector3(Mathf.Clamp(desiredPos.x, -7.9f, 7.9f), Mathf.Clamp(desiredPos.y, -2.6f, 2.6f), -10f);
		}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.M))
		{
			var map = GameObject.Find("MainMapCanvas");

			var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var mousePosRelativeToMap = map.transform.InverseTransformPoint(mousePos);
			var halfMapSize = map.GetComponent<RectTransform>().sizeDelta / 2f;

			Debug.Log((mousePosRelativeToMap.x + halfMapSize.x).ToString("F0") + ", " + (mousePosRelativeToMap.y + halfMapSize.y).ToString("F0"));
		}
#endif

		prevMousePos = Input.mousePosition;
	}
}