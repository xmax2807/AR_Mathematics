namespace Project.UI.DynamicScrollRect
{
    public interface IScrollItem
    {
        void Reset();
        public int CurrentIndex { get; set; }
        UnityEngine.RectTransform Rect{get;}
    }
}