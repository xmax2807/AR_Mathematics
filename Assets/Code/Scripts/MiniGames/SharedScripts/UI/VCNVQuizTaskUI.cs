using UnityEngine;
using Project.MiniGames;

namespace Project.MiniGames{
    public class VCNVQuizTaskUI : QuizTaskUI, IEventListener{
        [SerializeField] private QuizEventSTO QuizEvent;
        [SerializeField] private EventSTO ColliderEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            ColliderEvent?.RegisterListener(this);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ColliderEvent?.UnregisterListener(this);
        }
        protected override void OnCorrectAnswer()
        {
            QuizEvent?.Raise(true);
            canvas.enabled = false;
            NextQuestion();
        }
        protected override void OnWrongAnswer()
        {
            QuizEvent?.Raise(false);
            canvas.enabled = false;
        }

        public void OnEventRaised()
        {
            canvas.enabled = true;
        }

        public EventSTO GetEventSTO()
        {
           return this.ColliderEvent;
        }
    }
}