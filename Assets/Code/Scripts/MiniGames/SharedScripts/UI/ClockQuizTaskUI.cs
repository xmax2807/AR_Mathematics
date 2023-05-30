using Project.Managers;
using UnityEngine;

namespace Project.MiniGames{
    public class ClockQuizTaskUI : QuizTaskUI{
        
        private ClockTask currentClockTask;
        private Clock clock;
        protected override void UpdateUI(BaseTask task)
        {
            base.UpdateUI(task);
            if(task is not ClockTask clockTask){
                return;
            }
            currentClockTask = clockTask;
            clock?.SetTime(currentClockTask.GetAnswer());
        }
        public void SetClockObject(GameObject obj){
            if(obj.TryGetComponent<Clock>(out this.clock)){
                GameManager.Instance.OnGameFinishLoading?.Invoke();
                canvas.enabled = true;
                clock?.SetTime(currentClockTask.GetAnswer());
            }
        }
    }
}