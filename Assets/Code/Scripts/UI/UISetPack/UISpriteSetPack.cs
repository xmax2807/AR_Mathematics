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

        public override async Task<Sprite> FindASprite(string name)
        {
            if(!IsInitialized){
                await Init();
            }
            foreach(Sprite sprite in AssetPacks){
                if(sprite.name == name) return sprite;
            }
            return null;
        }
        void OnDisable() => IsInitialized = false;

        public override async Task Init()
        {
            if(IsInitialized) return;
            AssetPacks = await AddressableManager.Instance.PreLoadAssets(spriteRefs);
            IsInitialized = true;
        }

        protected override void OnDestroy(){
            Debug.Log("Asset pack destroyed");
            UnLoadAssets();
        }
        public void UnLoadAssets(){
            AddressableManager.Instance.UnloadAssets(spriteRefs);
            IsInitialized = false;
        }
    }
}