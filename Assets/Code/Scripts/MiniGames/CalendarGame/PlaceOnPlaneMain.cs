using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager), typeof(ARAnchorManager))]
public class PlaceOnPlaneMain : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    public event System.Action<GameObject> OnSpawnMainPlane;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_ARAnchorManager = GetComponent<ARAnchorManager>();

        //placementUpdate.AddListener(DiableVisual);

        // spawnedObject = Instantiate(m_PlacedPrefab, m_PlacedPrefab.transform.position, m_PlacedPrefab.transform.rotation);
        // Transform objTrans = spawnedObject.transform;

        // Vector3 direction = (objTrans.position - ARCamera.transform.position).normalized;
        // direction = new Vector3(0, direction.y, direction.z);
        // Quaternion lookRotation = Quaternion.LookRotation(direction);

        // var originalAngles = objTrans.rotation.eulerAngles;
        // spawnedObject.transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x + originalAngles.x, lookRotation.eulerAngles.y + originalAngles.y, originalAngles.z);
        // spawnedObject.AddComponent<ARAnchor>();
        // Project.Managers.TimeCoroutineManager.Instance.WaitForSeconds(0.5f, ()=> OnSpawnMainPlane?.Invoke(spawnedObject));
        // var rotation = objTrans.rotation;
        // spawnedObject.transform.Rotate(rotation.x + rotateObject.x, rotation.y + rotateObject.y, rotation.z + rotateObject.z);
    }

    public void SetPlacedPrefab(GameObject newPlace)
    {
        m_PlacedPrefab = newPlace;
    }
    public void SetPlacedPrefabAndStart(GameObject newPlace)
    {
        m_PlacedPrefab = newPlace;
        var newPlaceObj = Instantiate(newPlace, Vector3.zero, Quaternion.identity);
        spawnedObject = newPlaceObj;
        spawnedObject.AddComponent<ARAnchor>();
        OnSpawnMainPlane?.Invoke(newPlaceObj);
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    void Update()
    {
        //get ARCAmera current roation y
        //var curRotation = ARCamera.transform.rotation.eulerAngles.y;

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            m_ARAnchorManager?.AttachAnchor(s_Hits[0].trackable as ARPlane, hitPose);

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);                
                spawnedObject.AddComponent<ARAnchor>();

                DisablePlaneDetection();
                OnSpawnMainPlane?.Invoke(spawnedObject);
            }
        }
    }

    private void DisablePlaneDetection()
    {
        m_PlaneManager.enabled = false;
        foreach (var plane in m_PlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    static List<ARRaycastHit> s_Hits = new();

    ARRaycastManager m_RaycastManager;
    ARPlaneManager m_PlaneManager;
    ARAnchorManager m_ARAnchorManager;
}
