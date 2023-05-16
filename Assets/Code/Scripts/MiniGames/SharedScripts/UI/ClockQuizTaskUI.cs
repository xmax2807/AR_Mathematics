using Project.Managers;
using UnityEngine;

namespace Project.MiniGames{
    public class ClockQuizTaskUI : QuizTaskUI{

        private Clock clock;
        private Canvas canvas;
        private void Awake(){
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }
        protected override void UpdateUI(BaseTask task)
        {
            base.UpdateUI(task);
            if(task is not ClockTask clockTask){
                return;
            }
            clock?.SetTime(clockTask.GetAnswer());
        }
        public void SetClockObject(GameObject obj){
            if(obj.TryGetComponent<Clock>(out this.clock)){
                GameManager.Instance.OnGameFinishLoading?.Invoke();
                canvas.enabled = true;
            }
        }
    }
}