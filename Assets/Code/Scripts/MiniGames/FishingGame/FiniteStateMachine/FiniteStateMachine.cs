using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.MiniGames.FishingGame
{
    public class FiniteStateMachine<T> where T : BaseCharacter{
        public State<T> CurrentState;
        public T Host;
        public FiniteStateMachine(T host){
            Host = host;
        }
    }
}
