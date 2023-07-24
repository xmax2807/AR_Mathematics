using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.TutorialGames
{
    public class SpawnCommand : ITutorialCommand
    {
        public enum SpawnAlgorithm
        {
            Random,
            Row,
            Column,
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
            yield return new WaitUntil(() => Managers.AddressableManager.Instance.IsInitialized);

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

        private IEnumerator PickAlgoAndSpawn(GameObject[] objects)
        {
            GameObject[] result = null;
            Vector3 startPos = default;
            switch (m_algorithm)
            {
                case SpawnAlgorithm.Random:
                    result = new GameObject[0];
                    break;
                case SpawnAlgorithm.Row:
                    startPos = MeasureStartingPosition(objects, new Vector3(1, 0, 0));
                    result = Managers.SpawnerManager.Instance.SpawnObjectsInRow(objects, new Vector3(1, 0, 0), parentTransform, startPosition: startPos, spacing: 0.5f);
                    break;
                case SpawnAlgorithm.Column:
                    startPos = MeasureStartingPosition(objects, new Vector3(0, 0, 1));
                    result = Managers.SpawnerManager.Instance.SpawnObjectsInRow(objects, new Vector3(0, 0, 1), parentTransform,startPosition: startPos, spacing: 0.5f);
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

        private Vector3 MeasureStartingPosition(GameObject[] objects, Vector3 direction, float spacing = 0.5f)
        {
            Vector3 totalSize = default;
            for (int i = 0; i < objects.Length; ++i)
            {
                totalSize += objects[i].GetSizeFromRenderer();
            }

            // Create start position based on total size and direction
            Vector3 startPosition = new Vector3(totalSize.x / 2 * direction.x, totalSize.y / 2 * direction.y, totalSize.z / 2 * direction.z);
            // Add spacing
            startPosition += new Vector3(spacing * direction.x, spacing * direction.y, spacing * direction.z);
            // finalizing by multiplying by -1
            startPosition *= -1f;
            Debug.Log("StartPosition: " + startPosition);
            return startPosition;
        }
    }
}