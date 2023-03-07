using UnityEngine;
using Project.Managers;
using Unity​Engine.XR.Provider;

namespace Project.MiniGames.FishingGame
{
    public class Fish : BaseCharacter, IVisibleObject
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private ShapeBoard board;

        protected override void Awake()
        {
            base.Awake();
            board.transform.position += _renderer.bounds.size.y / 2 * Vector3.up;
        }
        public void OnBecameInvisible() => ToggleVisible(false);
        
        public void OnBecameVisible() => ToggleVisible(true);
        public void ToggleVisible(bool isVisible)
        {
            _renderer.enabled = isVisible;
            Animator.enabled = isVisible;
            board.enabled = isVisible;
        }

        protected override void InitFiniteStateMachine()
        {
            StateMachine = new FiniteStateMachine<Fish>(this);
            factory = new FishStateFactory(this);
        }
    }
}