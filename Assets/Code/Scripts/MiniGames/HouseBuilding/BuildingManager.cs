using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Project.Managers;

namespace Project.MiniGames.HouseBuilding
{
    public class BuildingManager : MonoBehaviour, IEventListener
    {
        [SerializeField] private TaskGiver taskGiver;

        [Header("Building Blocks")]
        [SerializeField] private Transform spawnParent;
        [SerializeField] private PlacementObject[] blockPrefabs; // 2 -> 9 => i = 0 -> 7
        [SerializeField] private SelectObjectFromCamera blockRaycaster;
        private PlacementObject[] pickedBlock;

        [Header("Main plane")]
        [SerializeField] private PlaceOnPlaneHouse mainGamePlacer;
        [SerializeField] private MainPlane mainPlane;
        private MainPlane mainPlaneObj;

        [SerializeField] bool IsDebugging = false;
        private Merge[] builtInBuildings;

        private int[] blockIndexes;
        private int currentProgress = 0;
        public string UniqueName => "BuildingManager";

        private void Awake()
        {
            if (taskGiver != null)
            {
                taskGiver.OnInitialized += OnTaskGiverReady;

            }
            mainGamePlacer.OnSpawnMainPlane += OnSpawnMainPlane;
            mainGamePlacer.IsBlockRaycast = true;
        }

        private void OnSpawnMainPlane(GameObject obj)
        {
            if (!obj.TryGetComponent<MainPlane>(out mainPlaneObj))
            {
                mainPlaneObj = obj.AddComponent<MainPlane>();
            }
            Debug.Log("Spawn main plane: " + mainPlaneObj.transform.position);
            spawnParent.transform.position = mainPlaneObj.transform.position;
            
            OnGameReset();
            //mainPlaneObj.AddFirstBuilding(builtInBuildings[blockIndexes[0] - 1], this);
        }

        private bool questionIsReady;
        private void OnTaskGiverReady()
        {
            if (taskGiver.CurrentTask is not HouseBuildingTask houseBuildingTask)
            {
                Debug.Log("Game does not have any task to play");
                return;
            }// 2 4 8 9
            blockIndexes = houseBuildingTask.GetAnswer();
            PickBlocks();
            questionIsReady = true;

            taskGiver.OnTaskChanged += OnTaskChanged;
        }
        private void OnEnable()
        {
            HouseBuildingEventManager.Instance.RegisterEvent(HouseBuildingEventManager.ResetBuildEventName, this, OnGameReset);
            HouseBuildingEventManager.Instance.RegisterEvent(HouseBuildingEventManager.StartGameEventName, this, StartGame);
            HouseBuildingEventManager.Instance.RegisterEvent<int>(HouseBuildingEventManager.BlockPlacedEventName, this, OnBlockPlaced);
            HouseBuildingEventManager.Instance.RegisterEvent(HouseBuildingEventManager.ReturnPrevBuildEventName, this, OnUnBuild);
        }

        private void StartGame()
        {
            if(!questionIsReady){
                TimeCoroutineManager.Instance.WaitUntil(()=>questionIsReady, StartGame);
                return;
            } 
            mainGamePlacer.IsBlockRaycast = false;
        }

        public void ReceiveBuildingsFromRemote(GameObject[] objs)
        {
            List<Merge> buildings = new(objs.Length);
            foreach (GameObject obj in objs)
            {
                if (obj.TryGetComponent<Merge>(out Merge result))
                {
                    buildings.Add(result);
                }
            }

            builtInBuildings = buildings.OrderBy((x) => x.ID).ToArray();

            TimeCoroutineManager.Instance.WaitUntil(() => questionIsReady, () =>
            {
                if (IsDebugging)
                {

                    mainGamePlacer.SetMainPlaneAndStart(mainPlane);
                }
                else
                {
                    mainGamePlacer.SetMainPLane(mainPlane);
                }
            });

        }

        public void OnEventRaised<T>(EventSTO sender, T result) { }

        private void OnGameReset()
        {
            currentProgress = 0;
            PickBlocks();
            mainPlaneObj.AddFirstBuilding(builtInBuildings[blockIndexes[0] - 1], this);
            blockRaycaster.SetPlacements(pickedBlock, spawnParent);
        }

        public Merge GiveNextMerge(int id)
        {
            int index = id - 1;
            if (index < 0 || index >= builtInBuildings.Length) return null;
            // ++currentProgress;
            // Debug.Log(blockIndexes[^1]);
            // if(id == blockIndexes[^1]){
            //     if(currentProgress == blockIndexes.Length - 1){
            //     // Complete level
            //         Debug.Log("Complete");
            //         TimeCoroutineManager.Instance.WaitForSeconds(2, ()=> taskGiver.Tasks.UpdateProgress(1));
            //     }
            //     else{
            //     //Fail Level
            //         Debug.Log("Failed");
            //     }
            // }
            return builtInBuildings[index];
        }

        private void OnUnBuild()
        {
            --currentProgress;
        }

        private void OnBlockPlaced(int id)
        {
            blockRaycaster.UnblockRaycast();
            ++currentProgress;
            Debug.Log(blockIndexes[^1]);
            if (id == blockIndexes[^1])
            {
                if (currentProgress >= blockIndexes.Length - 1)
                {
                    // Complete level
                    Debug.Log("Complete");
                    TimeCoroutineManager.Instance.WaitForSeconds(0.5f, mainPlaneObj.OnBlockPlaced);
                    TimeCoroutineManager.Instance.WaitForSeconds(1f, ()=>{
                        taskGiver.Tasks.UpdateProgress(1);
                    });
                }
                else
                {
                    //Fail Level
                    HouseBuildingEventManager.Instance.RaiseEvent(BaseGameEventManager.EndGameEventName, false);
                    Debug.Log("Failed");
                }
            }
        }

        private void OnTaskChanged(BaseTask task)
        {
            if (task is not HouseBuildingTask houseBuildingTask)
            {
                taskGiver.Tasks.UpdateProgress(1);
                return;
            }

            //Destroy existing blocks 
            foreach (Transform child in spawnParent)
            {
                Destroy(child.gameObject);
            }

            blockIndexes = houseBuildingTask.GetAnswer();
            OnGameReset();
        }

        private void PickBlocks()
        {
            int len = Math.Min(blockPrefabs.Length, blockIndexes.Length);
            pickedBlock = new PlacementObject[len - 1];

            for (int i = 1; i < len; ++i)
            {
                pickedBlock[i - 1] = blockPrefabs[blockIndexes[i] - 1];
            }
        }

    }
}