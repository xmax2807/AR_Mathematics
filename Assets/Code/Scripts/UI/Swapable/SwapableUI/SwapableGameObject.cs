using UnityEngine;

namespace Project.UI{
    public class SwapableGameObject : MonoBehaviour
    {
        ISwapableLogic<GameObject> controller;
        [SerializeField] private GameObject firstObject;
        [SerializeField] private GameObject secondObject;

        private GameObject current;

        public System.Func<bool> OnCheckCondition;
        void Awake(){
            controller = new SwapableLogicDefault<GameObject>(firstObject, new SwapableLogic<GameObject>(secondObject), OnCheckCondition);
        }
        void Start(){
            UpdateUI();
        }
        public void UpdateUI(){
            current?.SetActive(false);
            current = controller.GetData();
            current?.SetActive(true);
        }
        public void ManualSetup(GameObject firstObject, GameObject secondObject, System.Func<bool> condition){
            this.firstObject = firstObject;
            this.secondObject = secondObject;
            this.OnCheckCondition = condition;
            controller = new SwapableLogicDefault<GameObject>(firstObject, new SwapableLogic<GameObject>(secondObject), OnCheckCondition);
        }

        public void SwitchCondition(bool result){
            
        }
    }
}