using System;
using System.Collections;
using Project.Managers;
using Project.MiniGames;
using UnityEngine;

public class PlayerController : MonoBehaviour, IEventListener
{
    [SerializeField] protected Animator animator;
    private string currentAnimName = "idle";
    public enum PlayerState
    {
        Idle,
        BasicJump,
        HighJump,
        FailedJump
    }
    private PlayerState currentState = PlayerState.Idle;
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
            UpdateState();
            Die();
        }
        else
        {
            currentAnimName = "idle";
        }
        animator.SetBool(currentAnimName, true);
    }

    protected void Die(){
        float animationLen = animator.GetCurrentAnimatorStateInfo(0).length;
        TimeCoroutineManager.Instance.WaitForSeconds(animationLen, ()=>{
            VCNVGameEventManager.Instance.RaiseEvent(VCNVGameEventManager.EndGameEventName, false);
        });
    }
    

    #region GameEvents
    public string UniqueName => this.name;
    void OnEnable(){
        VCNVGameEventManager.Instance.RegisterEvent<bool>(VCNVGameEventManager.AnswerResultEventName,this, OnAnswerQuestion);
        BaseGameEventManager.Instance.RegisterEvent(BaseGameEventManager.StartGameEventName, this, OnGameStarted);
    }

    protected virtual void OnGameStarted()
    {
        ChangeState(PlayerState.BasicJump);
    }

    public void OnEventRaised<T>(EventSTO sender, T result){}

    private void OnAnswerQuestion(bool result){
        Debug.Log("StateChanged with result: " + result);
        if(result == true){
            ChangeState(PlayerState.HighJump);
            TimeCoroutineManager.Instance.WaitForSeconds(0.2f, ()=>ChangeState(PlayerState.BasicJump));
            //ChangeState(PlayerState.BasicJump);
        }
        else{
            ChangeState(PlayerState.FailedJump);
        }
    }
    
    #endregion
}
