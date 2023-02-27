

using Project.Managers;
namespace Project.MiniGames.FishingGame
{
    public class FishIdleState : FishBaseState
    {
        private UnityEngine.Coroutine idleCoroutine;
        public FishIdleState(Fish host, string animName) : base(host, animName)
        {
        }

        public override void Enter()
        {
            idleCoroutine = TimeCoroutineManager.Instance.WaitForSeconds(2f,()=>stateMachine.ChangeState(Factory.MovementState));
            base.Enter();
        }
        public override void Exit()
        {
            TimeCoroutineManager.Instance.StopCoroutine(idleCoroutine);
            base.Exit();
        }
    }
}
