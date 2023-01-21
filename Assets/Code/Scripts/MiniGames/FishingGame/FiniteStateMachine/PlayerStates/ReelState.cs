namespace Project.MiniGames.FishingGame{
    public class ReelState : State<Player>
    {
        public ReelState(FiniteStateMachine<Player> finiteStateMachine, string animName) : base(finiteStateMachine, animName)
        {
        }
    }
}