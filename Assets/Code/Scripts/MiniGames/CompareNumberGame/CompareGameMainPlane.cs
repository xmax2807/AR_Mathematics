using UnityEngine;
using Project.Managers;
using Project.Utils.ExtensionMethods;
using System;

namespace Project.MiniGames.ComparisonGame
{

    [RequireComponent(typeof(RandomObjectNumber))]
    public class CompareGameMainPlane : MonoBehaviour
    {
        #region  GUI
        [SerializeField] private Canvas numberTextCanvas;
        #endregion

        #region Spawning
        [Header("ObjectParentTransform")]
        [SerializeField] private Transform parentFirstObjTransform;
        [SerializeField] private Transform parentSecondObjTransform;
        private RandomObjectNumber randomizer;
        #endregion

        #region Controllers
        private TaskGiver taskGiver;
        #endregion
        public void Awake()
        {
            if (numberTextCanvas != null)
            {
                numberTextCanvas.worldCamera = GameManager.MainGameCam;
            }

            // Setup randomizer and env
            randomizer = GetComponent<RandomObjectNumber>();

            if(parentFirstObjTransform == null){
                parentFirstObjTransform = this.gameObject.EnsureChildWithName(childName: "LeftSide").transform;
            }
            if(parentSecondObjTransform == null){
                parentSecondObjTransform = this.gameObject.EnsureChildWithName(childName: "RightSide").transform;
            }
        }

        public void SetTaskGiver(TaskGiver giver){
            if(giver == null){
                Debug.Log("Task Giver is null");
                return;
            }

            taskGiver = giver;
            taskGiver.OnTaskChanged += UpdateUI;
        }

        private void UpdateUI(BaseTask task)
        {
            
        }
    }
}