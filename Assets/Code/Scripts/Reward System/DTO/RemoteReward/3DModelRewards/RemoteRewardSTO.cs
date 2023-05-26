using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.RewardSystem{
    [CreateAssetMenu(menuName ="STO/Reward/3D Model", fileName = "Model")]
    public class RemoteRewardSTO : UnityEngine.ScriptableObject{
        [SerializeField] private AssetReference RemoteModel;
        public string Description;
        private GameObject cache;
        public Task<GameObject> PreLoadAsset(){
            return RemoteModel.LoadAssetAsync<GameObject>().Task;
        }
        public void UnloadAsset(){
            RemoteModel.ReleaseAsset();
        }

        public async Task<GameObject> GetModel(){
            if(cache == null){
                cache = await PreLoadAsset();
            }
            return cache;
        }
    }
}