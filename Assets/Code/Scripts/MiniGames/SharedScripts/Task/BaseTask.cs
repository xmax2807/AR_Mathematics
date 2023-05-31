using System.Collections.Generic;
using System.Linq;
using Project.Utils.ExtensionMethods;
namespace Project.MiniGames
{
    public abstract class BaseTask
    {
        public int Goal;
        public string TaskDescription;
        public int CurrentProgress { get; protected set; }
        public bool IsCompleted => CurrentProgress >= Goal;
        public event System.Action OnTaskCompleted;
        public event System.Action<int> OnProgressChanged;
        protected void InvokeTaskCompleted() => OnTaskCompleted?.Invoke();
        public BaseTask(int goal, string description)
        {
            Goal = goal;
            TaskDescription = description;
        }
        public virtual void UpdateProgress(int value)
        {
            CurrentProgress += value;

            OnProgressChanged?.Invoke(CurrentProgress);
            if (IsCompleted)
            {
                OnTaskCompleted?.Invoke();
            }
        }
        public abstract bool IsCorrect(object value);
    }
    public class SequenceTask : BaseTask
    {
        private readonly List<BaseTask> TaskList;
        public BaseTask CurrentTask => TaskList[CurrentProgress.EnsureInIndexRange(Goal)];
        public int SubTaskGoal => CurrentTask.Goal;
        public event System.Action<BaseTask> OnTaskChanged;
        public SequenceTask(params BaseTask[] tasks) : base(tasks.Length, "")
        {
            TaskList = tasks.ToList();
        }
        public SequenceTask(int goal, string description) : base(goal, description)
        {
            TaskList = new(goal);
            //OnTaskChanged += TrackSubtask;
        }

        // private void TrackSubtask(BaseTask task){
        //     task.OnTaskCompleted += SubTaskCompleted;
        // }

        public void AddTask(BaseTask task)
        {
            TaskList.Add(task);
            task.OnTaskCompleted += SubTaskCompleted;
        }
        public override void UpdateProgress(int value)
        {
            if (IsCompleted)
            {
                InvokeTaskCompleted();
                return;
            }

            CurrentTask.UpdateProgress(value);

            // if (CurrentTask.IsCompleted)
            // {
            //     CurrentProgress = System.Math.Min(CurrentProgress + 1, Goal);

            //     if (CurrentProgress == Goal)
            //     {
            //         UnityEngine.Debug.Log("Completed");
            //         InvokeTaskCompleted();
            //         return;
            //     }
            //     OnTaskChanged?.Invoke(CurrentTask);
            // }
        }

        private void SubTaskCompleted()
        {
            CurrentProgress = System.Math.Min(CurrentProgress + 1, Goal);

            if (CurrentProgress == Goal)
            {
                UnityEngine.Debug.Log("Completed");
                InvokeTaskCompleted();
                return;
            }
            OnTaskChanged?.Invoke(CurrentTask);
        }

        public override bool IsCorrect(object value)
        {
            return CurrentTask.IsCorrect(value);
        }
        public void Clear(){
            TaskList?.Clear();
        }
    }
}