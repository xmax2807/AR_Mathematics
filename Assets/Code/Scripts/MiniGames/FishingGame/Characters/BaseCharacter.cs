using System.Collections;
using Project.Utils.ExtensionMethods;
using UnityEngine;

namespace Project.MiniGames.FishingGame
{
    [RequireComponent(typeof(Animator))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        private Animator _animator;
        public Animator Animator => _animator;
        protected IFiniteStateMachine StateMachine;
        protected abstract void InitFiniteStateMachine();  
        protected virtual void Awake(){
            this.EnsureComponent<Animator>(ref _animator);
            InitFiniteStateMachine();
        }
        protected virtual void Update() => StateMachine.LogicUpdate();
        
        protected virtual void FixedUpdate() => StateMachine.PhysicsUpdate();
    }
}
