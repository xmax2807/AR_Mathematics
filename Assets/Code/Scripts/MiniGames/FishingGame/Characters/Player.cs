using UnityEngine;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.FishingGame
{
    public class Player : BaseCharacter
    {
        [SerializeField] private FishingRod Rod;
        protected override void Awake()
        {
            base.Awake();
            
            if(Rod == null) Rod = this.gameObject.EnsureChildComponent<FishingRod>("FishingRod");
        }
        protected override void InitFiniteStateMachine(){
            StateMachine = new FiniteStateMachine<Player>(this);
            factory = new PlayerStateFactory(this);
        }
    }
}
