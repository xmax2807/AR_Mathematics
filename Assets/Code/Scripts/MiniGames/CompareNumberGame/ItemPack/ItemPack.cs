using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;

namespace Project.MiniGames.ComparisonGame
{

    [CreateAssetMenu(menuName = "STO/MiniGames/Comparison/ItemPack", fileName = "ItemPack")]
    public class ItemPack : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject[] itemTier1Ref;
        [SerializeField] private AssetReferenceGameObject[] itemTier2Ref;
        [SerializeField] private int[] fuseCondition = new int[1] { 10 };

        private GameObject[] cacheItemTier1;
        private GameObject[] cacheItemTier2;
        public async Task<GameObject[]> GetTier1()
        {
            cacheItemTier1 ??= await LoadAssetAsync(itemTier1Ref);
            return cacheItemTier1;
        }

        public async Task<GameObject[]> GetTier2()
        {
            cacheItemTier2 ??= await LoadAssetAsync(itemTier2Ref);
            return cacheItemTier2;
        }
        public int[] FuseCondition => fuseCondition;

        private async Task<GameObject[]> LoadAssetAsync(AssetReferenceGameObject[] refs)
        {
            return await AddressableManager.Instance.PreLoadAssets(refs);
        }
    }
}