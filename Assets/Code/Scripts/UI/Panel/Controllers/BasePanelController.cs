using UnityEngine;
using UnityEngine.UI;
using Gameframe.GUI.PanelSystem;

namespace Project.UI.Panel{
    public abstract class BasePanelController : MonoBehaviour{
        [SerializeField] public PanelViewBase View;
        [SerializeField] public Button BackButton;
        public UnityEngine.Events.UnityAction onBackButtonClicked;
        public abstract PanelEnumType Type {get;}
        public abstract void SetUI(PanelViewData Data);
        private void OnEnable(){
            if(BackButton == null || onBackButtonClicked == null) return;
            BackButton.onClick?.AddListener(onBackButtonClicked);
        }
        private void OnDisable(){
            if(BackButton == null || onBackButtonClicked == null) return;
            BackButton?.onClick?.RemoveListener(onBackButtonClicked);
        }
        public void Hide(){
            View.HideAsync();
        }
        public void Show(){
            View.ShowAsync();
        }
    }
    public abstract class BasePanelController<T> : BasePanelController where T : PanelViewData {
        public abstract void SetUI(T Data);

        public override void SetUI(PanelViewData Data)
        {
            if(Data is not T) return;

            SetUI((T)Data);
        }
        
    }
}