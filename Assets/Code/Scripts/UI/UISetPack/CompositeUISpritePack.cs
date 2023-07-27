using UnityEngine;
using UnityEngine.AddressableAssets;
using Project.Managers;
using System.Threading.Tasks;

namespace Project.UI.UISetPack
{
    public abstract class CompositeUISpriteSetPack : BaseUISpriteSetPack
    {
        protected abstract string packName {get;}
        protected BaseUISpriteSetPack[] packs;
        public override async Task Init()
        {
            if(packs == null) return;
            foreach(BaseUISpriteSetPack pack in packs){
                await pack.Init();
            }
            IsInitialized = true;
        }

        public CompositeUISpriteSetPack FindCompositePack(string name){
            foreach(var pack in packs){
                if(pack is CompositeUISpriteSetPack compositePack){
                    if(name == compositePack.packName){
                        return compositePack;
                    }
                }
            }

            return null;
        }

        public override async Task<Sprite> FindASprite(string name)
        {
            Sprite result = null;
            foreach(var pack in packs){
                result = await pack.FindASprite(name);
                
                if(result != null) break;
            }

            return result;
        }

        protected override void OnDestroy(){
            foreach(var pack in packs){
                Destroy(pack);   
            }
            IsInitialized = false;
        }
    }
}