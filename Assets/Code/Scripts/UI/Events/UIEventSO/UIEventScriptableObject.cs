using System.Collections.Generic;
using UnityEngine;
namespace Project.UI.Event{
    [CreateAssetMenu(fileName = "UIEventSO", menuName = "STO/UIEvent/UIEventSO")]
    public class UIEventScriptableObject : ScriptableObject, IUIEvent
    {
        protected List<IUIEventListener> listeners;
        public void AddListener(IUIEventListener listener)
        {
            listeners ??= new();
            listeners.Add(listener);
        }

        public void RemoveListener(IUIEventListener listener)
        {
            if(listeners == null) return;

            int count = listeners.Count;
            for(int i =0 ; i < count; ++i){
                if(listeners[i].UniqueID == listener.UniqueID){
                    listeners.RemoveAt(i);
                    break;
                }
            }
        }

        public void ShowUI(){
            if(listeners == null) return;

            int count = listeners.Count;
            for(int i =0 ; i < count; ++i){
                listeners[i].ShowUI();
            }
        }
        public void HideUI(){
            if(listeners == null) return;

            int count = listeners.Count;
            for(int i =0 ; i < count; ++i){
                listeners[i].HideUI();
            }
        }
    }
}