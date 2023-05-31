using System;
using System.Collections.Generic;
using Project.Managers;
using UnityEngine;
namespace Project.MiniGames.ObjectFinding{
    public class ObjectFindingManager : MonoBehaviour, IEventListener
    {
        [SerializeField] private bool IsDebugging;
        [SerializeField] private ObjectCenter objectCenter;
        private ObjectCenter objectCenterObj;
        [SerializeField] private PlaceOnPlaneHouse mainGamePlacer;
        [SerializeField] private SelecObjectFromCameraFO objectRaycaster;
        [SerializeField] private TaskGiver taskGiver;
        private PlacementObject[] placementObjectPrefabs;
        private bool questionIsReady;

        public string UniqueName => "ObjectFindingManager";

         private void Awake()
        {
            if (taskGiver != null)
            {
                taskGiver.OnInitialized += OnTaskGiverReady;

            }
            mainGamePlacer.OnSpawnMainPlane += OnSpawnMainPlane;
        }

        private void OnSpawnMainPlane(GameObject obj)
        {
            if (!obj.TryGetComponent<ObjectCenter>(out objectCenterObj))
            {
                objectCenterObj = obj.AddComponent<ObjectCenter>();
            }
            objectCenterObj.Setup(manager: this, giver: this.taskGiver);
            objectRaycaster.SetPlacements(placementObjectPrefabs, objectCenterObj.transform);
        }
        private void OnEnable()
        {
            ARGameEventManager.Instance.RegisterEvent(BaseGameEventManager.StartGameEventName, this, StartGame);
            ObjectFindingEventManager.Instance.RegisterEvent<int>(ObjectFindingEventManager.ObjectTouchEventName, this, OnPlacementTouch);
        }

        private void OnPlacementTouch(int id)
        {
            if(taskGiver.IsCorrect(id)){
                taskGiver.Tasks.UpdateProgress(1);
            }
        }

        private void StartGame()
        {
            if(!questionIsReady){
                TimeCoroutineManager.Instance.WaitUntil(()=>questionIsReady, StartGame);
                return;
            }
            
        }

        private void OnTaskGiverReady()
        {
            // if (taskGiver.CurrentTask is not HouseBuildingTask houseBuildingTask)
            // {
            //     Debug.Log("Game does not have any task to play");
            //     return;
            // }// 2 4 8 9
            // questionIsReady = true;
        }

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

        public void OnEventRaised<T>(EventSTO sender, T result)
        {
            throw new System.NotImplementedException();
        }
    } 
}