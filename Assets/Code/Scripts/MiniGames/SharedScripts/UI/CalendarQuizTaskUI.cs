using Project.Managers;
using UnityEngine;

namespace Project.MiniGames{
    public class CalendarQuizTaskUI : QuizTaskUI
    {
        private Calendar calendar;
        private CalendarTask currentCalendarTask;

        protected override void OnEnable()
        {
            base.OnEnable();
            EndGameButton.onClick.AddListener(EndGame);
            EndGameButton.gameObject.SetActive(false);
        }

        protected override void UpdateUI(BaseTask task)
        {
            base.UpdateUI(task);
            if(task is not CalendarTask calendarTask){
                return;
            }
            currentCalendarTask = calendarTask;
            calendar?.SetDate(calendarTask.GetAnswer());
        }
        public void OnPlacedCalendar(UnityEngine.GameObject obj){
            if(obj.TryGetComponent<Calendar>(out this.calendar)){
                BaseGameEventManager.Instance.RaiseEvent(ARGameEventManager.ObjectPlacedEventName);
                canvas.enabled = true;

                if(currentCalendarTask != null){
                    Debug.Log("Calendar: "  + currentCalendarTask.GetAnswer().ToLongDateString());
                    UpdateUI(currentCalendarTask);
                }
            }
        }

        protected override void OnWrongAnswer()
        {
            foreach(var option in options){
                option.Button.onClick.RemoveAllListeners();
            }
            UIEventManager.Unlock();
            EndGameButton.gameObject.SetActive(true);
        }

        private void EndGame(){
            BaseGameEventManager.Instance.RaiseEvent<bool>(BaseGameEventManager.EndGameEventName, false);
        }
    }
}