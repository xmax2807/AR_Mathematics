using System.Collections.Generic;
using System;
using UnityEngine;
namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Data/EventCenterSO", fileName = "EventCenter")]
    public class ObjectInteractionEventCenter : ScriptableObject
    {
        [SerializeField] ObjectDataDeliver[] dataDelivers;

        private Dictionary<ObjectDataDeliver, Action<int>> dataDeliverMap = new();

        private void OnEnable(){
            dataDeliverMap.Clear();
            // Add all data delivers from array to map
            for(int i = 0; i < dataDelivers.Length; ++i){
                dataDeliverMap.Add(dataDelivers[i], null);
            }
        }

        public void Subscribe(ObjectDataDeliver deliverToSubscribe, Action<int> callback, bool autoAdd = true)
        {
            if(dataDeliverMap.ContainsKey(deliverToSubscribe)){
                dataDeliverMap[deliverToSubscribe] += callback;
            }
            else{
                if(autoAdd){
                    dataDeliverMap.Add(deliverToSubscribe, null);
                    dataDeliverMap[deliverToSubscribe] += callback;
                }
                else{
                    // Can't find any deliver to subscribe
                    Debug.Log("Tried to subscribe to unavailable deliver");
                }
            }
        }

        public void InvokeEvent(ObjectDataDeliver targetDeliver, int objectIndex)
        {
            if (dataDeliverMap.TryGetValue(targetDeliver, out Action<int> callback))
            {
                callback?.Invoke(objectIndex);
            }
            else{
                Debug.Log("Tried to invoke unavailable deliver");
            }
        }
        public void Unsubscribe(ObjectDataDeliver deliverToSubscribe, Action<int> callback)
        {
            if(dataDeliverMap.ContainsKey(deliverToSubscribe)){
                dataDeliverMap[deliverToSubscribe] -= callback;
            }
            else{
                // Can't find any deliver to subscribe
                Debug.Log("Tried to unsubscribe to unavailable deliver");
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            dataDeliverMap.Clear();
            for(int i = 0; i < dataDelivers.Length; ++i){
                dataDeliverMap.Add(dataDelivers[i], null);
            }
        }
#endif
    }
}