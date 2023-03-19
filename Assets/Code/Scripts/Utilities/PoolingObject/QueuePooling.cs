using System.Collections.Generic;
using UnityEngine;
using Project.Managers;

namespace Project.Utils.ObjectPooling
{
    public class QueuePooling<T> where T : MonoBehaviour, IPooling
    {
        protected int CloneNumber;
        private readonly T prefabSample;
        private readonly Transform parent;
        public QueuePooling(int clone, T Sample, Transform parent)
        {
            CloneNumber = clone;
            prefabSample = Sample;
            this.parent = parent;
            poolQueue = new();

            PopulateObj();
        }
        private readonly Queue<T> poolQueue;

        public void AddToQueue(T obj)
        {
            obj.OnReturn();
            poolQueue.Enqueue(obj);
        }
        public void AddToQueue(IPooling obj)
        {
            AddToQueue((T)obj);
        }

        public T GetObj()
        {
            if (poolQueue.Count <= 0)
            {
                PopulateObj();
            }
            var obj = poolQueue.Dequeue();
            obj.OnRelease();
            return obj;
        }
        private void PopulateObj()
        {
            SpawnerManager.Instance.SpawnObjectsList(
                prefabSample,
                CloneNumber,
                parent,
                (newObj, i) =>
                {
                    newObj.AddBackToQueue += AddToQueue;
                    AddToQueue(newObj);
                });
        }
    }

}