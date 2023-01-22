namespace Project.Utils.ObjectPooling
{
    public interface IPooling
    {
        string Name { get; }
        bool IsUsing { get; set; }
        void OnReturn();
        void OnRelease();
        event System.Action<IPooling> AddBackToQueue;
    }
}
