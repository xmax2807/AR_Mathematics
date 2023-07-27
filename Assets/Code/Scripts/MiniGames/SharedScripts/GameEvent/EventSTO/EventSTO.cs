using System.Collections.Generic;
using UnityEngine;

namespace Project.MiniGames
{
    [CreateAssetMenu(menuName = "MiniGames/GameEventSTO/BaseGameEventSTO", fileName ="GameEventSTO")]
    public class EventSTO : ScriptableObject, IEvent
    {
        protected readonly List<IEventListener> listeners = new(5);
        void OnDisable()
        {
            listeners.Clear();
        }
        public void Raise<T>(T result)
        {
            for (int i = listeners.Count - 1; i >= 0; i--){
                listeners[i].OnEventRaised<T>(this, result);
            }
        }
        public void Raise(){
            Raise<bool>(default);
        }

        public void RegisterListener(IEventListener listener) => listeners.Add(listener);

        public void UnregisterListener(IEventListener listener) {
            listeners.Remove(listener); 
        }
        public void ClearEvents() => listeners.Clear();
    }
}