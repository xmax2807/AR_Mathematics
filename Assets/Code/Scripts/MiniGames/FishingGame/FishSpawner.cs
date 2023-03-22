using Project.Utils.ExtensionMethods;
using Project.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.MiniGames.FishingGame
{
    public class FishSpawner : MonoBehaviour
    {
        [System.Serializable]
        private struct FishPack{
            public Fish origin;
            public int count;
        }
        [SerializeField] private float randomRange = 20f;
        // [SerializeField] private FishPack[] AvailableFish;
        [SerializeField] private int[] spawnCounts;
        [SerializeField] private Vector3 OriginalSpawn;
        [SerializeField] private Vector3 LockDir;
        [SerializeField] private LayerMask collideLayerMask;
        private float enemyRadius;
        private Vector3 RandomMethod() => VectorExtensionMethods.Randomize(OriginalSpawn, randomRange, LockDir);
        // protected void Awake()
        // {
        //     enemyRadius = AvailableFish.FindLargestPropertyInObjects<FishPack,float>((pack)=>{
        //         if (!pack.origin.TryGetComponent<Collider>(out Collider collider)) return -1;
        //         return collider.bounds.extents.x;
        //     });

        // }

        public void SpawnFish(GameObject[] objs){
            for(int i = 0; i < spawnCounts.Length; i++){
                
                if(!objs[i].TryGetComponent<Fish>(out Fish fish)) continue;
                SpawnerManager.Instance.SpawnObjectsRandomly<Fish>(fish, spawnCounts[i], RandomMethod, CanBeSpawn, transform);
            }
        }

        // protected void Start(){
        //     for (int i = 0; i < AvailableFish.Length; i++)
        //     {
        //         SpawnerManager.Instance.SpawnObjectsRandomly<Fish>(AvailableFish[i].origin, AvailableFish[i].count, RandomMethod, CanBeSpawn, transform);
        //     }
        // }

        private bool CanBeSpawn(Vector3 spawnPoint)
        {
            Collider[] collisionResult = Physics.OverlapSphere(spawnPoint, enemyRadius, collideLayerMask);

            //If the Collision is empty then, we can instantiate
            return collisionResult == null || collisionResult.Length == 0;
        }
    }
}