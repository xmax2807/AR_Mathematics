using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField]private float angularSpeed = 5f;
    [SerializeField] private Vector3 rotateDirection = Vector3.one;
    private Transform thisTrans;
    void Start(){
        thisTrans = transform;
    }
    void Update()
    {
        thisTrans.Rotate(rotateDirection * angularSpeed * Time.deltaTime);
    }
}
