using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Project.MiniGames;
using Project.MiniGames.HouseBuilding;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlaneHouse : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    public Vector3 rotateObject;
    public Camera ARCamera;

    UnityEvent placementUpdate;

    [SerializeField]
    GameObject visualObject;

    [SerializeField]
    UnityEngine.Events.UnityEvent onSpawnPlane;
    public event System.Action<GameObject> OnSpawnMainPlane;
    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject PlacedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();

        if (placementUpdate == null)
            placementUpdate = new UnityEvent();

        if (ARCamera == null)
        {
            ARCamera = Camera.main;
        }
    }

    public void SetMainPlaneAndStart(MainPlane mainPlane)
    {
        m_PlacedPrefab = mainPlane.gameObject;
        var mainPlaneObj = Instantiate(mainPlane, Vector3.zero, Quaternion.identity);
        spawnedObject = mainPlaneObj.gameObject;
        OnSpawnMainPlane?.Invoke(spawnedObject);
    }
    public void SetMainPLane(MainPlane mainPlane)
    {
        m_PlacedPrefab = mainPlane.gameObject;
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
        OnSpawnMainPlane?.Invoke(newPlaceObj);
    }

    bool TryGetTouchPosition(out Vector3 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#endif
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

        if (!TryGetTouchPosition(out Vector3 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            var trackable = s_Hits[0].trackable;

            if (spawnedObject == null)
            {
                //Vector3 position0 = new Vector3(0, 0, 0);
                //spawnedObject = Instantiate(m_PlacedPrefab, trackable.transform.position ,Quaternion.identity) ;
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, m_PlacedPrefab.transform.rotation);
                spawnedObject.AddComponent<ARAnchor>();
                //spawnedObject.transform.LookAt(ARCamera.transform);
                //var rotation = spawnedObject.transform.rotation;
                //spawnedObject.transform.Rotate(rotation.x, rotation.y - 90, rotation.z);
                DisablePlaneDetection();
                OnSpawnMainPlane?.Invoke(spawnedObject);
                ARGameEventManager.Instance.RaiseEvent(BaseGameEventManager.StartGameEventName);
                //Debug.Log(spawnedObject.transform.localPosition);
                //spawnedObject.transform.LookAt(ARCamera.transform);
                //var rotation = spawnedObject.transform.rotation;
                //spawnedObject.transform.Rotate(rotation.x + rotateObject.x, rotation.y + rotateObject.y, rotation.z + rotateObject.z);


            }
            //else
            //{
            //    spawnedObject.transform.position = hitPose.position;
            //    spawnedObject.transform.LookAt(ARCamera.transform);
            //    var rotation = spawnedObject.transform.rotation;
            //    spawnedObject.transform.Rotate(rotation.x + rotateObject.x, rotation.y + rotateObject.y, rotation.z + rotateObject.z);
            //}

            placementUpdate.Invoke();
        }
    }

    public void DiableVisual()
    {
        visualObject.SetActive(false);
    }

    private void DisablePlaneDetection()
    {
        m_PlaneManager.enabled = false;
        foreach (var plane in m_PlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARPlaneManager m_PlaneManager;
    ARRaycastManager m_RaycastManager;
}
