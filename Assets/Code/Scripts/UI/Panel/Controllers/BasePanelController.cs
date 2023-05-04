using UnityEngine;
using UnityEngine.UI;
using Gameframe.GUI.PanelSystem;
using System.Threading.Tasks;

namespace Project.UI.Panel{
    public abstract class BasePanelController : MonoBehaviour{
        [SerializeField] public PanelViewBase View;
        [SerializeField] public Button BackButton;
        public UnityEngine.Events.UnityAction onBackButtonClicked;
        public abstract PanelEnumType Type {get;}
        public abstract bool CheckType(PanelViewData data);
        public abstract void SetUI(PanelViewData Data);
        protected virtual void OnEnable(){
            if(BackButton == null || onBackButtonClicked == null) return;
            BackButton.onClick?.AddListener(onBackButtonClicked);
        }
        protected virtual void OnDisable(){
            if(BackButton == null || onBackButtonClicked == null) return;
            BackButton?.onClick?.RemoveListener(onBackButtonClicked);
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