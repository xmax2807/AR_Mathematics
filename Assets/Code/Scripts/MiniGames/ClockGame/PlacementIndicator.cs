using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
	public GameObject ARCamera;
    readonly List<ARRaycastHit> hits = new();
	private ARRaycastManager rayManager;
	[SerializeField]
	private GameObject visual;

	void Start()
	{

		// get the components
		rayManager = FindObjectOfType<ARRaycastManager>();
		//visual = gameObject;

		// hide the placement indicator visual
		visual.SetActive(false);
	}
	float curRotation = 90;
	void Update()
	{
		//get the current rotation of the camera
		

		// shoot a raycast from the center of the screen
		
		rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

		// if we hit an AR plane surface, update the position and rotation
		if (hits.Count > 0)
		{
			transform.position = hits[0].pose.position;
			transform.rotation = hits[0].pose.rotation;
			//make the plane rotate add 90 degree X
			curRotation = ARCamera.transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(transform.rotation.x + 90, curRotation, transform.rotation.z);

			// enable the visual if it's disabled
			if (!visual.activeInHierarchy)
				visual.SetActive(true);
		}
	}
}