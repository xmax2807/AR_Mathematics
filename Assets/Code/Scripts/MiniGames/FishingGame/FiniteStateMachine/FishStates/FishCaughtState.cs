using UnityEngine;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.FishingGame{
    public class FishCaughtState : FishBaseState
    {
        private Vector3 playerPosition;
        private Transform hostTransform;
        private Vector3 velocity = Vector3.zero;
        private Vector3 originSpawnPosition;
        public FishCaughtState(Fish host, string animName) : base(host, animName)
        {
            hostTransform = host.transform;
            originSpawnPosition = hostTransform.position;
            var mainCam = Camera.main;
            playerPosition = mainCam == null? Vector3.up * 100 : mainCam.transform.position.y * Vector3.up;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            hostTransform.LookAtUpdate(playerPosition, 6f);
            hostTransform.position = Vector3.SmoothDamp(hostTransform.position, playerPosition,ref velocity, smoothTime: 5f * Time.deltaTime);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if(hostTransform.position.IsNear(playerPosition)){
                hostTransform.position = VectorExtensionMethods.SpawnPoint(originSpawnPosition, new Vector3(1, 0, 1), 10f);
                stateMachine.ChangeState(Factory.IdleState);
                host.ShuffleBoard();
            }
        }
    }
}