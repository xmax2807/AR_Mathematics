using UnityEngine;
namespace Project.UI.TrueFalseUI{
    public abstract class BaseTrueFalseUI<T> : MonoBehaviour {
        [SerializeField] protected T FalseUI;
        [SerializeField] protected T TrueUI;
        [SerializeField] protected T DefaultUI;
        protected virtual void Awake(){
            if(FalseUI == null) FalseUI = DefaultUI;
            if(TrueUI == null) TrueUI = DefaultUI;
        }
        public abstract void ChangeUI(bool isTrue);
        public abstract void Reset();
    }
}