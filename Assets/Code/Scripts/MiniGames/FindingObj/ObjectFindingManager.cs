using System.Collections.Generic;
using Project.Managers;
using UnityEngine;
namespace Project.MiniGames.ObjectFinding{
    public class ObjectFindingManager : MonoBehaviour{
        [SerializeField] private bool IsDebugging;
        [SerializeField] private ObjectCenter objectCenter;
        [SerializeField] private PlaceOnPlaneHouse mainGamePlacer;
        private PlacementObject[] placementObjectPrefabs;
        private bool questionIsReady;

        public void GetModelFromRemote(GameObject[] objs){
            List<PlacementObject> items = new(objs.Length);
            foreach (GameObject obj in objs)
            {
                if (obj.TryGetComponent<PlacementObject>(out PlacementObject result))
                {
                    items.Add(result);
                }
            }

            placementObjectPrefabs = items.ToArray();

            TimeCoroutineManager.Instance.WaitUntil(() => questionIsReady, () =>
            {
                if (IsDebugging)
                {

                    //mainGamePlacer.SetMainPlaneAndStart(objectCenter);
                }
                else
                {
                    mainGamePlacer.SetPlacedPrefab(objectCenter.gameObject);
                }
            });
        }
    } 
}