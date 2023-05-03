using UnityEngine;
using Project.Utils;
using System.Collections.Generic;

namespace Project.MiniGames{
    [CreateAssetMenu(menuName ="STO/GameMission/TaskSTO", fileName ="TaskSTO")]
    public class TaskSTO : ScriptableObject{
        public enum TaskType{
            ShapeTask, ClockTask
        }
        public TaskType Type;
        public int Goal;
        public string Description;
        public BaseTask GetTask(){
            return Type switch
            {
                TaskType.ShapeTask => new ShapeTask(Goal, Description),
                TaskType.ClockTask => new ClockTask(Goal, Description),
                _ => null,
            };
        }
    }
}