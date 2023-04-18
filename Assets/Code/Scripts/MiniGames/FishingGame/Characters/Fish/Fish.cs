using UnityEngine;
using Project.Managers;
using Unity​Engine.XR.Provider;
using Project.QuizSystem;
using System.Threading.Tasks;

namespace Project.MiniGames.FishingGame
{
    public class Fish : BaseCharacter, IVisibleObject
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private ShapeBoard board;
        static private Player fisherMan;

        public event System.Func<Shape.ShapeType, Task<bool>> OnBeingCaughtEvent;

        protected override void Awake()
        {
            base.Awake();
            if(fisherMan == null){
                fisherMan = FindAnyObjectByType<Player>();
            }

            board.transform.position += _renderer.bounds.size.y / 2 * Vector3.up;
            board.OnCatchFish += ButtonClick;

            if(fisherMan == null) return;
            OnBeingCaughtEvent += fisherMan.OnCaughtFish;
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
        public void ShuffleBoard(){
            board.UpdateUI();
        }

        private async Task ButtonClick(Shape.ShapeType type){
            bool result = OnBeingCaughtEvent != null && await OnBeingCaughtEvent.Invoke(type);

            if(result){
                OnBeingCaught();
            }
        }

        public void OnBeingCaught(){
            StateMachine.ChangeState(((FishStateFactory)factory).CaughtState);
        }
    }
}