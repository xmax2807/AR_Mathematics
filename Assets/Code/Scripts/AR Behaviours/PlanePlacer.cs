using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Project.ARBehaviours
{
    [RequireComponent(typeof(ARPlaneManager), typeof(ARRaycastManager))]
    public class PlanePlacer : MonoBehaviour, IPlanePlacer
    {
        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_PlaneManager = GetComponent<ARPlaneManager>();

            ARCamera ??= Camera.main;
        }

        public void SetPrefab(GameObject newPlace)
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
                
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, m_PlacedPrefab.transform.rotation);
                spawnedObject.AddComponent<ARAnchor>();
                DisablePlaneDetection();
                OnSpawnMainPlane?.Invoke(spawnedObject);
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

        private void EnablePlaneDetection()
        {
            m_PlaneManager.enabled = true;
            foreach (var plane in m_PlaneManager.trackables)
            {
                plane.gameObject.SetActive(true);
            }
        }

        public void TurnOffPlaneDetector()
        {
            this.enabled = false;
            DisablePlaneDetection();
        }

        public void TurnOnPlaneDetector()
        {
            this.enabled = true;
            EnablePlaneDetection();
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARPlaneManager m_PlaneManager;
        ARRaycastManager m_RaycastManager;
        private GameObject m_PlacedPrefab;
        private GameObject spawnedObject;
        public event Action<GameObject> OnSpawnMainPlane;
        private object ARCamera;
    }
}