using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TileManager tileManager;
    private string currentAnimName = "idle";
    private float currentSpeed = -2;
    public enum OctopusState
    {
        BasicJump,
        HighJump,
        FailedJump
    }
    private OctopusState currentState = OctopusState.BasicJump;
    public OctopusState CurrentState
    {
        get => currentState; set
        {
            if (currentState != value)
            {
                currentState = value;
            }
        }
    }
    public Rigidbody myRigidbody;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ChangeState(OctopusState.HighJump);
            return;
        }
        if (Input.GetKey(KeyCode.B))
        {
            ChangeState(OctopusState.FailedJump);
            return;
        }

        if (Input.GetKey(KeyCode.C))
        {
            ChangeState(OctopusState.BasicJump);
        }
    }
    public void ChangeState(OctopusState state)
    {
        currentState = state;
        animator.SetBool(currentAnimName, false);
        tileManager.SetSpeed(currentSpeed);
        if (currentState == OctopusState.HighJump)
        {
            currentAnimName = "jump";
        }
        else if (currentState == OctopusState.FailedJump)
        {
            currentAnimName = "fail";
        }
        else
        {
            currentAnimName = "idle";
        }
        animator.SetBool(currentAnimName, true);
    }
    public void SpawnTile(){
        tileManager.SpawnTileRandomly();
    }

    internal void SetTileManager(TileManager tileManager)
    {
        this.tileManager = tileManager;
    }
}
