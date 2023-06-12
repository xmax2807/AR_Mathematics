using UnityEngine;
using Project.Managers;
using Project.Utils.ExtensionMethods;
using System;
using Project.UI.GameObjectUI;

namespace Project.MiniGames.ComparisonGame
{

    [RequireComponent(typeof(RandomObjectNumber))]
    public class CompareGameMainPlane : MonoBehaviour
    {
        #region  GUI
        [SerializeField] private CompareGameUI gameUI;
        #endregion

        #region Spawning
        [Header("ObjectParentTransform")]
        [SerializeField] private Transform parentFirstObjTransform;
        [SerializeField] private Transform parentSecondObjTransform;
        private RandomObjectNumber randomizer;
        #endregion

        public event System.Action OnNextButtonClicked;

        public void AddFactory(ItemFactory<GameObject[]> factory){
            randomizer.SetFactory(factory);
        }

        public void Awake()
        {
            // Setup randomizer and env
            randomizer = GetComponent<RandomObjectNumber>();

            if(parentFirstObjTransform == null){
                parentFirstObjTransform = this.gameObject.EnsureChildWithName(childName: "LeftSide").transform;
            }
            if(parentSecondObjTransform == null){
                parentSecondObjTransform = this.gameObject.EnsureChildWithName(childName: "RightSide").transform;
            }

            SetupGameUI();
        }

        private void SetupGameUI(){
            if(gameUI == null){
                Debug.Log("ComparisonGame: Game UI is missing");
                return;
            }
            
            gameUI.NextQuestionButtonClicked += InvokeButtonClick;
        }

        private void InvokeButtonClick()
        {
            this.OnNextButtonClicked?.Invoke();
        }

        public void UpdateUI(ComparisonTask task)
        {
            gameUI.UpdateUI(task);
            TimeCoroutineManager.Instance.WaitForSeconds(0.5f, InteractionEventsBehaviour.Instance.UnblockRaycast);
        }
        public void SpawnObject(ComparisonTask task){
            int leftNumber = task.LeftNumber;
            int rightNumber = task.RightNumber;

            randomizer.SpawnMultiObjectGroup(leftNumber, parentFirstObjTransform, new Vector3(-1, -1, 0));
            randomizer.SpawnMultiObjectGroup(rightNumber, parentSecondObjTransform, new Vector3(1, -1, 0));
        }
    }
}