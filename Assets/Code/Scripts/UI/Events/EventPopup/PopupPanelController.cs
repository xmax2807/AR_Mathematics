using Gameframe.GUI.PanelSystem;
namespace Project.UI.Event.Popup{
    public class PopupPanelController : PanelViewController
    {
        private event System.Action<int> onClickCallback;
        public PopupPanelController(PanelType type, System.Action<int> OnClickCallback) : base(type)
        {
            this.onClickCallback = OnClickCallback;
        }

        protected override void ViewDidAppear()
        {
            base.ViewDidAppear();
        }

        protected override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
        }
    }
}