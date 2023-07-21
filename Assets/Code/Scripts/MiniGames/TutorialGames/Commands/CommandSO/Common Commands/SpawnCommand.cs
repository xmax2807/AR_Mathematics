using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.MiniGames.TutorialGames
{
    public class SpawnCommand : ITutorialCommand
    {
        public enum SpawnAlgorithm{
            Random,
            Linear,
            Grid,
        }
        private Addressable.GameObjectReferencePack m_pack;
        private GameObject[] m_objects;
        private Transform parentTransform;
        private System.Action<GameObject[]> m_spawnCallback;
        private SpawnAlgorithm m_algorithm;

        private bool isRemoteLoad;

        public SpawnCommand(GameObject[] objects, 
                    SpawnAlgorithm algorithm = SpawnAlgorithm.Random, 
                    Transform parentTransform = null, 
                    System.Action<GameObject[]> spawnCallback = null)
        {
            m_objects = objects;
            this.parentTransform = parentTransform;
            m_algorithm = algorithm;
            m_spawnCallback = spawnCallback;
            isRemoteLoad = false;
        }

        public SpawnCommand(Addressable.GameObjectReferencePack pack, 
                    Transform parentTransform = null, 
                    SpawnAlgorithm algorithm = SpawnAlgorithm.Random,
                    System.Action<GameObject[]> spawnCallback = null)
        {
            this.m_pack = pack;
            this.parentTransform = parentTransform;
            m_spawnCallback = spawnCallback;
            m_algorithm = algorithm;
            isRemoteLoad = true;
        }
        
        public IEnumerator Execute(ICommander commander)
        {
            yield return new WaitUntil(()=>Managers.AddressableManager.Instance.IsInitialized);
            
            Debug.Log("Getting references");


            Task<GameObject[]> task = isRemoteLoad ? Managers.AddressableManager.Instance.PreLoadAssets(m_pack.References) : Task.FromResult<GameObject[]>(this.m_objects);
            yield return new TaskAwaitInstruction(task);
            // while(!task.IsCompleted){
            //     yield return null;
            // }
            Debug.Log("Got references");

            GameObject[] m_objects = task.Result;
            yield return PickAlgoAndSpawn(m_objects);
        }

        private IEnumerator PickAlgoAndSpawn(GameObject[] objects){
            GameObject[] result = null;
            switch(m_algorithm){
                case SpawnAlgorithm.Random:
                    result = new GameObject[0];
                    break;
                case SpawnAlgorithm.Linear:
                    result = Managers.SpawnerManager.Instance.SpawnObjectsInRow(objects, new Vector3(1,0,0) ,parentTransform, spacing: 0.5f);
                    break;
                case SpawnAlgorithm.Grid:
                    result = new GameObject[0];
                    break;
                default:
                    result = new GameObject[0];
                    break;
            }
            yield return new WaitUntil(() => result != null);
            m_spawnCallback?.Invoke(result);
        }
    }
}