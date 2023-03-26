using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;

namespace Project.Addressable{
    [CreateAssetMenu(menuName ="STO/Addressables/StoRequest", fileName ="StoAddressableRequest")]
    public class StoAddressableRequest : IndividualAddressableRequest<ScriptableObject>
    {
        [SerializeField] private AssetReferenceT<ScriptableObject>[] requests;

        protected override async Task<ScriptableObject[]> Request()
        {
            return await AddressableManager.Instance.PreLoadAssets<ScriptableObject>(requests);
        }
    }
}