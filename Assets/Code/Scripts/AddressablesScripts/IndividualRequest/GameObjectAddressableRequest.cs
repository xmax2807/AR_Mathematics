using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;

namespace Project.Addressable{    
    public class PrefabAddressableRequest : IndividualAddressableRequest<GameObject>
    {
        [SerializeField] private AssetReferenceT<GameObject>[] requests;

        protected override async Task<GameObject[]> Request()
        {
            return await AddressableManager.Instance.PreLoadAssets<GameObject>(requests);
        }
    }
}