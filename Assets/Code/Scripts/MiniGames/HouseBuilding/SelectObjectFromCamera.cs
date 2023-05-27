using UnityEngine;
using Project.Utils.ExtensionMethods;

public class SelectObjectFromCamera : MonoBehaviour
{
    private Transform spawnParent;
    [SerializeField]
    private PlacementObject[] placedObjects;
    [SerializeField] private LayerMask filteredMask;
    private PlacementObject[] placements;

    [SerializeField]
    private Camera arCamera;

    private void Start(){
        if(arCamera == null){
            arCamera = Camera.main;
        }
    }
    public void SetPlacements(PlacementObject[] placementObjects, Transform placementParent = null){
        placedObjects = placementObjects;
        spawnParent = placementParent;
        onSpawnMainPlane();
    }
    public void onSpawnMainPlane()
    {
        placements = new PlacementObject[placedObjects.Length];
        var clone = placedObjects.Shuffle();

        Vector3 startPosition = new(-6,1,-5);

        for (int i = 0; i < placements.Length; i++)
        {
            var renderer = clone[i].GetComponentInChildren<MeshRenderer>();
            placements[i] = Instantiate(clone[i], spawnParent);
            placements[i].transform.position = startPosition;
            
            var size = renderer.bounds.size;
            Vector3 spacing = new(0.5f,0,0);
            //Debug.Log(size.x);
            startPosition = VectorExtensionMethods.AddExceptZero(startPosition, size, Vector3.right) + spacing;
            //startPosition += spacing;
        }
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
        if (!TryGetTouchPosition(out Vector3 touchPosition)) return;

#if UNITY_EDITOR
        OnTouch(touchPosition);
        return;
#else
        // touch object in camera android
        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            OnTouch(touchPosition);
        }
#endif
    }

    private void OnTouch(Vector3 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);

        // if we got a hit meaning that it was selected
        if (Physics.Raycast(ray, out RaycastHit hitObject, 100,filteredMask))
        {
            PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();

            if (placementObject != null)
            {
                ChangeSelectedObject(placementObject);
            }
        } // nothing selected so set the sphere color to inactive
        // else
        // {
        //     ChangeSelectedObject();
        // }
    }

    void ChangeSelectedObject(PlacementObject selected = null)
    {
        foreach (PlacementObject current in placements)
        {
            //MeshRenderer meshRenderer = current.GetComponent<MeshRenderer>();
            if (selected == current)
            {
                current.transform.position = new Vector3(0, 10f, 0);
                current.transform.rotation = Quaternion.identity;
            }
            // else
            // {
            //     current.Selected = false;
            // }
        }
    }
}
