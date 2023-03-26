using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;

namespace Project.Addressable{
    
    [CreateAssetMenu(menuName ="STO/Addressables/GameObjectRequest", fileName ="GameObjectAddressableRequest")]
    public class PrefabAddressableRequest : IndividualAddressableRequest<GameObject>
    {
        [SerializeField] private AssetReferenceT<GameObject>[] requests;

        protected override async Task<GameObject[]> Request()
        {
            return await AddressableManager.Instance.PreLoadAssets<GameObject>(requests);
        }
    }
}