using UnityEngine;
using UnityEngine.UI;
using Gameframe.GUI.PanelSystem;
using System.Threading.Tasks;

namespace Project.UI.Panel{
    public abstract class BasePanelController : MonoBehaviour{
        [SerializeField] public PanelViewBase View;
        [SerializeField] private Button backButton;
        public Button BackButton => GetBackButton();
        public UnityEngine.Events.UnityAction onBackButtonClicked;
        public abstract PanelEnumType Type {get;}
        public abstract bool CheckType(PanelViewData data);
        public abstract void SetUI(PanelViewData Data);
        protected virtual Button GetBackButton() => backButton;
        protected virtual void OnEnable(){
            if(backButton == null || onBackButtonClicked == null) return;
            backButton.onClick?.AddListener(onBackButtonClicked);
        }
        protected virtual void OnDisable(){
            if(backButton == null || onBackButtonClicked == null) return;
            backButton?.onClick?.RemoveListener(onBackButtonClicked);
        }
        public virtual async Task Hide(){
            await View.HideAsync();
        }
        public virtual async Task Show(){
            await View.ShowAsync();
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