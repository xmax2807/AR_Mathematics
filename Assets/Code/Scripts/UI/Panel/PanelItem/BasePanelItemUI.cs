using UnityEngine;

namespace Project.UI.Panel.PanelItem{
    public abstract class BasePanelItemUI : MonoBehaviour, IMenuItem
    {
        public abstract void SetUI(ButtonData data);
    }
    public abstract class BasePanelItemUI<T> : BasePanelItemUI where T : ButtonData{
        public override void SetUI(ButtonData data)
        {
            if(data is not T castData){
                SetDefaultUI(data);
                return;
            }
            SetUI(castData);
        }
        public abstract void SetUI(T data); 
        public abstract void SetDefaultUI(ButtonData data); 
    }
}