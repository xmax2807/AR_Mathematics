using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.UI{
    public class SwapableImage : MonoBehaviour{
        [SerializeField]Image Presenter;
        [SerializeField] Sprite defaultSprite;
        [SerializeField] Sprite realSprite;
        ISwapableLogic<Sprite> controller;
        public System.Func<bool> OnCheckCondition;
        void Awake(){
            controller = new SwapableLogicDefault<Sprite>(defaultSprite, new SwapableLogic<Sprite>(realSprite), OnCheckCondition);
        }
        void Start(){
            UpdateUI();
        }
        public void UpdateUI()=>Presenter.sprite = controller.GetData();
        public void ManualSetup(Sprite defaultSprite, Sprite realSprite, System.Func<bool> condition){
            this.defaultSprite = defaultSprite;
            this.realSprite = realSprite;
            this.OnCheckCondition = condition;
            controller = new SwapableLogicDefault<Sprite>(defaultSprite, new SwapableLogic<Sprite>(realSprite), OnCheckCondition);
        }
    }
}