using UnityEngine;
using System.Collections;
using System;

namespace Project.Managers{
    public class TimeCoroutineManager : MonoBehaviour{
    public static TimeCoroutineManager Instance {get;private set;}
    protected void Awake(){
        if(Instance == null){
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    protected void OnDestroy(){
        StopAllCoroutines();
    }

    public Coroutine WaitFor(YieldInstruction instruction, Action result){
        return StartCoroutine(ExecuteWaitCoroutine(instruction, result));
    }
    public Coroutine WaitForSeconds(float seconds, Action result){
        float timeout = Time.time + seconds;
        return StartCoroutine(ExecuteWaitCoroutine(()=>Time.time >= timeout, result));
    }
    public Coroutine WaitUntil(Func<bool> condition, Action result) => StartCoroutine(ExecuteWaitCoroutine(condition, result));
    private IEnumerator ExecuteWaitCoroutine(Func<bool> condition, Action result){
        yield return new WaitUntil(condition);
        result?.Invoke();
    }

    private IEnumerator ExecuteWaitCoroutine(YieldInstruction condition, Action result){
        yield return condition;
        result?.Invoke();
    }

    public Coroutine DoLoopAction(Action action, float Duration, float delayInterval = 0, Action endAction = null){
        float timeOut = Time.time + Duration;
        bool stopCondition() => Time.time >= timeOut;
        return StartCoroutine(ExecuteLoopAction(stopCondition, action, delayInterval, endAction));
    }
    public Coroutine DoLoopAction(Action action, Func<bool> stopCondition, float delayInterval = 0, Action endAction = null){
        return StartCoroutine(ExecuteLoopAction(stopCondition, action, delayInterval, endAction));
    }
    private IEnumerator ExecuteLoopAction(Func<bool> stopCondition, Action action, float delayInterval = 0, Action end = null){
        if(stopCondition != null){
            var delayTicker = new WaitForSeconds(delayInterval);
            while(!stopCondition.Invoke()){
                action?.Invoke();
                yield return delayTicker;
            }
        }
        end?.Invoke();
    }
}
}