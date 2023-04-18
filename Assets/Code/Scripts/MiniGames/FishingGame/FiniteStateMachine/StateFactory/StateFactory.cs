namespace Project.MiniGames.FishingGame{
    public interface IStateFactory{
    }
    public abstract class StateFactory<T> : IStateFactory where T : BaseCharacter{
        protected T host;

        public StateFactory(T host){
            this.host = host;
            CreateStates();
        }

        protected abstract void CreateStates();
    }
    public class FishStateFactory : StateFactory<Fish>
    {
        public FishIdleState IdleState {get;private set;}
        public FishMovementState MovementState {get;private set;}
        public FishCaughtState CaughtState {get;private set;}
        public FishStateFactory(Fish host) : base(host){}

        protected override void CreateStates()
        {
            IdleState = new (host, "idle");
            MovementState = new (host, "move");
            CaughtState = new(host, "caught");
            
            host.StateMachine.ChangeState(IdleState);
        }
    }
    public class PlayerStateFactory : StateFactory<Player>
    {
        public IdleState IdleState {get;private set;}
        public ReelState ReelState {get;private set;}
        public PlayerStateFactory(Player host) : base(host){}

        protected override void CreateStates()
        {
            IdleState = new (host, "idle");
            ReelState = new (host, "reel");

            host.StateMachine.ChangeState(IdleState);
        }
    }
}