using System;
using System.Collections;
using Project.MiniGames;
using UnityEngine;

public class PlayerController : MonoBehaviour, IEventListenerT<bool> 
{
    [SerializeField] private Animator animator;
    private string currentAnimName = "idle";
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

    public string Name => throw new NotImplementedException();

    public Rigidbody myRigidbody;

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKey(KeyCode.A))
    //     {
    //         ChangeState(PlayerState.HighJump);
    //         return;
    //     }
    //     if (Input.GetKey(KeyCode.B))
    //     {
    //         ChangeState(PlayerState.FailedJump);
    //         return;
    //     }

    //     if (Input.GetKey(KeyCode.C))
    //     {
    //         ChangeState(PlayerState.BasicJump);
    //     }
    // }
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
    

    #region GameEvents
    void OnDisable(){
        this.colliderEvent?.UnregisterListener(this);
        this.quizEvent?.UnregisterListener(this);
    }
    private EventSTO colliderEvent;
    public void SetColliderEvent(EventSTO colliderEvent){
        this.colliderEvent = colliderEvent;
        this.colliderEvent?.RegisterListener(this);
    }
    public EventSTO GetEventSTO()
    {
        return colliderEvent;
    }

    private QuizEventSTO quizEvent;
    public void SetQuizEvent(QuizEventSTO quizEvent){
        this.quizEvent = quizEvent;
        this.quizEvent?.RegisterListener(this);
    }
    public void OnEventRaised(bool result)
    {
        if(result == true){
            ChangeState(PlayerState.HighJump);
            Project.Managers.TimeCoroutineManager.Instance.WaitForSeconds(1, ()=>ChangeState(PlayerState.BasicJump));
        }
        else{
            ChangeState(PlayerState.FailedJump);
        }
    }

    public void OnEventRaised()
    {
        
    }

    
    #endregion
}
