using System;
using System.Collections.Generic;
using Project.Managers;
using UnityEngine;

namespace Project.MiniGames.ComparisonGame
{
    public class ComparisonGameManager : MonoBehaviour
    {
        [SerializeField] private PlaceOnPlaneMain mainPlanePlacer;
        [SerializeField] private TaskGiver taskGiver;
        [SerializeField] private CompareGameMainPlane mainPlanePrefab;
        private CompareGameMainPlane mainPlane;
        private ItemPack itemPack;
        private ItemFactory<GameObject[]> factory;
        private ComparisonTask currentTask;
        private bool questionIsReady = false;

        public bool IsDebugging;

        private void Awake()
        {
            if (mainPlanePlacer != null)
            {
                mainPlanePlacer.OnSpawnMainPlane += MainPlaneSpawned;
            }
            taskGiver.OnInitialized += () => questionIsReady = true;
            taskGiver.OnTaskChanged += OnTaskUpdate;
        }

        private void OnTaskUpdate(BaseTask task)
        {
            if(task == null){
                return;
            }
            if (task is not ComparisonTask realTask)
            {
                NextProgress();
                return;
            }
            currentTask = realTask;
            mainPlane?.UpdateUI(realTask);
            mainPlane?.SpawnObject(realTask);
        }

        private void MainPlaneSpawned(GameObject mainPlaneObj)
        {
            Debug.Log("Spawned main plane");
            if (!mainPlaneObj.TryGetComponent<CompareGameMainPlane>(out mainPlane))
            {
                Debug.Log("ComparisonGame: Missing the main plane script");
                return;
            }
            mainPlane.OnNextButtonClicked += NextProgress;
            TimeCoroutineManager.Instance.WaitUntil(() => factory != null, () => {
                mainPlane.AddFactory(factory);
                OnTaskUpdate(currentTask);
            });
        }

        private void NextProgress()
        {
            taskGiver.Tasks.UpdateProgress(1);
        }

        public async void GetModelFromRemote(ScriptableObject[] packs)
        {
            foreach (ScriptableObject pack in packs)
            {
                if (pack is ItemPack realPack)
                {
                    itemPack = realPack;
                }
            }
            if (itemPack == null)
            {
                Debug.Log("ComparisonGame: No remote model is on the remote bundle");
                return;
            }

            GameObject[] tier1s = await itemPack.GetTier1();
            GameObject[] tier2s = await itemPack.GetTier2();
            factory = new(new GameObject[2][] { tier1s, tier2s }, itemPack.FuseCondition);

            if (IsDebugging)
            {
                mainPlanePlacer.SetPlacedPrefabAndStart(mainPlanePrefab.gameObject);
            }
            else
            {
                mainPlanePlacer.SetPlacedPrefab(mainPlanePrefab.gameObject);
            }
        }
    }
}
