using UnityEngine;
namespace Project.UI{
    public abstract class AbstractSwapableBehaviour<TComponent> : MonoBehaviour where TComponent : Component{
        [SerializeField] protected TComponent falseComponent;
        [SerializeField] protected TComponent trueComponent;
        protected ISwapableLogic<TComponent> controller;
        public System.Func<bool> OnCheckCondition;
        void Awake(){
            controller ??= new SwapableLogicDefault<TComponent>(falseComponent, new SwapableLogic<TComponent>(trueComponent), OnCheckCondition);
        }
        void Start(){
            UpdateUI();
        }
        public abstract void UpdateUI();
        public void ManualSetup(TComponent falseComponent, TComponent trueComponent, System.Func<bool> condition){
            this.falseComponent = falseComponent;
            this.trueComponent = trueComponent;
            this.OnCheckCondition = condition;
            controller = new SwapableLogicDefault<TComponent>(falseComponent, new SwapableLogic<TComponent>(trueComponent), OnCheckCondition);
        }
        public void ChangeController(ISwapableLogic<TComponent> newController) => controller = newController;
    }
}