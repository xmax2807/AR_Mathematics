using System;

namespace Project.RewardSystem{
    public class Reward<T> : ICollectable, IUnlockable<T> where T : IComparable<T>
    {
        public T Goal {get;protected set;}
        public T CurrentValue {get;protected set;}

        public bool IsAcquired => CanBeRewarded();

        private bool isAcquired;

        public Reward(T goal){
            Goal = goal;
        }
        public void AddToCollector()
        {
            throw new System.NotImplementedException();
        }
        public bool CanBeRewarded(){
            if(isAcquired) return true;
            
            isAcquired = CurrentValue.CompareTo(Goal) >= 0;
            return isAcquired;
        }
        public bool CanBeRewarded(T currentValue) {
            if(isAcquired) return true;

            CurrentValue = currentValue;
            return CanBeRewarded();
        }
        public void UpdateProgress(T value){
            CurrentValue = value;
        }
    }
}