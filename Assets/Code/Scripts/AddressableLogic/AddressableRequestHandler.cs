using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;

namespace Project.Addressable{
    public class AddressableRequestHandler : MonoBehaviour{

    [Header("Prefab"),SerializeField] private AssetReferencePack prefabRefs;
    [Header("Textures"),SerializeField] private AssetReferenceTexture[] textureRefs;
    public async void Start(){
        var task = AddressableManager.Instance.PreLoadAssets(prefabRefs.references);
        await Task.WhenAll(task);

        
        Debug.Log(task.Result.Length);
        var objs = task.Result;
        foreach(var obj in objs){
            Debug.Log(obj.name);
        }
        prefabRefs.OnComplete?.Invoke(task.Result);
    }
}
}