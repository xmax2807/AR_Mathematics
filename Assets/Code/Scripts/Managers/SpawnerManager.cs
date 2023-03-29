using System.Collections;
using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;

namespace Project.Managers
{
    public class SpawnerManager : MonoBehaviour
    {
        public static SpawnerManager Instance {get; private set;}

        protected void Awake()
        {
            if(Instance == null){
                Instance = this;
            }
            else if(Instance != null && Instance != this){
                Destroy(this);
            }
        }
        public void SpawnObjectInParent<T>(T gameObj, Transform theParent, Action<T> onBuildObj = null) where T : MonoBehaviour
        {
            T newGameObj;
            newGameObj = Instantiate(gameObj, theParent);
            onBuildObj?.Invoke(newGameObj);
        }
        public void SpawnObject<T>(T gameObj, Vector3 newPosition, Transform theParent = null, Action<T> onBuildObj = null) where T : MonoBehaviour
        {
            T newGameObj;
            newGameObj = Instantiate(gameObj, newPosition, Quaternion.identity);
            newGameObj.transform.SetParent(theParent);

            onBuildObj?.Invoke(newGameObj);
        }
        public void SpawnObjects<T>(T origin, int count, Action<T> onBuildObj = null, Transform theParent = null) where T : MonoBehaviour
        {
            ListExtensionMethods.SplitLoop(count, 10,
                () => StartCoroutine(
                    SpawnObjectsAsync(origin, origin.transform.position, theParent,10,onBuildObj)
                ));
        }
        public void SpawnObjectsList<T>(T origin, int count,Transform theParent, Action<T,int> onBuildItem = null) where T : MonoBehaviour{
            for(int i = 0; i < count; i++){
                SpawnObjectInParent(origin, theParent,
                    (obj)=>{
                        onBuildItem?.Invoke(obj, i);
                    }
                );
            }
        }
        public void SpawnObjectsRandomly<T>(T origin, int count, Func<Vector3> randomMethod, Func<Vector3, bool> canSpawnCondition, Transform theParent = null) where T : MonoBehaviour
        {
            ListExtensionMethods.SplitLoop(count, 10,
                () => StartCoroutine(
                    SpawnObjectRandomlyAsync(origin, randomMethod, canSpawnCondition, theParent)
                ));
        }
        public IEnumerator SpawnObjectsRandomlyAsync<T>(T gameObj, Func<Vector3> randomMethod, Func<Vector3, bool> canSpawnCondition, Transform newParent = null, int count = 10) where T : MonoBehaviour
        {
            for (int i = 0; i < count; i++)
            {
                bool shouldStop = false;
                TimeCoroutineManager.Instance.WaitForSeconds(1, ()=>shouldStop = true);
                Vector3 newPosition;
                do
                {
                    newPosition = randomMethod.Invoke();
                }
                while (!shouldStop && canSpawnCondition != null && !canSpawnCondition.Invoke(newPosition));

                SpawnObject(gameObj, newPosition, newParent);
                yield return new WaitForEndOfFrame();
            }
        }
        public IEnumerator SpawnObjectsAsync<T>(T gameObj, Vector3 newPosition, Transform newParent = null, int count = 10, Action<T> onBuildObj = null) where T : MonoBehaviour
        {
            for (int i = 0; i < count; i++)
            {
                SpawnObject(gameObj, newPosition, newParent,onBuildObj);
                yield return new WaitForEndOfFrame();
            }
        }
        public IEnumerator SpawnObjectRandomlyAsync<T>(T gameObj, Func<Vector3> randomMethod, Func<Vector3, bool> canSpawnCondition, Transform newParent = null) where T : MonoBehaviour
        {
            Vector3 newPosition;
            bool shouldStop = false;
            TimeCoroutineManager.Instance.WaitForSeconds(1, ()=>shouldStop = true);
            do
            {
                newPosition = randomMethod.Invoke();
            }
            while (!shouldStop && canSpawnCondition != null && !canSpawnCondition.Invoke(newPosition));

            SpawnObject(gameObj, newPosition, newParent);
            yield return new WaitForEndOfFrame();
        }
    }
}