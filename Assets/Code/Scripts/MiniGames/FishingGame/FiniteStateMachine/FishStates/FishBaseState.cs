namespace Project.MiniGames.FishingGame{
    internal class FishBaseState : State<Fish>
    {
        public FishBaseState(FiniteStateMachine<Fish> finiteStateMachine, string animName) : base(finiteStateMachine, animName)
        {
        }
    }
}