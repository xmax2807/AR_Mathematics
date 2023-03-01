using System;

namespace Project.RewardSystem{
    public class Reward<T> : ICollectable, IUnlockable<T> where T : IComparable<T>
    {
        public T Goal {get;protected set;}

        private bool isAcquired;
        public bool IsAcquired => isAcquired;

        public Reward(T goal){
            Goal = goal;
        }
        public void AddToCollector()
        {
            throw new System.NotImplementedException();
        }

        public bool CanBeRewarded(T currentValue) {
            if(isAcquired) return true;

            isAcquired = currentValue.CompareTo(Goal) >= 0;
            return isAcquired;
        }
    }
}