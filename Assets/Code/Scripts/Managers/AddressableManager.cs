using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using RobinBird.FirebaseTools.Storage.Addressables;

namespace Project.Managers
{
    public class AddressableManager : MonoBehaviour
    {
        private static readonly string[] ERROR_MESSAGES =
    {
        "JSON parse error: The document root must not follow by other values.",
        "Unable to load asset of type UnityEngine.AddressableAssets.ResourceLocators.ContentCatalogData from location",
        "Unknown error in AsyncOperation : Resource<ContentCatalogData>",
        "Unable to load ContentCatalogData from location",
        "GroupOperation failed because one of its dependencies failed",
    };

        private const string FirebaseStoragePath = "gs://math-64c88.appspot.com/GameAssets/Android";
        public static AddressableManager Instance;
        public AsyncOperationHandle<IResourceLocator> InitializeTask { get; private set; }
        public Action<AsyncOperationHandle<IResourceLocator>> OnComplete;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //Addressables.ResourceManager.InternalIdTransformFunc += Configuration.ReplaceInteralId;
                //InitializeTask.Completed += async (x) => await UpdateCatalogs();
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        void Start(){
            InitializeTask = Addressables.InitializeAsync();
                InitializeTask.Completed += Instance.OnComplete;
        }

        public static void AddDependencies()
        {
            Debug.Log("add dependency");
            Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageAssetBundleProvider());
            Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageJsonAssetProvider());
            Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageHashProvider());

            // This requires Addressables >=1.75 and can be commented out for lower versions
            Addressables.InternalIdTransformFunc += FirebaseAddressablesCache.IdTransformFunc;
        }

        private static async Task UpdateCatalogs()
        {
            var isBrokenRemoteCatalog = false;

            // Addressables 内部でエラーや例外が発生した時に呼び出されます
            void OnLogMessageReceivedThreaded(string condition, string trace, LogType type)
            {
                // 既にエラーを検知している場合は無視します
                if (isBrokenRemoteCatalog) return;

                // 通常のログと警告ログは無視します
                if (type == LogType.Log || type == LogType.Warning) return;

                // リモートカタログのファイルが破損している系のメッセージならエラーとみなします
                isBrokenRemoteCatalog = ERROR_MESSAGES.Any(x => condition.Contains(x));

                // Addressables 内部でエラーや例外が発生した時に呼び出される関数を解除します
                Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;
            }

            // Addressables 内部でエラーや例外が発生した時に呼び出される関数を登録します
            Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;

            // カタログを更新します
            var result = Addressables.UpdateCatalogs();

            // 更新が完了するのを待機します
            await result.Task;

            // Addressables 内部でエラーや例外が発生した時に呼び出される関数を解除します
            Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;

            if (isBrokenRemoteCatalog)
            {
                Debug.LogError("Catalog Corrupted");
                return;
            }

            Debug.Log("Success");
        }
        public async Task<T> PreLoadAsset<T>(AssetReferenceT<T> required) where T : UnityEngine.Object{
            return await required.LoadAssetAsync<T>().Task;
        }
        public async Task<GameObject[]> PreLoadAssets(AssetReference[] required)
        {
            List<Task<GameObject>> tasks = new();
            foreach (AssetReference asset in required)
            {
                var task = asset.LoadAssetAsync<GameObject>().Task;
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            return tasks.Select((x) => x.Result).ToArray();
        }
        public async Task<T[]> PreLoadAssets<T>(AssetReferenceT<T>[] required) where T : UnityEngine.Object
        {
            List<Task<T>> tasks = new();
            foreach (AssetReferenceT<T> asset in required)
            {
                var task = asset.LoadAssetAsync<T>().Task;
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            return tasks.Select((x) => x.Result).ToArray();
        }
    }
}
