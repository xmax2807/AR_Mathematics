using Project.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Project.MiniGames{
    public class ClockQuizTaskUI : QuizTaskUI{
        private ClockTask currentClockTask;
        private Clock clock;

        protected override void OnEnable()
        {
            base.OnEnable();
            EndGameButton.onClick.AddListener(EndGame);
            EndGameButton.gameObject.SetActive(false);
        }
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

                Debug.Log("currentClock: " + currentClockTask?.GetAnswer());

                if(currentClockTask != null){
                    UpdateUI(currentClockTask);
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