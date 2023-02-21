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
        protected Animator animator => stateMachine.Host.Animator;
        protected string animName;
        protected FiniteStateMachine<T> stateMachine;
        protected bool isAnimationFinish;
        public State(FiniteStateMachine<T> finiteStateMachine, string animName){
            stateMachine = finiteStateMachine;
            this.animName = animName;
        }

        public virtual void Enter(){
            isAnimationFinish = false;
            animator.SetBool(animName, true);    
        }
        public virtual void Exit(){
            animator.SetBool(animName, false);
        }
        public virtual void LogicUpdate(){}
        public virtual void PhysicsUpdate(){}
        public virtual void OnAnimationFinish() => isAnimationFinish = true;
    }
}
