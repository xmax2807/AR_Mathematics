using System.Collections.Generic;
using UnityEngine;

namespace Project.MiniGames
{
    [CreateAssetMenu(menuName = "MiniGames/GameEventSTO/BaseGameEventSTO", fileName ="GameEventSTO")]
    public class EventSTO : ScriptableObject
    {
        private readonly List<IEventListener> listeners = new(5);
        void OnDisable()
        {
            listeners.Clear();
        }
        public void Raise()
        {
            Debug.Log(listeners.Count);
            for (int i = listeners.Count - 1; i >= 0; i--){
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(IEventListener listener) => listeners.Add(listener);

        public void UnregisterListener(IEventListener listener) {
            Debug.Log("Cleared");
            listeners.Remove(listener); 
        }
    }
}