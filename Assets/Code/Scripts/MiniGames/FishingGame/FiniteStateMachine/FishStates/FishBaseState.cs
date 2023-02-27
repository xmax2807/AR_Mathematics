namespace Project.MiniGames.FishingGame{
    public class FishBaseState : State<Fish>
    {
        protected FishStateFactory Factory => (FishStateFactory)host.factory;
        public FishBaseState(Fish host, string animName) : base(host, animName)
        {
        }
    }
}