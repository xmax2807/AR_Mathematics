using UnityEngine;

namespace Project.UI.GameObjectUI{
    public class GameObjectButton : MonoBehaviour, ITouchableObject
    {
        private string id;
        public string UniqueID => id;

        private bool m_interactable = true;
        public bool Interactable { get => m_interactable; set => m_interactable = value; }

        public UnityEngine.Events.UnityEvent<GameObjectButton> OnButtonTouchEvent;
        public event System.Action<GameObjectButton> OnButtonTouch;
        public event System.Action<GameObjectButton, Vector3> OnButtonTouchPosition;

        public void Awake(){
            //Create new Guid
            id = System.Guid.NewGuid().ToString();
        }

        public bool Equals(ITouchableObject other)
        {
            return other.UniqueID == this.UniqueID;
        }

        public void OnTouch(Touch touch)
        {
            if(touch.phase == TouchPhase.Ended){
                OnButtonTouch?.Invoke(this);
                OnButtonTouchEvent?.Invoke(this);
                OnButtonTouchPosition?.Invoke(this, touch.position);
            }
        }
    }
}