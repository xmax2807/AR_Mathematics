using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel{
    public abstract class BasePanelController : MonoBehaviour{
        [SerializeField] public Button BackButton;
        public abstract PanelEnumType Type {get;}
        public abstract void SetUI(PanelViewData Data);

        public void Hide(){
            gameObject.SetActive(false);
        }
        public void Show(){
            gameObject.SetActive(true);
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