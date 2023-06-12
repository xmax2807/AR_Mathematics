using UnityEngine;

namespace Project.UI.GameObjectUI{
    public class GameObjectButton : MonoBehaviour, ITouchableObject
    {
        private string id;
        public string UniqueID => id;
        public UnityEngine.Events.UnityEvent<GameObjectButton> OnButtonTouchEvent;
        public event System.Action<GameObjectButton> OnButtonTouch;

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
            }
        }
    }
}