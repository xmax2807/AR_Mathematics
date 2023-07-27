using System.Collections.Generic;
using System.Linq;

namespace Project.MiniGames{
    public class ItemFactory<T>{

        private T[] listItemTiers;
        private int[] fuseCondition;
        public ItemFactory(T[] listItemTiers, int[] fuseCondition){
            if(listItemTiers == null || listItemTiers.Length == 0){
                throw new System.Exception("Can't create ItemFactory if there is no Item");
            }
            this.listItemTiers = listItemTiers;
            this.fuseCondition = fuseCondition;
            EnsureFuseCondition();
        }

        private void EnsureFuseCondition(){
            fuseCondition ??= new int[1]{10};
            
            int[] reModified;
            if(fuseCondition.Length >= listItemTiers.Length){
                reModified = new int[listItemTiers.Length - 1];
                for(int i = 0; i < reModified.Length; ++i){
                    reModified[i] = fuseCondition[i];
                }
                fuseCondition = reModified;
            }
            else if(fuseCondition.Length < listItemTiers.Length - 1){
                reModified = new int[listItemTiers.Length - 1];
                int i = 0;
                for(; i < fuseCondition.Length; ++i){
                    reModified[i] = fuseCondition[i];
                }
                while(i < reModified.Length){
                    reModified[i] = reModified[i - 1];
                    ++i;
                }
                fuseCondition = reModified;
            }
        }

        public int[] AutoFuse(int count){
            Dictionary<int, int> result = new(listItemTiers.Length)
            {
                [0] = count
            };

            for (int i = 1; i < listItemTiers.Length; ++i){
                result[i] = 0;
            }

            for(int i = 1; i <= fuseCondition.Length; i++){
                int numTarget = result[i - 1] / fuseCondition[i - 1];
                result[i - 1] = result[i - 1] % fuseCondition[i - 1];
                result[i] += numTarget;
            }

            return result.Values.ToArray();
        }

        public T GetTier(int i){
            if(i < 0 || i >= listItemTiers.Length){
                return default;
            }

            return listItemTiers[i];
        }
    }
}