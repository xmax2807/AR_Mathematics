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
        [SerializeField] private RewardScriptableObject RewardData;
        public RewardScriptableObject Reward => RewardData;
        public SequenceTask Tasks { get; protected set; }
        public BaseTask CurrentTask => Tasks.CurrentTask;
        public event System.Action<BaseTask> OnTaskChanged;
        public UnityEngine.Events.UnityEvent<RewardBadgeSTO> OnRewardAccquired;
        [SerializeField] private PanelViewControllerBehaviour rewardPanelController;
        [SerializeField] private PanelStackSystem stackPanel;

        public bool AllTaskCompleted()=> Tasks.IsCompleted;
        protected virtual void Awake(){
            GameManager.Instance.OnGameFinishLoading += Initialize; 
        }
        protected virtual void OnDestroy(){
            GameManager.Instance.OnGameFinishLoading -= Initialize;
        }
        private void CompleteMission(){
            
            OnRewardAccquired?.Invoke(Reward.Badge);
            //stackPanel.Push(rewardPanelController);
            //pusher.Push();
        }
        private async Task Initialize(){
            await InitTasks();
            Tasks.OnTaskCompleted += CompleteMission;
        }
        protected abstract Task InitTasks();
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