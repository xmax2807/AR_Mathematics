using UnityEngine;
using Project.Utils.ExtensionMethods;
using Project.QuizSystem;
using System.Threading.Tasks;

namespace Project.MiniGames.FishingGame
{
    public class Player : BaseCharacter
    {
        [SerializeField] private FishingRod Rod;
        [SerializeField] private TaskGiver giver;
        public TaskGiver Giver => giver;
        public event System.Action OnCatchRightFish;
        protected override void Awake()
        {
            base.Awake();
            
            if(Rod == null) Rod = this.gameObject.EnsureChildComponent<FishingRod>("FishingRod");
        }
        protected override void InitFiniteStateMachine(){
            StateMachine = new FiniteStateMachine<Player>(this);
            factory = new PlayerStateFactory(this);
        }

        public Task<bool> OnCaughtFish(Shape.ShapeType type){
            if(Giver.IsCorrect(type)){
                Giver.Tasks.UpdateProgress(1);
                OnCatchRightFish?.Invoke();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
