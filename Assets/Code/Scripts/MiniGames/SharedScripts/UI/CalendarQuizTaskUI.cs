using Project.Managers;
using UnityEngine;

namespace Project.MiniGames{
    public class CalendarQuizTaskUI : QuizTaskUI
    {
        private Calendar calendar;
        protected override void UpdateUI(BaseTask task)
        {
            base.UpdateUI(task);
            if(task is not CalendarTask calendarTask){
                return;
            }
            calendar?.SetDate(calendarTask.GetAnswer());
        }
        public void OnPlacedCalendar(UnityEngine.GameObject obj){
            if(obj.TryGetComponent<Calendar>(out this.calendar)){
                GameManager.Instance.OnGameFinishLoading?.Invoke();
                canvas.enabled = true;
            }
        }
    }
}