using UnityEngine;
namespace Project.RewardSystem{
    [CreateAssetMenu(menuName ="STO/Reward/Badge", fileName = "Badge")]
    public class RewardBadgeSTO : ScriptableObject{
        public string Title;
        public Sprite Visual;

        public Sprite GetReward(){
            return Visual;
        }
    }
}