using Project.Utils.ExtensionMethods;
using Project.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

namespace Project.MiniGames.FishingGame
{
    public class FishSpawner : MonoBehaviour
    {
        [SerializeField] private float randomRange = 20f;
        [SerializeField] private int maxCount = 20;
        [SerializeField] private int minCount = 5;
        [SerializeField] private Vector3 OriginalSpawn;
        [SerializeField] private Vector3 LockDir;
        [SerializeField] private LayerMask collideLayerMask;
        private float enemyRadius;

        private List<Fish> cache;
        private Vector3 RandomMethod() => VectorExtensionMethods.Randomize(OriginalSpawn, randomRange, LockDir);
        // protected void Awake()
        // {
        //     enemyRadius = AvailableFish.FindLargestPropertyInObjects<FishPack,float>((pack)=>{
        //         if (!pack.origin.TryGetComponent<Collider>(out Collider collider)) return -1;
        //         return collider.bounds.extents.x;
        //     });

        // }

        public void SpawnFish(GameObject[] objs){
            for(int i = 0; i < objs.Length; i++){
                
                if(!objs[i].TryGetComponent<Fish>(out Fish fish)) continue;
                SpawnerManager.Instance.SpawnObjectsRandomly<Fish>(fish, Random.Range(minCount, maxCount), RandomMethod, CanBeSpawn, transform);
            }
        }

        public void Shuffle(){
            if(cache == null){
                cache = new();
                foreach(Transform child in transform){
                    if(!child.TryGetComponent<Fish>(out Fish fish)) continue;
                    cache.Add(fish);
                    fish.ShuffleBoard();
                }
                return;
            }

            foreach(Fish child in cache){
                child.ShuffleBoard();
            }
        }

        // protected void Start(){
        //     for (int i = 0; i < AvailableFish.Length; i++)
        //     {
        //         SpawnerManager.Instance.SpawnObjectsRandomly<Fish>(AvailableFish[i].origin, AvailableFish[i].count, RandomMethod, CanBeSpawn, transform);
        //     }
        // }

        private bool CanBeSpawn(Vector3 spawnPoint){
            return true;
        //     Collider[] collisionResult = Physics.OverlapSphere(spawnPoint, enemyRadius, collideLayerMask);

        //     //If the Collision is empty then, we can instantiate
        //     return collisionResult == null || collisionResult.Length == 0;
        }
    }
}