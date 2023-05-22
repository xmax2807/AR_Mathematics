using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TileManager tileManager;
    private string currentAnimName = "idle";
    private float currentSpeed = -2;
    public enum PlayerState
    {
        BasicJump,
        HighJump,
        FailedJump
    }
    private PlayerState currentState = PlayerState.BasicJump;
    public PlayerState CurrentState
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
            ChangeState(PlayerState.HighJump);
            return;
        }
        if (Input.GetKey(KeyCode.B))
        {
            ChangeState(PlayerState.FailedJump);
            return;
        }

        if (Input.GetKey(KeyCode.C))
        {
            ChangeState(PlayerState.BasicJump);
        }
    }
    void FixedUpdate(){
        UpdateState();
    }

    protected virtual void UpdateState(){}

    public virtual void ChangeState(PlayerState state)
    {
        currentState = state;
        animator.SetBool(currentAnimName, false);
        //tileManager.SetSpeed(currentSpeed);
        if (currentState == PlayerState.HighJump)
        {
            currentAnimName = "jump";
        }
        else if (currentState == PlayerState.FailedJump)
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
