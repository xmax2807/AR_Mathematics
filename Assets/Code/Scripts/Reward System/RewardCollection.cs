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

        private void Awake()
        {
            Datas = Datas.OrderBy((x) => x.Goal).ToArray();

            Rewards = new Reward<float>[Datas.Length];
            for (int i = 0; i < Rewards.Length; i++)
            {
                Rewards[i] = new Reward<float>(Datas[i].Goal);
            }
        }

        public void OnProgressValueChanged(float newValue)
        {
            int i = acquiredRewardIndex;
            if(i >= Datas.Length){

                return;
            }
            if (!Rewards[acquiredRewardIndex].CanBeRewarded(newValue)) return;
            
            
                acquiredRewardIndex++;
                AudioManager.Instance.PlayEffect(Datas[i].rewardSound);
        }
    }
}