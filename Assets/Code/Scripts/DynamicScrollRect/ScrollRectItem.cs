using UnityEngine;
using System;
using Project.Utils.ObjectPooling;
using Project.Utils.ExtensionMethods;

namespace Project.UI.DynamicScrollRect{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ScrollRectItem<T> : BasePoolingObject, IScrollItem, IComparable<ScrollRectItem<T>>
    {
        public Action RefreshListAction {get;set;}
        protected RectTransform rectTransform;
        public RectTransform Rect => rectTransform;
        public virtual float CurrentHeight
        {
            get => rectTransform.sizeDelta.y;
            set => rectTransform.sizeDelta.Set(rectTransform.sizeDelta.x, value);
        }

        public virtual float CurrentWidth
        {
            get => rectTransform.sizeDelta.x;
            set => rectTransform.sizeDelta.Set(value, rectTransform.sizeDelta.y);
        }

        public virtual int CurrentIndex { get; set; }
        public virtual void Reset(){}

        #region Viewport
        public bool IsCentralized { get; private set; }
        public Vector2 PositionInViewport { get; private set; }
        public Vector2 DistanceFromCenter { get; private set; }

        public virtual void SetPositionInViewport(Vector2 position, Vector2 distanceFromCenter)
        {
            PositionInViewport = position;
            DistanceFromCenter = distanceFromCenter;
        }
        public virtual void OnObjectIsCentralized()
        {
            IsCentralized = true;
        }

        public virtual void OnObjectIsNotCentralized()
        {
            IsCentralized = false;
        }
        #endregion

        public virtual void UpdateScrollItem(T item, int index){
            CurrentIndex = index;
            OnObjectIsNotCentralized();
        }

        public int CompareTo(ScrollRectItem<T> other)
        {
            if(other == null) return 1;

            return CurrentIndex.CompareTo(other.CurrentIndex);
        }
        private void OnValidate(){
            this.EnsureComponent<RectTransform>(ref rectTransform);
        }
    }
}