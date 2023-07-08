using System.Collections;
using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;
using System.Threading.Tasks;

namespace Project.Managers
{
    public class SpawnerManager : MonoBehaviour
    {
        public static System.Random RandomInstance {get;private set;}
        public static SpawnerManager Instance {get; private set;}

        protected void Awake()
        {
            if(Instance == null){
                Instance = this;
                RandomInstance = new();
            }
            else if(Instance != null && Instance != this){
                Destroy(this);
            }
        }
        public void SpawnObjectInParent<T>(T gameObj, Transform theParent, Action<T> onBuildObj = null) where T : MonoBehaviour
        {
            T newGameObj;
            newGameObj = Instantiate(gameObj, theParent);
            newGameObj.transform.SetParent(theParent, false);
            onBuildObj?.Invoke(newGameObj);
        }
        public void SpawnComponentInParent<T>(T component, Transform theParent, Action<T> onBuildObj = null) where T : Component
        {
            T newComponent;
            newComponent = Instantiate(component, theParent);
            newComponent.transform.SetParent(theParent, false);
            onBuildObj?.Invoke(newComponent);
        }
        public void SpawnObjectInParent(GameObject gameObj, Transform theParent, Action<GameObject> onBuildObj = null)
        {
            GameObject newGameObj = Instantiate(gameObj, theParent);
            newGameObj.transform.SetParent(theParent, false);
            onBuildObj?.Invoke(newGameObj);
        }
        public Task SpawnObjectInParentAsync<T>(T gameObj, int index, Transform theParent, Func<T, int, Task> onBuildObj = null) where T : MonoBehaviour{
            T newGameObj;
            newGameObj = Instantiate(gameObj, theParent);
            return onBuildObj?.Invoke(newGameObj, index);
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

        public Vector3 SpawnObjectsInGroup(GameObject prefab, int count, Vector3 direction, Transform parent = null, int maxPerRow = 5, Vector3 startPosition = default, float spacing = 0.05f){
            Vector3 currentPosition = startPosition;
            Vector3 size = prefab.TryGetObjectSize();

            float xDir = direction.x * (size.x + spacing);
            float yDir = direction.y * (size.y + spacing);
            float zDir = direction.z * (size.z + spacing);


            for(int i = 0; i < count;){
                for(int j = 0; j < maxPerRow && i < count; ++j){
                    GameObject obj = Instantiate(prefab, parent);
                    obj.transform.localPosition = currentPosition;
                    //Update progress
                    currentPosition = new Vector3(currentPosition.x + xDir, currentPosition.y, currentPosition.z + zDir);
                    ++i;
                }
                if(i == count){
                    currentPosition = new Vector3(currentPosition.x + spacing * direction.x, currentPosition.y, currentPosition.z + spacing * direction.z);
                    break;
                }

                currentPosition = new Vector3(startPosition.x, currentPosition.y + yDir, startPosition.z);
            }
            return currentPosition;
        }
        public Vector3 SpawnObjectsLimitCol(GameObject prefab, int count, int maxCol, Vector3 direction, Transform parent = null, Vector3 startPosition = default, float spacing = 0.05f){
            if(maxCol <= 0){
                return startPosition;
            }
            return SpawnObjectsInGroup(prefab, count, direction, parent, count / maxCol + 1, startPosition, spacing);
        }

        public TObject SpawnObjectInParent<TObject>(TObject prefab, Transform theParent) where TObject : MonoBehaviour{
            TObject newGameObj;
            newGameObj = Instantiate(prefab);
            newGameObj.transform.SetParent(theParent);
            return newGameObj;
        }
    }
}