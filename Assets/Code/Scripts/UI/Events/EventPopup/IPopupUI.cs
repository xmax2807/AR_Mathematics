using Gameframe.GUI.PanelSystem;
using Project.UI.Panel;

namespace Project.UI.Event.Popup{
    public interface IPopupUI {
        PanelViewController GetPanelViewController();
        event System.Action<bool> OnClose;
    }
}