using UnityEngine;
using System.Collections.Generic;

namespace Project.Utils.ObjectPooling{
    public class ListPooling<T> : List<T> where T : MonoBehaviour, IPooling{
        public bool createMoreIfNeeded = true;

        private Transform mParent;
        private Vector3 mStartPos;
        private GameObject referenceObject;

        public delegate void ObjectCreationCallback(T obj);
        public event ObjectCreationCallback OnObjectCreationCallBack;

        public ListPooling(GameObject refObject, Transform parent): this(0, refObject, parent)
        {
        }

        public ListPooling(int amount, GameObject refObject, Transform parent, bool startState = false) 
        : this(amount, refObject, parent, Vector3.zero, startState)
        {
        }

        public ListPooling (int amount, GameObject refObject, Transform parent, Vector3 worldPos, bool startState = false)
        {
            mParent = parent;
            mStartPos = worldPos;
            referenceObject = refObject;

            Clear();

            for (var i = 0; i < amount; i++)
            {
                var obj = CreateObject();

                if(startState) obj.OnReturn();
                else obj.OnRelease();

                Add(obj);
            }
        }
        
        public T Collect(Transform parent = null, Vector3? position = null, bool localPosition = true)
        {
            var obj = Find(x => x.IsUsing == false);
            if (obj == null && createMoreIfNeeded)
            {
                obj = CreateObject(parent, position);
                Add(obj);
            }

            if (obj == null) return obj;

            obj.transform.SetParent(parent ?? mParent);
            if (localPosition)
                obj.transform.localPosition = position ?? mStartPos;
            else
                obj.transform.position = position ?? mStartPos;
            obj.OnRelease();

            return obj;
        }

        public void Release(T obj)
        {
			obj?.OnReturn();
        }

        public List<T> GetAllWithState(bool active)
        {
            return FindAll(x => x.IsUsing == active);
        }

        private T CreateObject(Transform parent = null, Vector3? position = null)
        {
            var go = GameObject.Instantiate(referenceObject, position ?? mStartPos, Quaternion.identity, parent ?? mParent);
            var obj = go.GetComponent<T>() ?? go.AddComponent<T>();
            obj.transform.localPosition = position ?? mStartPos;
            obj.name = obj.Name + Count;

			OnObjectCreationCallBack?.Invoke(obj);

            return obj;
        }
    }
}