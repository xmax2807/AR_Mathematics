using UnityEngine;
using Project.Managers;
using System.Collections.Generic;

namespace Project.MiniGames{
    [CreateAssetMenu(menuName ="STO/GameMission/TaskSTO", fileName ="TaskSTO")]
    public class TaskSTO : ScriptableObject{
        public enum TaskType{
            ShapeTask, ClockTask, CalendarTask, HouseBuldingTask, FindingObjectTask, VCNVTask
        }
        public TaskType Type;
        public int Goal;
        public string Description;
        public BaseTask GetTask(){
            return Type switch
            {
                TaskType.ShapeTask => new ShapeTask(Goal, Description),
                TaskType.ClockTask => new ClockTask(Goal, Description),
                TaskType.CalendarTask => new CalendarTask(Goal, Description),
                TaskType.HouseBuldingTask => new HouseBuildingTask(Goal, Description),
                TaskType.FindingObjectTask => new FindingObjectTask(Goal, Description),
                TaskType.VCNVTask => new VCNVTask(UserManager.Instance.CurrentUnitProgress.chapter,Goal, Description),
                _ => null,
            };
        }
    }
}