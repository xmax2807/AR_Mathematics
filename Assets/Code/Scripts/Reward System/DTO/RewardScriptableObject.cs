using UnityEngine;

namespace Project.RewardSystem{
    [CreateAssetMenu(fileName = "RewardData", menuName ="STO/Reward/RewardData")]
    public class RewardScriptableObject : ScriptableObject{
        public AudioClip rewardSound;
        public int Goal;
        public RewardBadgeSTO Badge;
        public Sprite UnacquiredBadge;
        public RemoteRewardSTO RemoteModel;
    }
}