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
                    //add the created prefab to our array
                    _instantiatedPrefabs[imageName] = newPrefab;
                }
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
            //loop through all tracked images that have been updated
        }

        //If the AR subsystem has given up looking for a tracked image
        foreach (var trackedImage in eventArgs.removed)
        {
            //Destroy its prefab
            Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
            //Also remove the instance from our array
            _instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
            //or simply set the prefab to inactive
            //_instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
