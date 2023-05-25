using UnityEngine;
using Project.MiniGames;
using System;

namespace Project.MiniGames{
    public class VCNVQuizTaskUI : QuizTaskUI, IEventListener{
        [SerializeField] private EventSTO ColliderEvent;

        protected override void OnEnable()
        {
            base.OnEnable();
            //ColliderEvent?.RegisterListener(this);
            Debug.Log("register Event");
            VCNVGameEventManager.Instance.RegisterEvent(VCNVGameEventManager.ObstacleReachEventName, this, ShowQuizUI);
        }

        private void ShowQuizUI()
        {
            canvas.enabled = true;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            //ColliderEvent?.UnregisterListener(this);
        }
        protected override void OnCorrectAnswer()
        {
            canvas.enabled = false;
            BaseGameEventManager.RealInstance<VCNVGameEventManager>().AnswerResultEvent.Raise<bool>(true);
            NextQuestion();
        }
        protected override void OnWrongAnswer()
        {
            UIEventManager?.Unlock();
            canvas.enabled = false;
            BaseGameEventManager.RealInstance<VCNVGameEventManager>().AnswerResultEvent.Raise<bool>(false);
        }

        public string UniqueName => "VCNVQuizTaskUI";
        public void OnEventRaised<T>(EventSTO sender, T result)
        {
            canvas.enabled = true;
        }

        public EventSTO GetEventSTO()
        {
           return this.ColliderEvent;
        }
    }
}