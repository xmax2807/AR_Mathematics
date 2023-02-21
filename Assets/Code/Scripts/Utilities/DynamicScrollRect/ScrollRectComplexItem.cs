using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.DynamicScrollRect{
    public abstract class ScrollRectComplexItem<T> : ScrollRectItem<T[]>{
        public int maxItemInRow;
        public bool IsHorizontal;
        public override string Name => "List Rect Item";
        public sealed override void UpdateScrollItem(T[] item, int index)
        {
            base.UpdateScrollItem(item, index);
            for(int i = 0; i < item.Length; i++){
                UpdateUnitItem(item[i], i);
            }
        }
        protected virtual void UpdateUnitItem(T item, int index){}
    }
}