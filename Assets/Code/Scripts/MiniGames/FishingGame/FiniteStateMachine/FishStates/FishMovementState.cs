using Project.Utils.ExtensionMethods;
using UnityEngine;

namespace Project.MiniGames.FishingGame
{
    public class FishMovementState : FishBaseState
    {
        private readonly Vector3[] Points;
        private int currentPointIndex;
        private Vector3 velocity = Vector3.zero;
        private Vector3 CurrentPosition { get => host.transform.position; set => host.transform.position = value; }
        public FishMovementState(Fish host, string animName) : base(host, animName)
        {
            Points = VectorExtensionMethods.SpawnPoints(CurrentPosition, new Vector3(1, 0, 1), 50f);
        }
        public override void LogicUpdate()
        {
            host.transform.LookAtUpdate(Points[currentPointIndex], 6f);
            CurrentPosition = Vector3.SmoothDamp(CurrentPosition, Points[currentPointIndex],ref velocity, smoothTime: 400f * Time.deltaTime);

            base.LogicUpdate();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (CurrentPosition.IsNear(Points[currentPointIndex], 3f))
            {
                stateMachine.ChangeState(Factory.IdleState);
            }
        }
        public override void Exit()
        {
            currentPointIndex = (currentPointIndex + 1) % Points.Length;
            base.Exit();
        }
    }
}
