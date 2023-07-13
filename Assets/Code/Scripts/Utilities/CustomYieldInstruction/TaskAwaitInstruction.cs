using System.Threading.Tasks;
using UnityEngine;

namespace Project{
    public class TaskAwaitInstruction : CustomYieldInstruction
    {
        private Task task;
        public override bool keepWaiting => !task.IsCompleted;

        public TaskAwaitInstruction(Task task){
            this.task = task;
        }
    }
}