namespace Project.MiniGames.FishingGame{
    public class ThrowState : State<Player>
    {
        public ThrowState(FiniteStateMachine<Player> finiteStateMachine, string animName) : base(finiteStateMachine, animName)
        {
        }
    }
}