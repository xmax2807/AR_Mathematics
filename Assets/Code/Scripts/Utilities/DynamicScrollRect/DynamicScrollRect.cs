using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;

namespace Project.UI.DynamicScrollRect{
    /// <summary>
    /// Event class listens to user actions (drag, touch) for scrollRect.
    /// </summary>
    public class DynamicScrollRectEvent : UnityEvent<PointerEventData>{}

    public class DynamicScrollRect : ScrollRect{
        [SerializeField] private RectTransform _viewPort;
        [SerializeField] private RectTransform _content;

        public DynamicScrollRectEvent OnBeginDragEvent = new();
        public DynamicScrollRectEvent OnEndDragEvent = new();
        public DynamicScrollRectEvent OnStopDragEvent = new();

        public new MovementType MovementType;
        public bool needElasticReturn;
        public Vector2 clampedPosition;

        private bool dragging = false;
        private bool isWaitingToStop = false;
        private Vector2 pointerStartLocalCursor = Vector2.zero;

        #region DragEventMethods

        public override void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent?.Invoke(eventData);

            if (MovementType != MovementType.Elastic)
            {
                base.OnBeginDrag(eventData);
                return;
            }

            dragging = true;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            UpdateBounds();

            pointerStartLocalCursor = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out pointerStartLocalCursor);
            m_ContentStartPosition = content.anchoredPosition;

            base.OnBeginDrag(eventData);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            isWaitingToStop = true;
            dragging = false;
            base.OnEndDrag(eventData);
            OnEndDragEvent?.Invoke(eventData);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            if (MovementType != MovementType.Elastic)
            {
                base.OnDrag(eventData);
                return;
            }

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out var localCursor))
                return;

            UpdateBounds();

            var pointerDelta = localCursor - pointerStartLocalCursor;
            var position = m_ContentStartPosition + pointerDelta;

            var offset = CalculateOffset(position - content.anchoredPosition);
            position += offset;
            var viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);

            if (needElasticReturn)
            {
                if (offset.x != 0)
                    position.x -= RubberDelta(offset.x, viewBounds.size.x);
                if (offset.y != 0)
                    position.y -= RubberDelta(offset.y, viewBounds.size.y);
            }

            SetContentAnchoredPosition(position);
        }

        private float RubberDelta(float overStretching, float viewSize)
        {
            return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
        }
        #endregion

        protected override void Awake()
        {
            if(viewport == null) viewport = _viewPort;
            if(content == null) content = _content;
            base.Awake();
        }
        protected override void LateUpdate()
        {
            // check if user stop dragging.
            if (isWaitingToStop && velocity.magnitude < 0.01f)
            {
                OnMovementStop();
            }

            if (MovementType != MovementType.Elastic)
            {
                base.LateUpdate();
                return;
            }

            if (!content) return; // content is null

            EnsureLayoutHasRebuilt();
            UpdateBounds();

            DoSlideMotion();
            
            base.LateUpdate();
        }

        private void OnMovementStop()
        {
            isWaitingToStop = false;
            OnStopDragEvent?.Invoke(null);
        }

        /// <summary>
        /// Check if layout is rebuilt and force canvas to update
        /// </summary>
        private void EnsureLayoutHasRebuilt()
        {
            if (!CanvasUpdateRegistry.IsRebuildingLayout())
                Canvas.ForceUpdateCanvases();
        }

        private void DoSlideMotion(){
            float deltaTime = Time.unscaledDeltaTime;
            Vector2 offset = CalculateOffset(Vector2.zero);
            if (!dragging && (offset != Vector2.zero || velocity != Vector2.zero))
            {
                Vector2 position = content.anchoredPosition;
                Vector2 currentVel = velocity;

                for (var axis = 0; axis < 2; axis++)
                {
                    if (offset[axis] != 0)
                    {
                        float speed = velocity[axis];
                        position[axis] = Mathf.SmoothDamp(content.anchoredPosition[axis], content.anchoredPosition[axis] + offset[axis], ref speed, elasticity, Mathf.Infinity, deltaTime);

                        if (Mathf.Abs(speed) < 1)
                            speed = 0;
                        
                        currentVel[axis] = speed;
                    }
                    else if (inertia)
                    {
                        currentVel[axis] *= Mathf.Pow(decelerationRate, deltaTime);

                        if (Mathf.Abs(velocity[axis]) < 1)
                            currentVel[axis] = 0;

                        position[axis] += velocity[axis] * deltaTime;
                    }
                    else
                    {
                        currentVel[axis] = 0;
                    }
                }

                velocity = currentVel;

                SetContentAnchoredPosition(position);
            }

        }

        private Vector2 CalculateOffset(Vector2 delta)
        {
            var mViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
            return InternalCalculateOffset(ref mViewBounds, ref delta);
        }

        internal Vector2 InternalCalculateOffset(ref Bounds viewBounds, ref Vector2 delta)
        {
            Vector2 offset = Vector2.zero;

            if (!needElasticReturn)
                return offset;

            Vector2 contentAnchorPos = content.anchoredPosition;
            Vector2 min = new(contentAnchorPos.x - content.rect.width / 2, (contentAnchorPos.y - clampedPosition.y) - content.rect.height / 2);
            Vector2 max = new((contentAnchorPos.x - clampedPosition.x) + content.rect.width / 2, contentAnchorPos.y + content.rect.height / 2);

            if (horizontal)
            {
                min.x += delta.x;
                max.x += delta.x;

                if (min.x > viewBounds.min.x)
                    offset.x = viewBounds.min.x - min.x;
                else if (max.x < viewBounds.max.x)
                    offset.x = viewBounds.max.x - max.x;
            }

            if (vertical)
            {
                min.y += delta.y;
                max.y += delta.y;

                if (max.y < viewBounds.max.y)
                    offset.y = viewBounds.max.y - max.y;
                else if (min.y > viewBounds.min.y)
                    offset.y = viewBounds.min.y - min.y;
            }

            return offset;
        }

        // public new void OnValidate()
        // {
        //     base.OnValidate();

        //     if(viewport == null) viewport = _viewPort;
        //     if(content == null) content = _content;
        // }
    }
}