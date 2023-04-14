using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;

namespace Project.UI.UISetPack
{
    [CreateAssetMenu(menuName ="STO/UISpritePack/SinglePack", fileName ="SingleUIPack")]
    public class UISpriteSetPack : BaseUISpriteSetPack
    {
        [SerializeField] protected AssetReferenceSprite[] spriteRefs;
        public Sprite[] AssetPacks {get; private set;}
        public override async Task Init()
        {
            AssetPacks = await AddressableManager.Instance.PreLoadAssets(spriteRefs);
            IsInitialized = true;
        }

        protected override void OnDestroy(){
            AddressableManager.Instance.UnloadAssets(spriteRefs);
            IsInitialized = false;
        }
    }
}