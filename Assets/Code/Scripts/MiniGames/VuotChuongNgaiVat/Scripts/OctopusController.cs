using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusController : MonoBehaviour
{
    enum OctopusState{
        BasicJump,
        HighJump,
        FailedJump
    }
    private OctopusState currentState = OctopusState.BasicJump;
    public Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (currentState == OctopusState.HighJump)
        {
            
        }
        else if(currentState == OctopusState.FailedJump)
        {

        }
        else
        {

        }
    }
}
