using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.RewardSystem{
    [CreateAssetMenu(menuName ="STO/Reward/3D Model", fileName = "Model")]
    public class RemoteRewardSTO : UnityEngine.ScriptableObject{
        [SerializeField] private AssetReference RemoteModel;
        [SerializeField] private string uniqueName;
        public string UniqueName => uniqueName;
        public string Description;
        private GameObject cache;
        private AsyncOperationHandle<GameObject> remoteLoadOperation;
        public void PreLoadAsset(){
            if(cache != null){
                Debug.Log("AssetReference: no need to load asset");
                return;
            }
            remoteLoadOperation = RemoteModel.LoadAssetAsync<GameObject>();
        }
        public void UnloadAsset(){
            RemoteModel.ReleaseAsset();
            if(cache == null){
                return;
            }
            RemoteModel.ReleaseInstance(cache);
        }

        public async Task<GameObject> GetModel(){
            Debug.Log("Getting model");
            if(cache == null){
                if(remoteLoadOperation.Equals(default)){
                    PreLoadAsset();
                }
                try{
                    await remoteLoadOperation.Task;
                }
                catch(System.Exception e){
                    Debug.Log(e.Message);
                    PreLoadAsset();
                    await remoteLoadOperation.Task;
                }
                cache = remoteLoadOperation.Result;
            }
            Debug.Log("Got model " + cache?.name);
            return cache;
        }
    }
}