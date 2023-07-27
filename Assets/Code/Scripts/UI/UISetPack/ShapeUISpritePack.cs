using UnityEngine;
using System.Linq;
using Project.QuizSystem;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project.UI.UISetPack{
    [CreateAssetMenu(menuName ="STO/UISpritePack/CompositeSpritePack", fileName ="ComplexPack")]
    public sealed class ShapeUISpritePack : CompositeUISpriteSetPack{
        [System.Serializable]
        public struct ShapePack{
            public Shape.ShapeType type;
            public UISpriteSetPack pack;
        }
        [SerializeField] private ShapePack[] shapePacks;

        protected override string packName => "ShapePacks";

        public override Task Init()
        {
            packs = new UISpriteSetPack[shapePacks.Length];
            for(int i = 0; i < shapePacks.Length; i++){
                packs[i] = shapePacks[i].pack;
            }
            return base.Init();
        }
        public Sprite PickASpriteRandomly(Shape.ShapeType type){
            UISpriteSetPack pack = shapePacks.First((pack)=>pack.type == type).pack;

            if(!pack.IsInitialized){
                Task.Run(pack.Init).GetAwaiter().GetResult();
            }
            int randomIndex = UnityEngine.Random.Range(0, pack.AssetPacks.Length);

            return pack.AssetPacks[randomIndex];
        }
    }
}