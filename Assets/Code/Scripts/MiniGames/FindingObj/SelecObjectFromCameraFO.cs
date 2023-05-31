using Project.MiniGames;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SelecObjectFromCameraFO : MonoBehaviour
{
    [SerializeField] private LayerMask filteredMask;
    [SerializeField] private Transform spawnParent;
    [SerializeField]
    private PlacementObject[] placedObjects;

    private PlacementObject[] placements;

    [SerializeField]
    private Camera arCamera;

    private Vector2 touchPosition = default;

    [SerializeField]
    private bool displayCanvas = true;

    private ObjectPositionQuestion aimText;
    private TaskGiver taskGiver;

    public void onSpawnMainPlane()
    {
        taskGiver = FindObjectOfType<TaskGiver>();
        if(taskGiver != null){
            
        }
        placements = new PlacementObject[placedObjects.Length];
        for (int i = 0; i < placements.Length; i++)
        {
            placements[i] = Instantiate(placedObjects[i], spawnParent);
        }
    }

    public void SetPlacements(PlacementObject[] placements, Transform spawnParent){
        this.spawnParent = spawnParent;
        
        placedObjects = placements;
        
        this.placements = new PlacementObject[placedObjects.Length];
        for (int i = 0; i < placedObjects.Length; i++)
        {
            this.placements[i] = Instantiate(placedObjects[i], spawnParent);
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
        if (Physics.Raycast(ray, out RaycastHit hitObject, 100, filteredMask))
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
            if (selected == current)
            {
                // current.Selected = true;
                Animator anim = current.GetComponentInChildren<Animator>(true);
                bool IsCorrect = taskGiver.CurrentTask.IsCorrect(current.ID);
                if (IsCorrect == true)
                {
                    taskGiver.Tasks.UpdateProgress(1);
                }
                else
                {
                    Debug.Log("Fail");
                }

                StartCoroutine(VideoStart(anim,IsCorrect));
                // current.Selected = false;                
            }
        }
    }
    IEnumerator VideoStart(Animator anim, bool RightAns)
    {
        anim.gameObject.SetActive(true);
        if(RightAns == true)
        {
            anim.Play("RightAnswerAnimation");
        }
        else
        {
            anim.Play("WrongAnswerAnimation");
        }
        anim.transform.LookAt(arCamera.transform);
        yield return new WaitForSeconds(2);
        anim.gameObject.SetActive(false);
        Debug.Log("Run video");
    }
}
