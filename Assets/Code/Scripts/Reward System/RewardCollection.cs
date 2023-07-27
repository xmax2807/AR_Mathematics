using Project.Managers;
using UnityEngine;
using System.Linq;

namespace Project.RewardSystem
{
    public class RewardCollection : MonoBehaviour
    {
        private Reward<int>[] Rewards;
        [SerializeField] private RewardScriptableObject[] Datas;
        private int acquiredRewardIndex;
        public Reward<int> CurrentReward => Rewards[acquiredRewardIndex];
        
        public UnityEngine.Events.UnityEvent<RewardBadgeSTO> OnRewardAccquired;
        public UnityEngine.Events.UnityEvent<RemoteRewardSTO> OnRemoteRewardAccquired;

        private void Awake()
        {
            //Datas = Datas.OrderBy((x) => x.Goal).ToArray();

            Rewards = new Reward<int>[Datas.Length];
            for (int i = 0; i < Rewards.Length; i++)
            {
                Rewards[i] = new Reward<int>(Datas[i].Goal);
            }
        }

        public RewardScriptableObject CurrentRewardData => Datas[acquiredRewardIndex];
        public RewardScriptableObject GetRewardDataAt(int index){
            if(index < 0 || index >= Rewards.Length){
                return null;
            }
            return Datas[index];
        }
        public bool CanBeRewarded() => Rewards[acquiredRewardIndex].CanBeRewarded();
        public void IncreaseProgress(int value)
        {
            int currentValue = Rewards[acquiredRewardIndex].CurrentValue + value;
            Rewards[acquiredRewardIndex].UpdateProgress(currentValue);
            //OnProgressValueChanged(currentValue);
        }
        public void OnProgressValueChanged(int newValue)
        {
            if (acquiredRewardIndex >= Datas.Length)
            {
                return;
            }

            CurrentReward.UpdateProgress(newValue);
            Debug.Log($"{CurrentReward.CurrentValue} - {CurrentReward.Goal}");
            if (!CurrentReward.CanBeRewarded(newValue)) return;

            GiveReward(CurrentRewardData);
            GiveRemoteReward(CurrentRewardData);
            NextReward();
        }
        public void OnProgressValueChanged(int newValue, int rewardIndex){
            if (rewardIndex >= Datas.Length)
            {
                return;
            }

            var currentReward = Rewards[rewardIndex];
            currentReward.UpdateProgress(newValue);
            Debug.Log($"{currentReward.CurrentValue} - {currentReward.Goal}");
            if (!currentReward.CanBeRewarded(newValue)) return;

            GiveReward(GetRewardDataAt(rewardIndex));
            GiveRemoteReward(GetRewardDataAt(rewardIndex));
            NextReward();
        }
        private void NextReward(){
            ++acquiredRewardIndex;
            if(acquiredRewardIndex >= Datas.Length){
                Debug.Log("Player completed the game");
            }
        }

        private void GiveReward(RewardScriptableObject rewardData){
            if(rewardData.Badge == null) return;
            OnRewardAccquired?.Invoke(rewardData.Badge);
        }
        private void GiveRemoteReward(RewardScriptableObject rewardData){
            if(rewardData.RemoteModel == null) return;
            UserManager.Instance.AddModelReward(rewardData.RemoteModel.UniqueName);
            //DatabaseManager.Instance.UserController.SaveLocalData();
            OnRemoteRewardAccquired?.Invoke(rewardData.RemoteModel);
        }
    }
}