using UnityEngine;

namespace Project.MiniGames.UI{
    public class DefaultTaskUIWrappee : TaskUIWrappee
    {
        [SerializeField] private TMPro.TextMeshProUGUI MissionContent;
        [SerializeField] private TMPro.TextMeshProUGUI MissionGoal;
        public override void ManuallyUpdateUI(BaseTask task)
        {
            if(MissionContent != null){
                MissionContent.text = task.TaskDescription;
            }

            if(MissionGoal != null){
                MissionGoal.text = $"\nSố lượng: {task.Goal}";
            }
        }
    }
}