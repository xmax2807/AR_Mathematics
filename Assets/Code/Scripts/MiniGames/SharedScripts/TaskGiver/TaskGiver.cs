using UnityEngine;
using Project.Managers;
using System.Threading.Tasks;
using Project.RewardSystem;

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
        public bool AllTaskCompleted()=> Tasks.IsCompleted;
        protected virtual void Awake(){
            GameManager.Instance.OnGameFinishLoading += InitTasks; 
        }
        protected virtual void OnDestroy(){
            GameManager.Instance.OnGameFinishLoading -= InitTasks;
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