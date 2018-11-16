using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float minDistance = 1.0f;
	public float maxDistance = 15.0f;
	public float smooth = 10.0f;
	Vector3 dollyDirection;
	public Vector3 dollyDirewctionAdjustment;
	public float distance;

	void Awake()
	{
		dollyDirection = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
	}

	void Update()
	{
        Debug.DrawLine(transform.position, transform.position + dollyDirection * 10f);
        Vector3 desiredCameraPosition = transform.parent.TransformPoint(dollyDirection * maxDistance);

        if (Physics.Linecast(transform.parent.position, desiredCameraPosition, out RaycastHit hit))
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
