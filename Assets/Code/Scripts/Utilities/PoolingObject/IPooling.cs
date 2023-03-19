namespace Project.Utils.ObjectPooling
{
    public interface IPooling
    {
        bool IsUsing { get; set; }
        void OnReturn();
        void OnRelease();
        event System.Action<IPooling> AddBackToQueue;
        
    }
}
