using System;

namespace Project.RewardSystem{
    public abstract class Reward<T> : ICollectable, IUnlockable where T : IEquatable<T>
    {
        public T CurrentProgress{get; protected set;}
        public T Goal {get;protected set;}

        public Reward(T goal){
            Goal = goal;
            CurrentProgress = default;
        }
        public Reward(T goal, T progress) : this(goal){
            CurrentProgress = progress;
        }
        public void AddToCollector()
        {
            throw new System.NotImplementedException();
        }

        public bool IsRewarded() => CurrentProgress.Equals(Goal);
    }
}