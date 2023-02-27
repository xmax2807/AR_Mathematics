using System;
using UnityEngine;
using UnityEngine.Events;

namespace Project.MiniGames.FishingGame{
    public class FishingRod : MonoBehaviour{
        private Fish Attached;
        public UnityEvent<Fish> OnCaught;

        void OnTriggerEnter(Collider collider){
            if(!collider.TryGetComponent(out Fish result)){
                return;
            }
            Attached = result;
            OnCaught?.Invoke(result);
        }
        
    }
}