using System.Collections.Generic;
using System.Threading.Tasks;
using Project.QuizSystem;
using Project.RewardSystem;
using Project.Utils;
using UnityEngine;

namespace Project.MiniGames
{
    public class AutoGeneratedTaskGiver : TaskGiver
    {
        [SerializeField] private TaskSTO Template;

        public override void ResetTask()
        {
            Tasks?.Clear();

            //Tasks
            Tasks = new(CurrentReward.Goal, "");
            Tasks.OnTaskChanged += base.ChangeTask;

            for (int i = 0; i < CurrentReward.Goal; i++)
            {
                BaseTask task = Template.GetTask();
                Tasks.AddTask(task);
            }
        }

        protected override async Task InitTasks()
        {
            await base.InitTasks();
            if (CurrentReward == null)
            {
                return;
                //TODO Endless mode
            }
            await Task.Run(() =>
            {
                Tasks = new(CurrentReward.Goal, "");
                Tasks.OnTaskChanged += base.ChangeTask;

                for (int i = 0; i < CurrentReward.Goal; i++)
                {
                    BaseTask task = Template.GetTask();
                    Tasks.AddTask(task);
                }
            });
            ChangeTask(Tasks.CurrentTask);
        }
    }
}