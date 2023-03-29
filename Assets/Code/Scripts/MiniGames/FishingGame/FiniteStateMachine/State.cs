using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.MiniGames.FishingGame
{
    public interface IState{
        public void Enter();
        public void Exit();
        public void LogicUpdate();
        public void PhysicsUpdate();
        public void OnAnimationFinish();
    }
    public abstract class State<T> : IState where T : BaseCharacter
    {
        protected T host;
        protected Animator animator => stateMachine.Host.Animator;
        protected string animName;
        protected FiniteStateMachine<T> stateMachine;
        protected bool isAnimationFinish;
        public State(T host, string animName){
            this.host = host;
            stateMachine = (FiniteStateMachine<T>)host.StateMachine;
            this.animName = animName;
            animator.logWarnings = false;
        }

        public virtual void Enter(){
            isAnimationFinish = false;

            if(animator.runtimeAnimatorController == null || !animator.GetBool(animName)) return;
            
            animator.SetBool(animName, true);    
        }
        public virtual void Exit(){
            if(animator.runtimeAnimatorController == null || !animator.GetBool(animName)) return;

            animator.SetBool(animName, false);
        }
        public virtual void LogicUpdate(){}
        public virtual void PhysicsUpdate(){}
        public virtual void OnAnimationFinish() => isAnimationFinish = true;
    }
}
