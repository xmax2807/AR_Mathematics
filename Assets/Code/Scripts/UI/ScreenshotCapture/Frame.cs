using UnityEngine;

namespace Project.UI.Screenshot{
    public class FixedSizeFrame : IFrame {
        private RectTransform m_rectTransform;
        public Vector2 GetSize() {
            return new Vector2(m_rectTransform.rect.width, m_rectTransform.rect.height);
        }
        public Vector2 GetPosition() {
            return m_rectTransform.position;
        }

        public Vector2 GetLocalPosition()
        {
            return m_rectTransform.localPosition;
        }

        public FixedSizeFrame(RectTransform rectTransform) {
            m_rectTransform = rectTransform;
        }
    }

    public class ResizableFrame : IFrame
    {
        private float m_width;
        private float m_height;

        public Vector2 GetLocalPosition()
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetPosition()
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetSize() {
            return new Vector2(m_width, m_height);
        }
    }
}