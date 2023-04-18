using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;

namespace Project.Addressable{
    public class StoAddressableRequest : IndividualAddressableRequest<ScriptableObject>
    {
        [SerializeField] private AssetReferenceT<ScriptableObject>[] requests;

        protected override async Task<ScriptableObject[]> Request()
        {
            return await AddressableManager.Instance.PreLoadAssets<ScriptableObject>(requests);
        }
    }
}