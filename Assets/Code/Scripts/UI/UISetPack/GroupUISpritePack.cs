using System.Threading.Tasks;
using UnityEngine;

namespace Project.UI.UISetPack{
    [CreateAssetMenu(menuName ="STO/UISpritePack/GroupSpritePack", fileName ="GroupSpritePack")]
    public class GroupUISpritePack : CompositeUISpriteSetPack{
        [SerializeField] private BaseUISpriteSetPack[] listOfPacks;

        protected override string packName => "GroupPacks";

        public override Task Init()
        {
            packs = new BaseUISpriteSetPack[listOfPacks.Length];
            for(int i = 0; i < listOfPacks.Length; i++){
                packs[i] = listOfPacks[i];
            }
            return base.Init();
        }
    }
}