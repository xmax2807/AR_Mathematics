using System;
using System.Collections.Generic;
using UnityEngine;
using Gameframe.GUI.Camera.UI;

namespace Project.MiniGames.HouseBuilding{
    public class MainPlane : MonoBehaviour, IEventListener{
        [SerializeField] private Transform buildingParent;
        [SerializeField] private ParticleSystem firework;
        public Merge Current {get;set;}

        public string UniqueName => "MainPlane";
        private Stack<PlacementObject> placedBlockHistory;
        private BuildingManager manager;
        private void Awake(){
            if(buildingParent == null){
                var newGameObj = new GameObject("BuildingParent");
                buildingParent = newGameObj.transform;
                newGameObj.transform.SetParent(this.transform, false);
            }
            this.transform.position = new Vector3(0,-2.5f,6.5f);
            
            placedBlockHistory = new Stack<PlacementObject>(10);
            HouseBuildingEventManager.Instance.RegisterEvent<PlacementObject>(HouseBuildingEventManager.BlockTouchEventName, this, OnBlockTouch);
            HouseBuildingEventManager.Instance.RegisterEvent(HouseBuildingEventManager.ReturnPrevBuildEventName, this, Undo);
        }

        public void OnBlockPlaced()
        {
            if(firework != null && !firework.isPlaying){
                firework.gameObject.SetActive(true);
                firework.Play();
            }
        }

        private void Undo()
        {
            if(Current?.PrevMerge == null){
                Debug.Log("Can't undo");
                return;
            }

            Merge prev = Current.PrevMerge;
            prev.gameObject.SetActive(true);
            Destroy(Current.gameObject);
            Current = prev;
            PlacementObject currentBlock = placedBlockHistory.Pop();
            currentBlock?.gameObject.SetActive(true);
        }

        public void AddFirstBuilding(Merge first, BuildingManager manager){
            foreach(Transform child in buildingParent){
                Destroy(child.gameObject);
            }

            Current = Instantiate(first, buildingParent);
            Current.transform.localPosition = Vector3.zero;
            this.manager = manager;
        }

        private void OnBlockTouch(PlacementObject block){

            placedBlockHistory.Push(block);
            Instantiate(block, buildingParent.position + new Vector3(0,10,0), Quaternion.identity, buildingParent);
            block.gameObject.SetActive(false);

            Merge nextMerge = manager.GiveNextMerge(block.ID);
            if(nextMerge == null){
                HouseBuildingEventManager.Instance.RaiseEvent<bool>(BaseGameEventManager.EndGameEventName, false);
                return;
            }
            nextMerge = Instantiate(nextMerge, buildingParent);
            nextMerge.gameObject.SetActive(false);
            
            Current.NextMerge = nextMerge;
            nextMerge.PrevMerge = Current;
            Current = nextMerge;
        }

        public void OnEventRaised<T>(EventSTO sender, T result)
        {
            throw new System.NotImplementedException();
        }
    } 
}