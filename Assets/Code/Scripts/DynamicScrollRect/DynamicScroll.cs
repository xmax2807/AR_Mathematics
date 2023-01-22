using System;
using System.Collections.Generic;
//using pooling;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Project.Utils.ObjectPooling;

namespace Project.UI.DynamicScrollRect
{
    [Flags]
    public enum ScrollDirection
    {
        NONE = 0x1,
        LEFT = 0x2,
        RIGHT = 0x4,
        UP = 0x8,
        DOWN = 0x10
    }

    public class DynamicScroll<T, T1>
        where T1 : ScrollRectItem<T>
    {
        public const float CONTENT_OFFSET_FIXER_LIMIT = 1000f;
        public float spacing = 15f;
        public ListPooling<T1> listPool;

        public T1 CentralizedObject { get; private set; }
        public DynamicScrollRect ScrollRect { get; private set; }

        public bool centralizeOnStop;
        public Action<Vector2> OnDragEvent;
        public UnityEvent<PointerEventData> OnBeginDragEvent;
        public UnityEvent<PointerEventData> OnEndDragEvent;

        private VerticalLayoutGroup mVerticalLayoutGroup;
        private HorizontalLayoutGroup mHorizontalLayoutGroup;
        private GridLayoutGroup mGridLayoutGroup;
        private ContentSizeFitter mContentSizeFitter;

        private bool mIsVertical = false;
        private bool mIsHorizontal = false;
        private bool mIsDragging = false;

        private ScrollDirection mLastInvalidDirections;
        private ScrollRect.MovementType mMovementType;

        private Vector2 mNewAnchoredPosition = Vector2.zero;
        private Vector2 mScrollVelocity = Vector2.zero;
        private Vector2 mLastPos = Vector2.zero;
        private Vector2 mClampedPosition = Vector2.zero;
        private IList<T> infoList;
        private Tween forceMoveTween;
        public IList<T> RawDataList => infoList;
        public void Initiate(DynamicScrollRect scrollRect, IList<T> infoList, int startIndex, GameObject objReference, bool createMoreIfNeeded = true, int? forceAmount = null)
        {
            ScrollRect = scrollRect;
            if (ScrollRect == null)
                throw new Exception("No scroll rect in gameObject.");

            if (objReference == null)
                throw new Exception("No Reference GameObject has set.");

            if (startIndex >= infoList.Count)
                throw new Exception("Invalid index: " + startIndex);

            this.infoList = infoList;

            ScrollRect.onValueChanged.AddListener(OnScroll);
            ScrollRect.OnBeginDragEvent.AddListener(OnBeginDrag);
            ScrollRect.OnEndDragEvent.AddListener(OnEndDrag);
            ScrollRect.OnStopDragEvent.AddListener(OnStopMoving);

            mMovementType = ScrollRect.movementType;
            ScrollRect.MovementType = ScrollRect.movementType;
            ScrollRect.movementType = UnityEngine.UI.ScrollRect.MovementType.Unrestricted;

            if (ScrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
            {
                mVerticalLayoutGroup = ScrollRect.content.GetComponent<VerticalLayoutGroup>();
                mVerticalLayoutGroup.spacing = spacing;
            }

            if (ScrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
            {
                mHorizontalLayoutGroup = ScrollRect.content.GetComponent<HorizontalLayoutGroup>();
                mHorizontalLayoutGroup.spacing = spacing;
            }

            if (ScrollRect.content.GetComponent<GridLayoutGroup>() != null)
            {
                mGridLayoutGroup = ScrollRect.content.GetComponent<GridLayoutGroup>();
                mGridLayoutGroup.spacing = new Vector2(spacing, spacing);
            }

            if (ScrollRect.content.GetComponent<ContentSizeFitter>() != null)
                mContentSizeFitter = ScrollRect.content.GetComponent<ContentSizeFitter>();

            mIsHorizontal = ScrollRect.horizontal;
            mIsVertical = ScrollRect.vertical;

            listPool = new(forceAmount ?? 0, objReference, ScrollRect.content)
            {
                createMoreIfNeeded = createMoreIfNeeded
            };

            CreateList(startIndex);

            listPool.ForEach(x =>
            {
                x.RefreshListAction = RefreshPosition;
                x.OnObjectIsNotCentralized();
            });

            SetCentralizedObject();
            DisableGridComponents();

            if (!mIsHorizontal || !mIsVertical) return;
            Debug.LogError("DynamicScroll doesn't support scrolling in both directions, please choose one direction (horizontal or vertical)");
            mIsHorizontal = false;
        }

        private void SetCentralizedObject()
        {
            var oldCenteredObject = CentralizedObject;
            CentralizedObject = GetCentralizedObject();
            if (oldCenteredObject == CentralizedObject)
                return;

            oldCenteredObject?.OnObjectIsNotCentralized();
            CentralizedObject.OnObjectIsCentralized();
        }

        //if startIndex = -1, it will keep the same position
        public void ChangeList(IList<T> infoList, int startIndex = -1, bool resetContentPosition = false)
        {
            if (startIndex == -1)
                startIndex = GetHighest().CurrentIndex;

            ScrollRect.StopMovement();
            ScrollRect.content.anchoredPosition = Vector2.zero;

            var objs = listPool.GetAllWithState(true);
            objs.ForEach(x => listPool.Release(x));
            if (resetContentPosition)
                ScrollRect.content.anchoredPosition = new Vector2((mIsHorizontal ? spacing : 0), (mIsVertical ? spacing : 0));

            this.infoList = infoList;

            CreateList(startIndex);
        }

        private void CreateList(int startIndex)
        {
            var totalSize = 0f;
            var lastObjectPosition = Vector2.zero;
            startIndex = Mathf.Max(0, startIndex);
            var currentIndex = startIndex;
            var canDrag = false;

            if (infoList != null && infoList.Count > 0)
            {
                do
                {
                    var obj = listPool.Collect();
                    obj.UpdateScrollItem(this.infoList[currentIndex], currentIndex);
                    var posX = currentIndex > 0 ? lastObjectPosition.x + (mIsHorizontal ? spacing : 0) : 0;
                    var posY = currentIndex > 0 ? lastObjectPosition.y - (mIsVertical ? spacing : 0) : 0;
                    obj.Rect.anchoredPosition = new Vector2(posX, posY);
                    lastObjectPosition = new Vector2(posX + (mIsHorizontal ? obj.CurrentWidth : 0), posY - (mIsVertical ? obj.CurrentHeight : 0));

                    totalSize += ((mIsVertical) ? obj.CurrentHeight : obj.CurrentWidth) + spacing;
                    currentIndex++;
                } while (currentIndex < infoList.Count &&
                         (mIsVertical && totalSize < (ScrollRect.viewport.rect.height * 2f)) ||
                         (mIsHorizontal && totalSize < (ScrollRect.viewport.rect.width * 2f)));

                canDrag = (mIsHorizontal && totalSize > ScrollRect.viewport.rect.width) || (mIsVertical && totalSize > ScrollRect.viewport.rect.height);
            }

            ToggleScroll(canDrag);
        }

        public void RefreshPosition()
        {
            var lastObject = GetHighest();
            var objs = listPool.GetAllWithState(true);
            var index = lastObject.CurrentIndex;
            var totalSize = 0f;

            for (var i = 0; i < objs.Count; i++)
            {
                var currentObject = listPool.Find(x => x.CurrentIndex == index);
                if (currentObject != null && currentObject.IsUsing && currentObject.CompareTo(lastObject) != 0)
                {
                    var no = currentObject.Rect;
                    var lo = lastObject.Rect;
                    var x = mIsHorizontal ? lo.anchoredPosition.x + lastObject.CurrentWidth + spacing : no.anchoredPosition.x;
                    var y = mIsVertical ? lo.anchoredPosition.y - lastObject.CurrentHeight - spacing : no.anchoredPosition.y;
                    no.anchoredPosition = new Vector2(x, y);
                    totalSize += mIsHorizontal ? lastObject.CurrentWidth : lastObject.CurrentHeight;
                    lastObject = currentObject;
                }

                index++;
            }

            if (lastObject != null)
                totalSize += mIsHorizontal ? lastObject.CurrentWidth : lastObject.CurrentHeight;

            var canDrag = (mIsHorizontal && totalSize > ScrollRect.viewport.rect.width) || (mIsVertical && totalSize > ScrollRect.viewport.rect.height);
            ToggleScroll(canDrag);
            SetCentralizedObject();
            LimitScroll();
        }

        private void DisableGridComponents()
        {
            if (mVerticalLayoutGroup != null)
                mVerticalLayoutGroup.enabled = false;

            if (mHorizontalLayoutGroup != null)
                mHorizontalLayoutGroup.enabled = false;

            if (mContentSizeFitter != null)
                mContentSizeFitter.enabled = false;

            if (mGridLayoutGroup != null)
                mGridLayoutGroup.enabled = false;

            ScrollRect.content.anchorMax = Vector2.one;
            ScrollRect.content.anchorMin = Vector2.zero;
            ScrollRect.content.offsetMax = Vector2.zero;
            ScrollRect.content.offsetMin = Vector2.zero;
        }

        private void OnScroll(Vector2 pos)
        {
            mScrollVelocity = ScrollRect.content.anchoredPosition - mLastPos;
            mLastPos = ScrollRect.content.anchoredPosition;

            OnDragEvent?.Invoke(mScrollVelocity);

            mLastInvalidDirections = LimitScroll();

            if ((mLastInvalidDirections & ScrollDirection.NONE) != ScrollDirection.NONE)
            {
                ScrollRect.needElasticReturn = true;
                return;
            }

            if (mIsDragging)
                ScrollRect.needElasticReturn = false;
            //TODO: fix offset
            //ApplyOffsetIfNeeded();

            var lowestObj = GetLowest();
            if(lowestObj == null) return;
            var lowestRect = lowestObj.Rect;
            var highestObj = GetHighest();
            if(highestObj == null) return;
            var highestRect = highestObj.Rect;

            UpdateObjectsCentralizedPosition();
            SetCentralizedObject();

            if (mIsHorizontal)
            {
                if (mScrollVelocity.x > 0)
                {
                    while (highestRect.anchoredPosition.x + ScrollRect.content.anchoredPosition.x
                        > ScrollRect.viewport.rect.width + (highestObj.CurrentWidth * 0.1f))
                    {
                        var nextIndex = lowestObj.CurrentIndex - 1;
                        if (nextIndex < 0) return;
                        listPool.Release(highestObj);
                        var obj = listPool.Collect();
                        obj.UpdateScrollItem(infoList[nextIndex], nextIndex);
                        obj.transform.SetAsFirstSibling();

                        mNewAnchoredPosition = lowestRect.anchoredPosition;
                        mNewAnchoredPosition.x += -lowestObj.CurrentWidth - spacing;

                        obj.Rect.anchoredPosition = mNewAnchoredPosition;
                        ResetObjects();
                    }
                }
                else if (mScrollVelocity.x < 0)
                {
                    while (lowestRect.anchoredPosition.x + ScrollRect.content.anchoredPosition.x + (lowestObj.CurrentWidth / 2f)
                        < (-ScrollRect.viewport.rect.width / 2f) - (lowestObj.CurrentWidth * 0.1f))
                    {
                        var nextIndex = highestObj.CurrentIndex + 1;
                        if (nextIndex >= infoList.Count) return;
                        listPool.Release(lowestObj);
                        var obj = listPool.Collect();
                        obj.UpdateScrollItem(infoList[nextIndex], nextIndex);
                        obj.transform.SetAsFirstSibling();

                        mNewAnchoredPosition = highestRect.anchoredPosition;
                        mNewAnchoredPosition.x += obj.CurrentWidth + spacing;

                        obj.Rect.anchoredPosition = mNewAnchoredPosition;
                        ResetObjects();
                    }
                }
            }
            else if (mIsVertical)
            {
                if (mScrollVelocity.y > 0)
                {
                    while (highestRect.anchoredPosition.y + ScrollRect.content.anchoredPosition.y - (highestObj.CurrentHeight / 2f)
                        > ((ScrollRect.viewport.rect.height / 2f) + highestObj.CurrentHeight * 0.1f))
                    {
                        var nextIndex = lowestObj.CurrentIndex + 1;
                        if (nextIndex >= infoList.Count) return;
                        listPool.Release(highestObj);
                        var obj = listPool.Collect();
                        obj.UpdateScrollItem(infoList[nextIndex], nextIndex);
                        obj.transform.SetAsLastSibling();

                        mNewAnchoredPosition = lowestRect.anchoredPosition;
                        mNewAnchoredPosition.y += -lowestObj.CurrentHeight - spacing;

                        obj.Rect.anchoredPosition = mNewAnchoredPosition;
                        ResetObjects();
                    }
                }
                else if (mScrollVelocity.y < 0)
                {
                    while (lowestRect.anchoredPosition.y + ScrollRect.content.anchoredPosition.y + (highestObj.CurrentHeight / 2f)
                        < -(ScrollRect.viewport.rect.height + lowestObj.CurrentHeight * 0.1f))
                    {
                        var nextIndex = highestObj.CurrentIndex - 1;
                        if (nextIndex < 0) return;
                        listPool.Release(lowestObj);
                        var obj = listPool.Collect();
                        obj.UpdateScrollItem(infoList[nextIndex], nextIndex);
                        obj.transform.SetAsFirstSibling();

                        mNewAnchoredPosition = highestRect.anchoredPosition;
                        mNewAnchoredPosition.y += obj.CurrentHeight + spacing;

                        obj.Rect.anchoredPosition = mNewAnchoredPosition;
                        ResetObjects();
                    }
                }
            }

            void ResetObjects()
            {
                lowestObj = GetLowest();
                lowestRect = lowestObj.Rect;
                highestObj = GetHighest();
                highestRect = highestObj.Rect;
            }
        }

        private void OnBeginDrag(PointerEventData pointData)
        {
            mIsDragging = true;
            forceMoveTween?.Kill();
            OnBeginDragEvent?.Invoke(pointData);
        }

        private void OnEndDrag(PointerEventData pointData)
        {
            mIsDragging = false;
            OnEndDragEvent?.Invoke(pointData);
        }

        private void OnStopMoving(PointerEventData arg0)
        {
            if (!centralizeOnStop)
                return;
            MoveToIndex(GetCentralizedObject().CurrentIndex, 0.2f);
        }

        private void ApplyOffsetIfNeeded()
        {
            if (mIsVertical && Mathf.Abs(ScrollRect.content.anchoredPosition.y) > CONTENT_OFFSET_FIXER_LIMIT)
            {
                var v = (ScrollRect.content.anchoredPosition.y > 0 ? -CONTENT_OFFSET_FIXER_LIMIT : CONTENT_OFFSET_FIXER_LIMIT);
                ScrollRect.content.anchoredPosition = new Vector2(ScrollRect.content.anchoredPosition.x, ScrollRect.content.anchoredPosition.y + v);
                Vector2 objAnchoredPos;
                listPool.ForEach(x =>
                {
                    objAnchoredPos.x = x.Rect.anchoredPosition.x;
                    objAnchoredPos.y = x.Rect.anchoredPosition.y - v;
                    x.Rect.anchoredPosition = objAnchoredPos;
                });
            }

            if (mIsHorizontal && Mathf.Abs(ScrollRect.content.anchoredPosition.x) > CONTENT_OFFSET_FIXER_LIMIT)
            {
                var v = (ScrollRect.content.anchoredPosition.x > 0 ? -CONTENT_OFFSET_FIXER_LIMIT : CONTENT_OFFSET_FIXER_LIMIT);
                ScrollRect.content.anchoredPosition = new Vector2(ScrollRect.content.anchoredPosition.x + v, ScrollRect.content.anchoredPosition.y);
                Vector2 objAnchoredPos;
                listPool.ForEach(x =>
                {
                    objAnchoredPos.x = x.Rect.anchoredPosition.x - v;
                    objAnchoredPos.y = x.Rect.anchoredPosition.y;
                    x.Rect.anchoredPosition = objAnchoredPos;
                });
            }
        }

        private void StopScrollAndChangeContentPosition(Vector2 pos)
        {
            ScrollRect.StopMovement();
            ScrollRect.enabled = false;
            ScrollRect.content.anchoredPosition = pos;
            ScrollRect.enabled = true;
        }

        private ScrollDirection LimitScroll()
        {
            var invalidDirections = ScrollDirection.NONE;
            var lowestObj = GetLowest();
            if(lowestObj == null) return ScrollDirection.NONE;
            var lowestPos = lowestObj.Rect.anchoredPosition;
            var highestObj = GetHighest();
            if(highestObj == null) return ScrollDirection.NONE;
            var highestPos = highestObj.Rect.anchoredPosition;
            var contentPos = ScrollRect.content.anchoredPosition;

            if (mIsVertical)
            {
                if (highestObj.CurrentIndex == 0)
                {
                    //Going Down
                    var limit = ScrollRect.viewport.rect.height;
                    var objPosY = contentPos.y + highestPos.y + spacing + limit;

                    if (objPosY < limit)
                    {
                        mClampedPosition = new Vector2(contentPos.x, contentPos.y + limit - objPosY);
                        forceMoveTween?.Kill();

                        if (mMovementType == UnityEngine.UI.ScrollRect.MovementType.Clamped)
                            StopScrollAndChangeContentPosition(mClampedPosition);
                        invalidDirections |= ScrollDirection.DOWN;
                        invalidDirections &= ~ScrollDirection.NONE;
                    }
                }
                if (lowestObj.CurrentIndex == infoList.Count - 1)
                {
                    //Going Up
                    var objPosY = contentPos.y + lowestPos.y + ScrollRect.viewport.rect.height - spacing;
                    var limit = lowestObj.CurrentHeight;

                    if (objPosY > limit)
                    {
                        mClampedPosition = new Vector2(contentPos.x, contentPos.y + limit - objPosY);
                        ScrollRect.clampedPosition = mClampedPosition;
                        forceMoveTween?.Kill();

                        if (mMovementType == UnityEngine.UI.ScrollRect.MovementType.Clamped)
                            StopScrollAndChangeContentPosition(mClampedPosition);
                        invalidDirections |= ScrollDirection.UP;
                        invalidDirections &= ~ScrollDirection.NONE;
                    }
                }
            }
            else if (mIsHorizontal)
            {
                if (highestObj.CurrentIndex == infoList.Count - 1)
                {
                    //Going Left
                    var objPosX = ScrollRect.content.anchoredPosition.x + highestPos.x + spacing + highestObj.CurrentWidth;
                    var limit = ScrollRect.viewport.rect.width;
                    if (objPosX < limit)
                    {
                        mClampedPosition = new Vector2(contentPos.x + limit - objPosX, contentPos.y);
                        ScrollRect.clampedPosition = mClampedPosition;
                        forceMoveTween?.Kill();

                        if (mMovementType == UnityEngine.UI.ScrollRect.MovementType.Clamped)
                            StopScrollAndChangeContentPosition(mClampedPosition);
                        invalidDirections |= ScrollDirection.LEFT;
                        invalidDirections &= ~ScrollDirection.NONE;
                    }
                }
                if (lowestObj.CurrentIndex == 0)
                {
                    //Going Right
                    var objPosX = ScrollRect.content.anchoredPosition.x + lowestPos.x;
                    var limit = 0;

                    if (objPosX > limit)
                    {
                        mClampedPosition = new Vector2(contentPos.x + limit - objPosX, contentPos.y);
                        forceMoveTween?.Kill();

                        if (mMovementType == UnityEngine.UI.ScrollRect.MovementType.Clamped)
                            StopScrollAndChangeContentPosition(mClampedPosition);
                        invalidDirections |= ScrollDirection.RIGHT;
                        invalidDirections &= ~ScrollDirection.NONE;
                    }
                }
            }

            return invalidDirections;
        }

        public bool CanMove(ScrollDirection directions)
        {
            if (((directions & ScrollDirection.DOWN) == ScrollDirection.DOWN) && ((mLastInvalidDirections & ScrollDirection.DOWN) == ScrollDirection.DOWN))
                return false;
            if (((directions & ScrollDirection.UP) == ScrollDirection.UP) && ((mLastInvalidDirections & ScrollDirection.UP) == ScrollDirection.UP))
                return false;
            if (((directions & ScrollDirection.LEFT) == ScrollDirection.LEFT) && ((mLastInvalidDirections & ScrollDirection.LEFT) == ScrollDirection.LEFT))
                return false;
            if (((directions & ScrollDirection.RIGHT) == ScrollDirection.RIGHT) && ((mLastInvalidDirections & ScrollDirection.RIGHT) == ScrollDirection.RIGHT))
                return false;
            return true;
        }

        public void ToggleScroll(bool active)
        {
            ScrollRect.enabled = active;
            ScrollRect.viewport.anchorMin = new Vector2(0, 0);
            ScrollRect.viewport.anchorMax = new Vector2(1, 1);
            ScrollRect.viewport.offsetMin = new Vector2(0, 0);
            ScrollRect.viewport.offsetMax = new Vector2(0, 0);
            ScrollRect.viewport.pivot = new Vector2(0.5f, 0.5f);

            if (!active)
                ScrollRect.content.anchoredPosition = Vector2.zero;
        }

        private void UpdateObjectsCentralizedPosition()
        {
            var objs = listPool.GetAllWithState(true);
            foreach (var obj in objs)
            {
                var x = (Mathf.Abs(obj.Rect.anchoredPosition.x) - Mathf.Abs(ScrollRect.content.anchoredPosition.x)) + (obj.CurrentWidth / 2f);
                var y = (Mathf.Abs(obj.Rect.anchoredPosition.y) - Mathf.Abs(ScrollRect.content.anchoredPosition.y)) + (obj.CurrentHeight / 2f);
                obj.SetPositionInViewport(new Vector2(x, y), new Vector2(Mathf.Abs(x - ScrollRect.viewport.rect.width / 2f), Mathf.Abs(y - ScrollRect.viewport.rect.height / 2f)));
            }
        }

        public void MoveToIndex(int index, float? totalTime = null, float? timePerElement = null)
        {
            if (index >= infoList.Count)
                throw new Exception("Invalid index to move: " + index);

            if (!totalTime.HasValue && !timePerElement.HasValue)
                throw new Exception("Either send totalTime or timePerElement to make MoveToIndex work.");

            var refObject = listPool.GetAllWithState(true)[0];
            index = Mathf.Clamp(index, 0, infoList.Count - 1);
            forceMoveTween?.Kill();
            ScrollRect.StopMovement();
            ScrollRect.needElasticReturn = false;
            var amountToGo = Mathf.Abs(GetCentralizedObject().CurrentIndex - index);
            var time = totalTime ?? timePerElement.Value * amountToGo;

            var pos = mIsHorizontal ? -((index * (refObject.CurrentWidth + spacing)) - (ScrollRect.viewport.rect.width / 2f) + (refObject.CurrentWidth / 2f))
                : ((index * (refObject.CurrentHeight + spacing)) - (ScrollRect.viewport.rect.height / 2f) + (refObject.CurrentHeight / 2f));
            forceMoveTween = (mIsHorizontal ? ScrollRect.content.DOAnchorPosX(pos, time) : ScrollRect.content.DOAnchorPosY(pos, time)).SetEase(Ease.OutQuint);
        }

        public T1 GetCentralizedObject()
        {
            UpdateObjectsCentralizedPosition();

            var objs = listPool.GetAllWithState(true);
            var distFromCenter = float.MaxValue;
            T1 centerObject = null;
            foreach (var obj in objs)
            {
                var pos = Mathf.Abs((mIsVertical ? (obj.PositionInViewport.y - ScrollRect.viewport.rect.height / 2f) :
                    (obj.PositionInViewport.x - ScrollRect.viewport.rect.width / 2f)));
                if (pos > distFromCenter)
                    continue;

                centerObject = obj;
                distFromCenter = pos;
            }

            return centerObject;
        }

        public T1 GetLowest()
        {
            var min = float.MaxValue;
            T1 lowestObj = null;
            var objs = listPool.GetAllWithState(true);

            foreach (var t in objs)
            {
                var anchoredPosition = t.Rect.anchoredPosition;

                if (mIsVertical && anchoredPosition.y < min || mIsHorizontal && anchoredPosition.x < min)
                {
                    min = mIsVertical ? anchoredPosition.y : anchoredPosition.x;
                    lowestObj = t;
                }
            }

            return lowestObj;
        }

        public T1 GetHighest()
        {
            var max = float.MinValue;
            T1 highestObj = null;
            var objs = listPool.GetAllWithState(true);
            foreach (var t in objs)
            {
                var anchoredPosition = t.Rect.anchoredPosition;

                if (mIsVertical && anchoredPosition.y > max || mIsHorizontal && anchoredPosition.x > max)
                {
                    max = mIsVertical ? anchoredPosition.y : anchoredPosition.x;
                    highestObj = t;
                }
            }

            return highestObj;
        }
    }
}