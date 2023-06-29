namespace Project.UI.Event{
    public interface IUIEventListener{
        public string UniqueID {get;}

        public void ShowUI();
        public void HideUI();
    }
    public interface IPagerUIEventListener : IUIEventListener{
        public void MoveTo(int index);
    }
}