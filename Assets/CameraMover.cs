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
			transform.position = new Vector3(Mathf.Clamp(desiredPos.x, -7.9f, 7.9f), Mathf.Clamp(desiredPos.y, -2.6f, 1.9f), -10f);
		}

		prevMousePos = Input.mousePosition;
	}
}