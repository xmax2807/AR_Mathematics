using UnityEngine;
using Project.Managers;
using UnityEngine.UI;

namespace Project.UI.Indicator{
    public interface IIndicatorItem{
        void SwitchToUnselectedUI();
        void SwitchToSelectedUI();
    }
    public abstract class BaseIndicator<TItem> : MonoBehaviour where TItem : Component, IIndicatorItem
    {
        [SerializeField] private LayoutGroup prefabContainer;
        [SerializeField] private TItem prefab;
        protected TItem[] items;
        protected int currentIndex;

        public void ManualFetchItem(int count){
            items = new TItem[count];
            for(int i = 0; i < count; i++){
                SpawnerManager.Instance.SpawnComponentInParent(prefab, prefabContainer.transform, (obj)=>OnBuildComponent(obj, i));
            }
            UpdateNextItemUI();
        }

        protected virtual void OnBuildComponent(TItem item, int index){
            items[index] = item;
            item.SwitchToUnselectedUI();
        }

        public bool CanMoveTo(int index) => index != currentIndex && index >= 0 && items != null && index < items.Length;
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