using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using RobinBird.FirebaseTools.Storage.Addressables;
using System.Linq;
using UnityEngine.ResourceManagement.ResourceProviders;
using System;

namespace Project.Addressable
{
    public class AddressableRequestHandler : MonoBehaviour
    {

        [Header("Prefab"), SerializeField] private GameObjectReferencePack prefabRefs;
        [Header("Textures"), SerializeField] private AssetReferencePack<Texture2D> textureRefs;
        [Header("Materials"), SerializeField] private AssetReferenceSingle<Material>[] EnviromentMaterial;
        public void Start()
        {
            // if(AddressableManager.Instance.InitializeTask.IsDone){
            //     PreLoad();
            //     return;
            // }
            // AddressableManager.Instance.OnComplete += (AsyncOperationHandle<IResourceLocator> locator)=>{
            //     PreLoad();
            // };
            //TimeCoroutineManager.Instance.WaitUntil(() => FirebaseAddressablesManager.IsFirebaseSetupFinished, LoadDependencies);
            TimeCoroutineManager.Instance.WaitUntil(() => FirebaseAddressablesManager.IsFirebaseSetupFinished, PreLoad);
        }
        private async void PreLoad()
        {
            var prefabTask = await AddressableManager.Instance.PreLoadAssets(prefabRefs.References);

            prefabRefs.OnComplete?.Invoke(prefabTask);
            foreach (var pack in EnviromentMaterial)
            {
                var materialTask = await AddressableManager.Instance.PreLoadAsset(pack.Reference);
                pack.OnComplete?.Invoke(materialTask);
            }
        }
    }
}