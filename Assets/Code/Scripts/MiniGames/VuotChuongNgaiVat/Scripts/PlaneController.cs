using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 direction;
    public float forwardSpeed = -2;
    void Start()
    {
        direction = Vector3.right * forwardSpeed;
    }

    private void Update(){
        this.transform.position += direction * Time.deltaTime;
    }
}
