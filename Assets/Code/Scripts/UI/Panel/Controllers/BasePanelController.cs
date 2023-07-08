using UnityEngine;
using UnityEngine.UI;
using Gameframe.GUI.PanelSystem;
using System.Threading.Tasks;

namespace Project.UI.Panel{
    public abstract class BasePanelController : MonoBehaviour{
        [SerializeField] public PanelViewBase View;
        public abstract PanelEnumType Type {get;}
        public event System.Action OnPanelHideViewEvent;
        public event System.Action OnPanelShowViewEvent;
        public abstract bool CheckType(PanelViewData data);
        public abstract void SetUI(PanelViewData Data);
        protected virtual void OnEnable(){}
        protected virtual void OnDisable(){}
        public virtual async Task Hide(){
            if(View != null){
                await View.HideAsync();
            }
            OnPanelHideViewEvent?.Invoke();
        }
        public async void HideImmediately(){
            await Hide();
        }
        public virtual async Task Show(){
            OnPanelShowViewEvent?.Invoke();
            if(View != null){
                await View.ShowAsync();
            }
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