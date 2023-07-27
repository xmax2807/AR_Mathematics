using Project.QuizSystem.UIFactory;
using UnityEngine;

namespace Project.UI.QuizAnswerUI{
    public abstract class BaseQuizAnswerUI<T> : MonoBehaviour{
        [SerializeField] protected T FalseUI;
        [SerializeField] protected T TrueUI;
        [SerializeField] protected T SelectedUI;
        [SerializeField] protected T DefaultUI;

        protected virtual void Awake(){
            if(FalseUI == null) FalseUI = DefaultUI;
            if(TrueUI == null) TrueUI = DefaultUI;
            if(SelectedUI == null) SelectedUI = DefaultUI;
        }
        public abstract void ChangeToSelectState();
        public abstract void ChangeToTrueFalseState(bool isTrue);
        public abstract void Reset();
    }
}