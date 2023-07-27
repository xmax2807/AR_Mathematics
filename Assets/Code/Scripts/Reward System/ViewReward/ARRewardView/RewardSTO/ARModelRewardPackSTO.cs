using System.Collections.Generic;
using UnityEngine;
namespace Project.RewardSystem.ViewReward{
    [CreateAssetMenu(fileName = "ARModelRewardPackSTO", menuName = "STO/Reward/ARModelRewardPackSTO")]
    public class ARModelRewardPackSTO : ScriptableObject {
        [SerializeField] private ARRewardSTO[] ListOfRewards;
        [SerializeField] private Sprite lockedImage;
        public Sprite LockedImage => lockedImage;

        public int Count => ListOfRewards.Length;
        public ARRewardSTO GetAt(int index) {
            if(index < 0 || index >= ListOfRewards.Length){
                return null;
            }
            return ListOfRewards[index];
        }

        private Dictionary<string, ARRewardSTO> cacheUniqueNames;

        private void OnEnable(){
            Setup();
        }
        private void OnDisable(){
            ManualUnloadAsset();
        }

        public void ManualUnloadAsset(){
            for(int i = 0; i < ListOfRewards.Length; ++i){
                ListOfRewards[i].UnloadModel();
            }
        }

        private void Setup(){
            cacheUniqueNames = new Dictionary<string, ARRewardSTO>(ListOfRewards.Length);
            for(int i = 0; i < ListOfRewards.Length; ++i){
                string uniqueName = ListOfRewards[i].UniqueName;
                cacheUniqueNames[uniqueName] = ListOfRewards[i];
            }
        }

        public ARRewardSTO GetReward(string uniqueName){
            if(cacheUniqueNames == null){
                Setup();
            }
            return cacheUniqueNames[uniqueName];
        }

        // #if UNITY_EDITOR
        // void OnValidate(){
        //     cacheUniqueNames = new Dictionary<string, ARRewardSTO>(ListOfRewards.Length);
        //     for(int i = 0; i < ListOfRewards.Length; ++i){
        //         string uniqueName = ListOfRewards[i].UniqueName;
        //         cacheUniqueNames[uniqueName] = ListOfRewards[i];
        //     }
        // }
        // #endif
    }
}