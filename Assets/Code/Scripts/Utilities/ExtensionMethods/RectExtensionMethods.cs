using UnityEngine;
using UnityEngine.UI;

namespace Project.Utils.ExtensionMethods
{
    public static class RectTransformExtensionMethods
    {
        public enum RectPositionType
        {
            Top, MiddleV, Bottom,
            Left, MiddleH, Right
        }
        public static void SetRectPosition(this RectTransform rect, RectPositionType type)
        {
            Vector2 min = Vector2.zero;
            Vector2 max = Vector2.zero;
            switch (type)
            {
                case RectPositionType.Top:
                    min.Set(0, 1f);
                    max.Set(0, 1f);
                    break;
                case RectPositionType.MiddleH:
                    min.Set(0f, 0.5f);
                    max.Set(0f, 0.5f);
                    break;
                case RectPositionType.Bottom:
                    min.Set(0, 0);
                    max.Set(0, 0);
                    break;
                case RectPositionType.Left:
                    min.Set(0, 0);
                    max.Set(0, 0);
                    break;
                case RectPositionType.MiddleV:
                    min.Set(0.5f, 0);
                    max.Set(0.5f, 0);
                    break;
                case RectPositionType.Right:
                    min.Set(1, 0);
                    max.Set(1, 0);
                    break;
            }
            rect.anchorMin = min;
            rect.anchorMax = max;
        }
        public static void StretchWidth(this RectTransform rect, RectPositionType rectPositionType)
        {
            rect.SetRectPosition(rectPositionType);
            rect.anchorMin = new Vector2(0f, rect.anchorMin.y);
            rect.anchorMax = new Vector2(1f, rect.anchorMax.y);
            SetLeft(rect, 0f);
            SetRight(rect, 0f);
        }
        public static void SetLeft(this RectTransform rect, float left)
        {
            rect.offsetMin = new Vector2(left, rect.offsetMin.y);
        }

        public static void SetRight(this RectTransform rect, float right)
        {
            rect.offsetMax = new Vector2(-right, rect.offsetMax.y);
        }

        public static void SetTop(this RectTransform rect, float top)
        {
            rect.offsetMax = new Vector2(rect.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rect, float bottom)
        {
            rect.offsetMin = new Vector2(rect.offsetMin.x, bottom);
        }
        public static void RefreshContentFitter(this RectTransform transform, LayoutGroup layoutGroup = null, ContentSizeFitter fitter = null)
        {
            if (transform == null || !transform.gameObject.activeSelf)
            {
                return;
            }

            if(layoutGroup == null){
                if(!transform.TryGetComponent<LayoutGroup>(out layoutGroup)) return;
            }
            if(fitter == null){
                if(!transform.TryGetComponent(out fitter)) return;
            }
            
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
        }
    }
}