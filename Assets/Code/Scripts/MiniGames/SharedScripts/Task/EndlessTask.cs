using System.Collections.Generic;

namespace Project.MiniGames{
    public class EndlessTask : SequenceTask
    {
        private SequenceTask sequenceTask;
        private TaskSTO template;
        public override List<BaseTask> TaskList => sequenceTask.TaskList;
        public override BaseTask CurrentTask => sequenceTask.CurrentTask;
        public EndlessTask(int goal, string description, TaskSTO template) : base(goal, description)
        {
            this.template = template;
            CreateNewTasks();
        }

        public override bool IsCorrect(object value)
        {
            return sequenceTask.IsCorrect(value);
        }

        private void CreateNewTasks(){
            UnityEngine.Debug.Log("Endless tasks is creating");
            sequenceTask?.Clear();
            sequenceTask = new SequenceTask(this.Goal, "");


            for(int i = 0; i < this.Goal; ++i){
                sequenceTask.AddTask(template.GetTask());
            }
            sequenceTask.OnTaskChanged += InvokeOnTaskChanged;
            sequenceTask.OnTaskCompleted += CreateNewTasks;

            InvokeOnTaskChanged(sequenceTask.CurrentTask);
        }
    }
}