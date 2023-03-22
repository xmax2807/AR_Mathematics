using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    public static AddressableManager Instance;
    void Awake(){
        if(Instance == null){
            Instance = this;
            Addressables.InitializeAsync();
        }
        else if(Instance != this){
            Destroy(this.gameObject);
        }
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
        return tasks.Select((x)=>x.Result).ToArray();
    }
    public async Task<T[]> PreLoadAssets<T>(AssetReferenceT<T>[] required) where T : UnityEngine.Object{
         List<Task<T>> tasks = new();
        foreach (AssetReferenceT<T> asset in required)
        {
            var task = asset.LoadAssetAsync<T>().Task;
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
        return tasks.Select((x)=>x.Result).ToArray();
    }
}
