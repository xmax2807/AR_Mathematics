using System;
using UnityEngine;
using UnityEngine.Events;
using Project.Managers;

namespace Project.UI.CountdownTimer{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    class BaseCountdownTimer : MonoBehaviour{
        private TMPro.TextMeshProUGUI timerText;
        public UnityEvent<int> TimeChanged;
        public UnityEvent<int> TimeStarted;
        public event System.Action DoneCountingdown;
        private int timeRemainingInSeconds;
        private Coroutine isCountingdown;
        public int RemainingSeconds => timeRemainingInSeconds;

        private void Awake(){
            timerText = GetComponent<TMPro.TextMeshProUGUI>();
            DoneCountingdown += StopTimer;
        }

        private void OnDisable(){
            StopTimer();
        }
        private void OnDestroy(){
            StopTimer();
        }

        public void SetSeconds(int seconds){
            timeRemainingInSeconds = seconds + 1;
        }
        public void SetMinutes(int minutes){
            SetSeconds(minutes * 60);
        }
        public void SetHours(int hours){
            SetSeconds(hours * 3600);
        }

        public void StartTimer(){
            if(isCountingdown != null) return;

            TimeStarted?.Invoke(timeRemainingInSeconds);
            ResumeTimer();
        }
        public void ResumeTimer(){
            isCountingdown = TimeCoroutineManager.Instance.DoLoopAction(
                action: ChangeTime, 
                stopCondition: ()=>timeRemainingInSeconds <= 0, 
                delayInterval: 1, 
                endAction: ()=>DoneCountingdown?.Invoke()
            );
        }
        public void StopTimer(){
            PauseTimer();
            timeRemainingInSeconds = 0;
        }
        public void PauseTimer(){
            if(isCountingdown == null) return;
            TimeCoroutineManager.Instance.StopCoroutine(isCountingdown);
        }

        private void ChangeTime(){
            timeRemainingInSeconds--;
            TimeSpan time = TimeSpan.FromSeconds(timeRemainingInSeconds);
            timerText.text = $"Thời gian: {time.ToString(@"hh\:mm\:ss")}";
            TimeChanged?.Invoke(timeRemainingInSeconds);
        }
    }
}