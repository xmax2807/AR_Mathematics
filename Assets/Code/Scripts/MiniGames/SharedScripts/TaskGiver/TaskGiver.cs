using UnityEngine;
using Project.Managers;
using System.Threading.Tasks;
using Project.RewardSystem;
using Gameframe.GUI.PanelSystem;

namespace Project.MiniGames{

    /// <summary>
    /// This class act like a Quest Manager. Whenever the world event is fired, this will filter down to quest and check correct.
    /// </summary>
    public abstract class TaskGiver : MonoBehaviour{
        [SerializeField] private RewardScriptableObject[] RewardData;
        [SerializeField] private bool IsDebugging = false;
        public event System.Action OnInitialized;
        public RewardScriptableObject CurrentReward {
            get{
                if(CurrentTaskIndex < 0 || CurrentTaskIndex >= RewardData.Length){
                    return null;
                }
                return RewardData[CurrentTaskIndex];
            }
        } 
        protected int CurrentTaskIndex;
        public SequenceTask Tasks { get; protected set; }
        public BaseTask CurrentTask => Tasks.CurrentTask;
        public event System.Action<BaseTask> OnTaskChanged;
        public UnityEngine.Events.UnityEvent<RewardBadgeSTO> OnRewardAccquired;

        protected GameController GameController => DatabaseManager.Instance.GameController;
        // [SerializeField] private PanelViewControllerBehaviour rewardPanelController;
        // [SerializeField] private PanelStackSystem stackPanel;

        public bool AllTaskCompleted()=> Tasks.IsCompleted;
        protected virtual async void Awake(){
            CurrentTaskIndex = 0;
            GameManager.Instance.OnGameFinishLoading += Initialize;
            if(IsDebugging){
                await Initialize();
            }
        }
        protected virtual void OnDestroy(){
            GameManager.Instance.OnGameFinishLoading -= Initialize;
        }
        private async void CompleteMission(){
            
            OnRewardAccquired?.Invoke(CurrentReward.Badge);
            Debug.Log(CurrentTaskIndex);
            CurrentTaskIndex++;
            GameController.SaveGame(CurrentTaskIndex);
            await Initialize();
            //stackPanel.Push(rewardPanelController);
            //pusher.Push();
        }
        private async Task Initialize(){
            await InitTasks();
            Tasks.OnTaskCompleted += CompleteMission;
            OnInitialized?.Invoke();
        }
        protected virtual Task InitTasks(){
            GameModel currentGame = UserManager.Instance.CurrentGame;
            if(currentGame == null) return Task.CompletedTask;
            
            this.CurrentTaskIndex =  GameController.GetLastSavedTask(UserManager.Instance.CurrentGame.GameId);
            Debug.Log(CurrentTaskIndex);
            return Task.CompletedTask;
        }
        protected virtual void ChangeTask(BaseTask task)
        {
            // Safely raise the event for all subscribers
            OnTaskChanged?.Invoke(task);
        }
        public bool IsCorrect(object item){
            return CurrentTask.IsCorrect(item);
        }
    }
}