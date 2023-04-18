using Project.Managers;
using UnityEngine;
using System.Linq;

namespace Project.RewardSystem
{
    public class RewardCollection : MonoBehaviour
    {
        private Reward<float>[] Rewards;
        [SerializeField] private RewardScriptableObject[] Datas;
        private int acquiredRewardIndex;
        public Reward<float> CurrentReward => Rewards[acquiredRewardIndex];
        private void Awake()
        {
            Datas = Datas.OrderBy((x) => x.Goal).ToArray();

            Rewards = new Reward<float>[Datas.Length];
            for (int i = 0; i < Rewards.Length; i++)
            {
                Rewards[i] = new Reward<float>(Datas[i].Goal);
            }
        }

        public RewardScriptableObject CurrentRewardData => Datas[acquiredRewardIndex];
        public bool CanBeRewarded()=> Rewards[acquiredRewardIndex].CanBeRewarded();
        public void IncreaseProgress(float value){
            float currentValue = Rewards[acquiredRewardIndex].CurrentValue; 
            Rewards[acquiredRewardIndex].UpdateProgress(currentValue + value);
        }
        public void OnProgressValueChanged(float newValue)
        {
            if(acquiredRewardIndex >= Datas.Length){
                return;
            }
            if (!Rewards[acquiredRewardIndex].CanBeRewarded(newValue)) return;
            
            
                acquiredRewardIndex++;
                AudioManager.Instance.PlayEffect(Datas[0].rewardSound);
        }
    }
}