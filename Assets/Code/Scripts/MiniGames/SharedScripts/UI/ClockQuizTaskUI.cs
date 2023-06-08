using Project.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Project.MiniGames
{
    public class ClockQuizTaskUI : QuizTaskUI
    {
        private ClockTask currentClockTask;
        private Clock clock;
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
            if (task is not ClockTask clockTask)
            {
                return;
            }
            currentClockTask = clockTask;
            if (clock != null)
            {
                clock?.SetTime(currentClockTask.GetAnswer(), 1);
                TimeCoroutineManager.Instance.WaitForSeconds(1, () => canvas.enabled = true);
            }
        }
        public void SetClockObject(GameObject obj)
        {
            if (obj.TryGetComponent<Clock>(out this.clock))
            {
                GameManager.Instance.OnGameFinishLoading?.Invoke();
                canvas.enabled = true;

                Debug.Log("currentClock: " + currentClockTask?.GetAnswer());

                if (currentClockTask != null)
                {
                    UpdateUI(currentClockTask);
                }
            }
        }

        protected override void OnCorrectAnswer()
        {
            PlayAnimation(true, () =>
            {
                canvas.enabled = false;
                NextQuestion();
            });
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

        private void EndGame()
        {
            BaseGameEventManager.Instance.RaiseEvent<bool>(BaseGameEventManager.EndGameEventName, false);
        }
    }
}