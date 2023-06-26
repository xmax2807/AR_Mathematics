using System.Threading.Tasks;
using UnityEngine;
namespace Project.RewardSystem.ViewReward{
    [CreateAssetMenu(menuName ="STO/Reward/AR Reward (remote)", fileName = "AR Model")]
    public class ARRewardSTO : ScriptableObject{
        [SerializeField] private RemoteRewardSTO rewardSTO;
        [SerializeField] private Sprite avatar;
        //[SerializeField] private string uniqueName;

        public string UniqueName => rewardSTO.UniqueName;
        public string Description => rewardSTO.Description;
        public Sprite Avatar => avatar;

        public void InitModel(){
            rewardSTO.PreLoadAsset();
        }
        public void UnloadModel(){
            rewardSTO.UnloadAsset();
        }
        public Task<GameObject> GetModel(){
            return rewardSTO.GetModel();
        }
    }
}