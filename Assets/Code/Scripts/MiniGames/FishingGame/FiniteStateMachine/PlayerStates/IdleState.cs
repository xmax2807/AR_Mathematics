namespace Project.MiniGames.FishingGame{
    public class IdleState : State<Player>
    {
        public IdleState(FiniteStateMachine<Player> finiteStateMachine, string animName) : base(finiteStateMachine, animName)
        {
        }
    }
}