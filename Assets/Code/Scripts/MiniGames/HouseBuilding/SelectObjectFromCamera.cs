using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectObjectFromCamera : MonoBehaviour
{

    [SerializeField]
    private PlacementObject[] placedObjects;

    [SerializeField]
    private Camera arCamera;

    private Vector2 touchPosition = default;

    [SerializeField]
    private bool displayCanvas = true;

    void Awake()
    {
        
    }

    void Update()
    {
        // touch object in camera android
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                // if we got a hit meaning that it was selected
                if (Physics.Raycast(ray, out hitObject))
                {
                    PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
                    
                    if (placementObject != null)
                    {
                        ChangeSelectedObject(placementObject);
                    }
                } // nothing selected so set the sphere color to inactive
                else
                {
                    ChangeSelectedObject();
                }
            }
        }
    }

    void ChangeSelectedObject(PlacementObject selected = null)
    {
        foreach (PlacementObject current in placedObjects)
        {
            MeshRenderer meshRenderer = current.GetComponent<MeshRenderer>();
            if (selected != current)
            {
                current.Selected = false;
            }
            else
            {
                current.Selected = true;
                current.transform.position = new Vector3(0, 10f, 0);
                current.Selected = false;
            }
        }
    }
}
