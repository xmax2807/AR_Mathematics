using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using RobinBird.FirebaseTools.Storage.Addressables;
using Gameframe.GUI.PanelSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace Project.Addressable
{
    public class AddressableRequestHandler : MonoBehaviour
    {
        [SerializeField] private PanelStackSystem stackSystem;
        [SerializeField] private PanelType type;
        [Header("Prefab"), SerializeField] private GameObjectReferencePack prefabRefs;
        [Header ("Initialized Prefabs"), SerializeField] private AssetReferenceT<GameObject>[] preInitPrefabs;
        [Header("SFXPack"), SerializeField] private AssetReferenceSingle<Audio.AudioPackSTO> audioRefs;
        
        private List<AsyncOperationHandle<GameObject>> cacheOperations;

        public void Start()
        {
            stackSystem.PushAsync(new PanelViewController(type));
            TimeCoroutineManager.Instance.WaitUntil(() => FirebaseAddressablesManager.IsFirebaseSetupFinished, PreLoad);
        }
        private async void PreLoad()
        {
            cacheOperations = new();
            var operations = AddressableManager.Instance.InstantiatePrefabs(preInitPrefabs);
            cacheOperations.AddRange(operations);

            var prefabs = await AddressableManager.Instance.PreLoadAssets(prefabRefs.References);
            prefabRefs.OnComplete?.Invoke(prefabs);

            var audioPack = await AddressableManager.Instance.PreLoadAsset(audioRefs.Reference);
            AudioManager.Instance.SwapSoundPack(audioPack);
            await stackSystem.PopAsync();

            GameManager.Instance.OnGameFinishLoading?.Invoke();
        }

        private void OnDestroy(){
            AddressableManager.Instance.UnloadAssets(prefabRefs.References);
            AddressableManager.Instance.UnloadAsset(audioRefs.Reference);
            AddressableManager.Instance.UnloadInstancePrefabs(cacheOperations);
        }
    }
}