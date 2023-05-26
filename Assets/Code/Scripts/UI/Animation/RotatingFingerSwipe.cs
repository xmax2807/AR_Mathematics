using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFingerSwipe : MonoBehaviour
{
    [Header("Default Setting (recommended)")]
    public float rotationDamping = 5f;
    public float rotationSpeed = 200f;
    public Vector3 currentRotationSpeed;
    public Vector3 targetRotationSpeed;

    private Vector3 centerPoint;
    private Renderer modelRenderer;
    private bool dragging = false;

    void Start()
    {
        modelRenderer = GetComponentInChildren<Renderer>();
        centerPoint = modelRenderer.bounds.center;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            targetRotationSpeed = Vector3.zero;
        }
#else
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                dragging = true;
            }
            else{
                dragging = false;
                targetRotationSpeed =Vector3.zero;
            }
        }
#endif
    }

    void FixedUpdate()
    {
        if (dragging)
        {
            float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

            targetRotationSpeed = new Vector3(y, -x, 0f);
        }
        currentRotationSpeed = Vector3.Lerp(currentRotationSpeed, targetRotationSpeed, rotationDamping * Time.fixedDeltaTime);
        transform.RotateAround(centerPoint, Vector3.up, currentRotationSpeed.y);
        transform.RotateAround(centerPoint, Vector3.right, currentRotationSpeed.x);
    }
}
