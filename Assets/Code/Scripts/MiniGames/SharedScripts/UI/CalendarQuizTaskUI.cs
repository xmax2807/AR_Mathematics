using Project.Managers;
using UnityEngine;

namespace Project.MiniGames{
    public class CalendarQuizTaskUI : QuizTaskUI
    {
        private Calendar calendar;
        private CalendarTask currentCalendarTask;

        [SerializeField] private Animator TrueFalseAnsGIF;

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
                    UpdateUI(currentCalendarTask);
                }
            }
        }

        protected override void OnCorrectAnswer()
        {
            PlayAnimation(true, NextQuestion);
            //wrong: WrongAnswerAnimation
            //NextQuestion();
        }

        protected override void OnWrongAnswer()
        {
            PlayAnimation(false, () =>
            {
                //End the game
                foreach (var option in options)
                {
                    option.Button.onClick.RemoveAllListeners();
                }
                UIEventManager.Unlock();
                EndGameButton.gameObject.SetActive(true);
            });
        }

        private void PlayAnimation(bool result, System.Action postAnimCallback)
        {
            TrueFalseAnsGIF.gameObject.SetActive(true);
            TrueFalseAnsGIF.SetBool("isCorrect", result);

            var stateInfo = TrueFalseAnsGIF.GetCurrentAnimatorStateInfo(0);
            TimeCoroutineManager.Instance.WaitForSeconds(stateInfo.length, () =>
            {
                TrueFalseAnsGIF.gameObject.SetActive(false);
                postAnimCallback?.Invoke();
            });
        }

        private void EndGame(){
            BaseGameEventManager.Instance.RaiseEvent<bool>(BaseGameEventManager.EndGameEventName, false);
        }
    }
}