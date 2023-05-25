using UnityEngine;
using Project.UI.ProgressBar;
using Gameframe.GUI.PanelSystem;
using Gameframe.GUI.TransitionSystem;

namespace Project.MiniGames{
    public class FalseResultTaskUI : BaseTaskUI, IEventListener
    {
        [SerializeField] private TMPro.TextMeshProUGUI result;
        [SerializeField] private ProgressBarController progressBarController;
        [SerializeField] private OkCancelPanelView view; 

        public string UniqueName => "FalseResultUI";

        public void OnEventRaised<T>(EventSTO sender, T result)
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            BaseGameEventManager.Instance.RegisterEvent<bool>(BaseGameEventManager.EndGameEventName, this, OnResult);
        }
        protected override void Start()
        {
            base.Start();
            view?.HideAsync();
        }
        protected override void UpdateUI(BaseTask task)
        {
            int progress = this.giver.Tasks.CurrentProgress;
            int goal = this.giver.Tasks.Goal;
            
            result.text += $"{progress}/{goal}";

            progressBarController.SetupAnimation(goal);
            progressBarController.UpdateEndValue(progress);
            progressBarController.StartAnimation();
        }
        protected void OnResult(bool result){
            view?.ShowAsync();
            UpdateUI(giver.CurrentTask);
        }
    }
}