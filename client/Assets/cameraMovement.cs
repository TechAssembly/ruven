using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
	public float minDistance = 1.0f;
	public float maxDistance = 15.0f;
	public float smooth = 10.0f;
	private Vector3 dollyDirection;
	public Vector3 dollyDirectionAdjustment;
	public float distance;

	private void Awake()
	{
		dollyDirection = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}

	private void Update()
	{
		Vector3 desiredCameraPosition = transform.parent.TransformPoint(dollyDirection * maxDistance);
		RaycastHit hit;

		if (Physics.Linecast(transform.parent.position, desiredCameraPosition, out hit))
		{
			distance = Mathf.Clamp(hit.distance * 0.9f, minDistance, maxDistance);
		}
		else
		{
			distance = maxDistance;
		}

		transform.localPosition =
			Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime * smooth); 
	}
}
