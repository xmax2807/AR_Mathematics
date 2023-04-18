using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceClock : MonoBehaviour
{
    //Reference to AR tracked image manager component
    private ARTrackedImageManager _trackedImageManager;

    public GameObject[] ARPrefabs;

    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

    void Awake()
    {
        //Get the AR tracked image manager component
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _trackedImageManager.enabled = true;
    }

    void OnEnable()
    {
        //Subscribe to the tracked image event
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        //Unsubscribe to the tracked image event
        _trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //loop through all new tracked images that have been detected
        foreach (var trackedImage in eventArgs.added)
        {
            //get the name of the ref image
            var imageName = trackedImage.referenceImage.name;
            //now loop over the array of prefabs
            foreach (var curPrefab in ARPrefabs)
            {
                if (string.Compare(curPrefab.name, imageName, System.StringComparison.OrdinalIgnoreCase) == 0 && !_instantiatedPrefabs.ContainsKey(imageName))
                {
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                    newPrefab.transform.Rotate(newPrefab.transform.rotation.x, newPrefab.transform.rotation.y - 90, newPrefab.transform.rotation.z - 90);
                    //add the created prefab to our array
                    _instantiatedPrefabs[imageName] = newPrefab;
                }
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            //get the name of the ref image
            var imageName = trackedImage.referenceImage.name;
            //now loop over the array of prefabs
            foreach (var curPrefab in ARPrefabs)
            {
                if(string.Compare(curPrefab.name, imageName, System.StringComparison.OrdinalIgnoreCase) != 0) continue;
                if (!_instantiatedPrefabs.ContainsKey(imageName))
                {
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
					newPrefab.transform.Rotate(newPrefab.transform.rotation.x, newPrefab.transform.rotation.y - 90, newPrefab.transform.rotation.z - 90);

					//add the created prefab to our array
					_instantiatedPrefabs[imageName] = newPrefab;
                }
                
                _instantiatedPrefabs[imageName].SetActive(trackedImage.trackingState == TrackingState.Tracking);
            }
        }

        //If the AR subsystem has given up looking for a tracked image
        foreach (var trackedImage in eventArgs.removed)
        {
            var name = trackedImage.referenceImage.name;
            //Destroy its prefab
            // Destroy(_instantiatedPrefabs[name]);
            // //Also remove the instance from our array
            // _instantiatedPrefabs.Remove(name);
            //or simply set the prefab to inactive
            _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
        }
    }
}
