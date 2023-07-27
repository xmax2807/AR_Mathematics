using UnityEngine;
using Gameframe.GUI.PanelSystem;
using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;

namespace Project.UI.Event.Popup{
    public class PopupContainer : MonoBehaviour, IPanelStackController, IPanelViewContainer
    {
        private IPanelStackController wrapper;
        public RectTransform ParentTransform => this?.GetComponent<RectTransform>();
        private PanelStackSystem stackSystem;
        private UIEventManager eventManager;

        public Task TransitionAsync()
        {
            return wrapper.TransitionAsync();
        }

        public void SettingUp(PanelStackSystem stackSystem, UIEventManager manager){
            this.stackSystem = stackSystem;
            this.eventManager = manager;
            wrapper = new PanelStackController(this.stackSystem,this, eventManager);
        }
    }
}