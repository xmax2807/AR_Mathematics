namespace Project.MiniGames.FishingGame
{
    public interface IFiniteStateMachine{
        void LogicUpdate();
        void PhysicsUpdate();
        void ChangeState(IState newState);
    }
    public class FiniteStateMachine<T> : IFiniteStateMachine where T : BaseCharacter{
        public IState CurrentState {get;private set;}
        public readonly T Host;
        public FiniteStateMachine(T host){
            Host = host;
        }
        public FiniteStateMachine(T host, IState initState) : this(host){
            if(initState == null){
                UnityEngine.Debug.LogWarning("initState is null");
            }
            CurrentState = initState;
        }
        public void LogicUpdate() => CurrentState.LogicUpdate();
        public void PhysicsUpdate() => CurrentState.PhysicsUpdate();
        public void ChangeState(IState newState){
            CurrentState?.Exit();
            newState?.Enter();
            CurrentState = newState;
        }
    }
}
