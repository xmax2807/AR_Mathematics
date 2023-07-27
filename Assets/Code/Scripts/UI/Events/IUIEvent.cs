namespace Project.UI.Event{
    public interface IUIEvent{
        void AddListener(IUIEventListener listener);
        void RemoveListener(IUIEventListener listener);
    }
}