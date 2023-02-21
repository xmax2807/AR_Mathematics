using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.MiniGames.FishingGame
{
    internal class FishMovementState : FishBaseState
    {
        public FishMovementState(FiniteStateMachine<Fish> finiteStateMachine, string animName) : base(finiteStateMachine, animName)
        {
        }
    }
}
