using UnityEngine;
using Gameframe.GUI.PanelSystem;
namespace Project.UI.Event.Popup{
    public class PopupContainer : MonoBehaviour, IPanelViewContainer
    {
        public RectTransform ParentTransform => (RectTransform)this.transform;
    }
}