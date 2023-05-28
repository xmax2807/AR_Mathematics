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
        private string defaultText;

        public string UniqueName => "FalseResultUI";

        public void OnEventRaised<T>(EventSTO sender, T result)
        {
        }

        private void Awake(){
            defaultText = result?.text;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Debug.Log("task result enable");
            BaseGameEventManager.Instance?.RegisterEvent<bool>(BaseGameEventManager.EndGameEventName, this, OnResult);
        }
        protected override void Start()
        {
            base.Start();
            view?.HideAsync();
        }
        protected override void UpdateUI(BaseTask task)
        {
            Debug.Log("Update UI");
            // int progress = this.giver.Tasks.CurrentProgress;
            // int goal = this.giver.Tasks.Goal;
            
            // result.text = $"{defaultText} {progress}/{goal}";

            // progressBarController.SetupAnimation(goal);
            // progressBarController.UpdateEndValue(progress);
            // progressBarController.StartAnimation();
        }
        private void UpdateUI(){
            int progress = this.giver.Tasks.CurrentProgress;
            int goal = this.giver.Tasks.Goal;
            
            result.text = $"{defaultText} {progress}/{goal}";

            progressBarController.SetupAnimation(goal);
            progressBarController.UpdateEndValue(progress, 3);
            
        }
        protected async void OnResult(bool result){
            UpdateUI();
            await view?.ShowAsync();
            progressBarController.StartAnimation();
        }
    }
}