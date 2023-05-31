using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Project.MiniGames.ObjectFinding{
    public class ObjectCenter : MonoBehaviour{
        [SerializeField] private ObjectFindingMissionUI missionUI;
        private ObjectFindingManager manager;
        // private TaskGiver giver;
    
        public void Setup(ObjectFindingManager manager, TaskGiver giver){
            this.manager = manager;
            // this.giver = giver;
            missionUI.AddTaskGiver(giver);
            this.gameObject.AddComponent<ARAnchor>();
        }
    }
}