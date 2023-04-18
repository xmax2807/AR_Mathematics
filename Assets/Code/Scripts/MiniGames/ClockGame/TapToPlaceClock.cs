using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using System;


public class TapToPlaceClock : MonoBehaviour
{
    public GameObject _objectToPlace;

    //private ARSessionOrigin _arOrigin;
    private ARRaycastManager _arOrigin;
    public GameObject _placementIndicator;

	private Pose _placementPose;
    private bool _placementPoseIsValid = false;

	// Start is called before the first frame update
	void Start()
    {
        _arOrigin = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }

        UpdatePlacementPose();
        UpdatePlacementIndicator();

        
    }

	private void PlaceObject()
	{
        Instantiate(_objectToPlace, _placementPose.position, _placementPose.rotation);
	}

	private void UpdatePlacementIndicator()
	{
		if (_placementPoseIsValid)
        {
            _placementIndicator.SetActive(true);
            _placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
        {
			_placementIndicator.SetActive(false);
		}
	}

	private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
		_arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        _placementPoseIsValid = hits.Count > 0;
        if ( _placementPoseIsValid )
        {
            _placementPose = hits[0].pose;

            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }
}
