using UnityEngine;
using Project.Utils;
using System.Collections.Generic;

namespace Project.MiniGames{
    [CreateAssetMenu(menuName ="STO/GameMission/TaskSTO", fileName ="TaskSTO")]
    public class TaskSTO : ScriptableObject{
        public enum TaskType{
            ShapeTask,
        }
        public TaskType Type;
        public int Goal;
        public string Description;
        public BaseTask GetTask(){
            return Type switch
            {
                TaskType.ShapeTask => new ShapeTask(Goal, Description),
                _ => null,
            };
        }
    }
}