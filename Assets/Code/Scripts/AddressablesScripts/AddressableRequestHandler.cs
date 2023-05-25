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

        [Header("ScriptableObject"), SerializeField] private AssetReferencePack<ScriptableObject> soRefs;
        [Header("Prefab"), SerializeField] private GameObjectReferencePack prefabRefs;
        [Header ("Initialized Prefabs"), SerializeField] private AssetReferenceT<GameObject>[] preInitPrefabs;
        [Header("Parent for PreInit"), SerializeField] private Transform parentPreInitPrefabs;
        [Header("SFXPack"), SerializeField] private AssetReferenceSingle<Audio.AudioPackSTO> audioRefs;
        
        private List<AsyncOperationHandle<GameObject>> cacheOperations;

        public async void Start()
        {
            await stackSystem.PushAsync(new PanelViewController(type));
            TimeCoroutineManager.Instance.WaitUntil(() => FirebaseAddressablesManager.IsFirebaseSetupFinished, PreLoad);
        }
        private async void PreLoad()
        {
            cacheOperations = new();
            var operations = AddressableManager.Instance.InstantiatePrefabs(preInitPrefabs);
            foreach(var operation in operations){
               operation.Completed += (operation) => SpawnerManager.Instance.SpawnObjectInParent(operation.Result, parentPreInitPrefabs);
            }
            cacheOperations.AddRange(operations);
            Debug.Log("preInitPrefabs");

            var prefabs = await AddressableManager.Instance.PreLoadAssets(prefabRefs.References);
            prefabRefs.OnComplete?.Invoke(prefabs);
            Debug.Log("refs");

            var ScriptableObjects = await AddressableManager.Instance.PreLoadAssets(soRefs.References);
            Debug.Log(ScriptableObjects[0].name);
            soRefs.OnComplete?.Invoke(ScriptableObjects);
            Debug.Log("scriptableObject");

            if(audioRefs.Reference.IsValid()) {
                var audioPack = await AddressableManager.Instance.PreLoadAsset(audioRefs.Reference);
                AudioManager.Instance.SwapSoundPack(audioPack);
                Debug.Log("audio");
            }
            
            await stackSystem.PopAsync();

            GameManager.Instance.OnGameFinishLoading?.Invoke();
        }

        private void OnDestroy(){
            Debug.Log("Destroyed");
            AddressableManager.Instance.UnloadAssets(soRefs.References);
            AddressableManager.Instance.UnloadAssets(prefabRefs.References);
            AddressableManager.Instance.UnloadAsset(audioRefs.Reference);
            AddressableManager.Instance.UnloadInstancePrefabs(cacheOperations);
        }
    }
}