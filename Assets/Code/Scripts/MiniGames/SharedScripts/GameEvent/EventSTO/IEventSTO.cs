namespace Project.MiniGames{
    public interface IEvent{
        public void Raise<T>(T result);
        public void RegisterListener(IEventListener listener);
        public void UnregisterListener(IEventListener listener);
    }
}