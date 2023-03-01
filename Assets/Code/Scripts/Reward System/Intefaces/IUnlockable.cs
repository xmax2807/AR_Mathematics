namespace Project.RewardSystem{
    public interface IUnlockable<T>{
        bool CanBeRewarded(T currentValue);
        bool IsAcquired {get;}
    }
}