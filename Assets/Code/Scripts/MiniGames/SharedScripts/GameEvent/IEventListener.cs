namespace Project.MiniGames{
    public interface IEventListener{
        public void OnEventRaised<T>(EventSTO sender, T result);
        public string UniqueName{get;}
    }
}