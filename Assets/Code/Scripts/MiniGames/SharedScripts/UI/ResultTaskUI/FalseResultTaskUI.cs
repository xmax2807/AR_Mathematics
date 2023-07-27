using UnityEngine;
using Project.UI.ProgressBar;
using Gameframe.GUI.PanelSystem;
using Gameframe.GUI.TransitionSystem;
using UnityEngine.UI;

namespace Project.MiniGames{
    public class FalseResultTaskUI : BaseTaskUI, IEventListener
    {
        [SerializeField] private TMPro.TextMeshProUGUI result;
        [SerializeField] private ProgressBarController progressBarController;
        [SerializeField] private OkCancelPanelView view;
        [SerializeField] private Button GiftBox;
        [SerializeField] private Button PlayAgainButton;
        public UnityEngine.Events.UnityEvent OnGiftBoxClicked;
        private string defaultText;

        public string UniqueName => "FalseResultUI";

        private Canvas canvas;
        public void OnEventRaised<T>(EventSTO sender, T result)
        {
        }

        private void Awake(){
            defaultText = result?.text;
            SetupGiftBox();
            canvas = GetComponent<Canvas>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            BaseGameEventManager.Instance?.RegisterEvent<bool>(BaseGameEventManager.EndGameEventName, this, OnResult);
            PlayAgainButton.onClick.AddListener(Managers.GameManager.Instance.ReLoadCurrentScene);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            PlayAgainButton.onClick.RemoveAllListeners();
        }
        protected override void Start()
        {
            base.Start();
            view?.HideAsync();
            canvas.enabled = false;
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
            if(PlayAgainButton != null){
                PlayAgainButton.interactable = result == false;
            }
            if(GiftBox != null){
                GiftBox.interactable = result == true;
            }
            UpdateUI();
            canvas.enabled = true;
            await view?.ShowAsync();
            progressBarController.StartAnimation();

            Audio.SoundFXController.SoundFXType type = result ? Audio.SoundFXController.SoundFXType.OnComplete: Audio.SoundFXController.SoundFXType.OnFail; 
            Managers.AudioManager.Instance.PlayEffect(type);
        }

        private void SetupGiftBox(){
            if(GiftBox != null){
                Button.ButtonClickedEvent giftBoxClickEvent = new Button.ButtonClickedEvent();
                giftBoxClickEvent.AddListener(()=>GiftBox.interactable = false);
                giftBoxClickEvent.AddListener(()=>OnGiftBoxClicked?.Invoke());
                GiftBox.onClick = giftBoxClickEvent;
                GiftBox.interactable = false;
            }
        }
    }
}