namespace Project.MiniGames{
    public interface IEventListener{
        public void OnEventRaised();
        public EventSTO GetEventSTO();
    }

    public interface IEventListenerT<T> : IEventListener{
        public void OnEventRaised(T result);
    }
}