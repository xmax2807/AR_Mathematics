using Project.QuizSystem;
using Project.UI;
using Project.UI.ProgressBar;
using UnityEngine;
using UnityEngine.UI;
namespace Project.MiniGames.ObjectFinding{
    public class ObjectFindingMissionUI : MonoBehaviour{
        [SerializeField] private ProgressBarController OverallGameTask;
        [SerializeField] private SwapableImage Image;
        [SerializeField] private TMPro.TextMeshProUGUI MissionContent;
        [SerializeField] private TaskGiver Giver;

        private void Awake(){
            Giver.OnInitialized += Initialize;
            Giver.OnTaskChanged += UpdateQuestion;
        }
        void Initialize(){
            //var CurrentRewardData = Giver.CurrentReward;

            //Image.ManualSetup(CurrentRewardData.UnacquiredBadge,CurrentRewardData.Badge.GetReward(), Giver.AllTaskCompleted);
            //OverallGameTask.SetupAnimation(Giver.Tasks.Goal);

            //UpdateQuestion(Giver.CurrentTask);
            //UpdateProgress();
        }
        void OnDestroy(){
            Giver.OnTaskChanged -= UpdateQuestion;   
        }
        private void UpdateProgress(){
            if(Giver.Tasks.IsCompleted) {
                OverallGameTask.UpdateEndValue(Giver.Tasks.CurrentProgress, 1);
                OverallGameTask.StartAnimation();
                return;
            }

            Image.UpdateUI();
            OverallGameTask.UpdateEndValue(Giver.Tasks.CurrentProgress, 1);
            OverallGameTask.StartAnimation();
        }

        private void UpdateQuestion(BaseTask task){
            Debug.Log(task.TaskDescription);
            MissionContent.text = task.TaskDescription;
        }
    }
}