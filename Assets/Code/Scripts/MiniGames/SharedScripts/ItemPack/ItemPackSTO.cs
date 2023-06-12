using UnityEngine;
using UnityEngine.AddressableAssets;
namespace Project.MiniGames{
    public abstract class ItemPackSTO : ScriptableObject{
        [SerializeField] private AssetReferenceGameObject[] itemTier1;
        [SerializeField] private AssetReferenceGameObject[] itemTier2;
        [SerializeField] private int[] fuseCondition = new int[1] {10};
    }
}