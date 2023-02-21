using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Project.MiniGames.FishingGame
{
    public class Fish : BaseCharacter
    {
        protected override void InitFiniteStateMachine()
        {
            StateMachine = new FiniteStateMachine<Fish>(this);
        }
    }
}