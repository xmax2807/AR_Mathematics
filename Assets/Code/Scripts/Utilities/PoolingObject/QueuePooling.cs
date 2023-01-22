using System.Collections.Generic;
using UnityEngine;

namespace Project.Utils.ObjectPooling
{
    public class QueuePooling<T> : MonoBehaviour where T :MonoBehaviour, IPooling{
        [SerializeField] protected int CloneNumber;
        [SerializeField] private T prefabSample;
        public static QueuePooling<T> Instance {get;private set;}
        private void Awake(){
            if(Instance == null){            
                Instance = this;
            }
        }
        private Queue<T> poolQueue = new();

        public void AddToQueue(T obj){
            obj.OnReturn();
            poolQueue.Enqueue(obj);
        }
        public void AddToQueue(IPooling obj){
            AddToQueue((T)obj);
        }
        
        public T GetObj(){
            if(poolQueue.Count <= 0){
                PopulateObj();
            }
            var obj = poolQueue.Dequeue();
            obj.OnRelease();
            return obj;
        }
        private void PopulateObj(){
            for(int i = 0; i <  CloneNumber; i++){
                T newObj = Instantiate(prefabSample, this.transform);
                newObj.AddBackToQueue += AddToQueue;
                AddToQueue(newObj);
            }
        }
    }

}