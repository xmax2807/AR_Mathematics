using System.Collections.Generic;
using UnityEngine;

namespace Project.MiniGames{
    [CreateAssetMenu(menuName = "MiniGames/GameEventSTO/QuizEventSTO", fileName ="QuizEventSTO")]
    public class QuizEventSTO : UnityEngine.ScriptableObject{
        private readonly List<IEventListenerT<bool>> listeners = new(5);
        void OnDisable()
        {
            listeners.Clear();
        }
        public void Raise(bool result)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised(result);
        }

        public void RegisterListener(IEventListenerT<bool> listener) => listeners.Add(listener);

        public void UnregisterListener(IEventListenerT<bool> listener) => listeners.Remove(listener); 
    }
}