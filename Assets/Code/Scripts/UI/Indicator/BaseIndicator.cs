using UnityEngine;
using Project.Managers;
using UnityEngine.UI;

namespace Project.UI.Indicator{

    public interface IIndicator{
        public event System.Action<int> OnIndexChanged;
        public void ManualFetchItem(int count, bool autoMoveTo = true);
    }
    
    public abstract class BaseIndicator<TItem> : MonoBehaviour, IIndicator where TItem : Component, IIndicatorItem
    {
        [SerializeField] private LayoutGroup prefabContainer;
        [SerializeField] private TItem prefab;
        protected TItem[] items;
        protected int currentIndex;

        private INavigationCondition navigationCondition;

        public event System.Action<int> OnIndexChanged;

        protected void InvokeIndexChanged(int index) => OnIndexChanged?.Invoke(index);
        protected virtual INavigationCondition InitNavigation(int count){
            return new BasicNavigationCondition(count);
        }
        public void ManualFetchItem(int count, bool autoMoveTo = true){
            navigationCondition = InitNavigation(count);
            items = new TItem[count];
            for(int i = 0; i < count; i++){
                SpawnerManager.Instance.SpawnComponentInParent(prefab, prefabContainer.transform, (obj)=>OnBuildComponent(obj, i));
            }
            if(autoMoveTo){
                UpdateNextItemUI();
            }
        }

        public void ManualFetchItemCallback(int count, System.Action<TItem, int> buildCallback, bool autoMoveTo = true){
            navigationCondition = InitNavigation(count);
            items = new TItem[count];
            for(int i = 0; i < count; i++){
                int index = i;
                SpawnerManager.Instance.SpawnComponentInParent(prefab, prefabContainer.transform, (obj)=>{
                    OnBuildComponent(obj, index);
                    buildCallback?.Invoke(obj,index);
                });
            }
            if(autoMoveTo){
                UpdateNextItemUI();
            }
        }

        protected virtual void OnBuildComponent(TItem item, int index){
            items[index] = item;
            item.SwitchToUnselectedUI();
        }

        public bool CanMoveTo(int index) => navigationCondition != null && navigationCondition.CanMoveTo(currentIndex, index);
        public void MoveTo(int index){
            if(!CanMoveTo(index)) return;

            UpdatePrevItemUI();
            currentIndex = index;
            UpdateNextItemUI();
        }
        public void MoveNext()=>MoveTo(currentIndex + 1);
        public void MovePrev()=>MoveTo(currentIndex - 1);

        protected virtual void UpdatePrevItemUI() => items[currentIndex]?.SwitchToUnselectedUI();
        
        protected virtual void UpdateNextItemUI() {
            if(currentIndex >= items.Length){
                return;
            }

            items[currentIndex]?.SwitchToSelectedUI();
        }
    }
}