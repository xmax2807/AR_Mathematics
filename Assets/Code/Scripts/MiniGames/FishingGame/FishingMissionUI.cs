using Project.QuizSystem;
using Project.UI;
using Project.UI.ProgressBar;
using UnityEngine;
using UnityEngine.UI;
namespace Project.MiniGames.FishingGame{
    public class FishingMissionUI : MonoBehaviour{
        [SerializeField] private Canvas gameTaskProgressCanvas;
        [SerializeField] private ProgressBarController OverallGameTask;
        [SerializeField] private SwapableImage Image;
        [SerializeField] private TMPro.TextMeshProUGUI MissionContent;
        [SerializeField] private TMPro.TextMeshProUGUI MissionProgressNumber;
        [SerializeField] private ProgressBarController MissionProgress;
        private Player player;
        private TaskGiver Giver => player.Giver;

        private void Awake(){
            player = FindObjectOfType<Player>();
            if(player == null){
                Debug.LogWarning("Player/ Mission Giver is not existed for progress");
                return;
            }
            
            player.OnCatchRightFish += UpdateProgress;
            Giver.OnTaskChanged += UpdateQuestion;
        }
        void Start(){
            var CurrentRewardData = Giver.CurrentReward;
            if(CurrentRewardData != null){

                Image.ManualSetup(CurrentRewardData.UnacquiredBadge,CurrentRewardData.Badge.GetReward(), Giver.AllTaskCompleted);
                OverallGameTask.SetupAnimation(Giver.Tasks.Goal);
                gameTaskProgressCanvas.enabled = true;
                return;
            }
            else{
                gameTaskProgressCanvas.enabled = false;
            }


            UpdateQuestion(Giver.CurrentTask);
            UpdateProgress();
        }
        void OnDestroy(){
            if(player == null) return;
            player.OnCatchRightFish -= UpdateProgress; 
            Giver.OnTaskChanged -= UpdateQuestion;   
        }
        private void UpdateProgress(){
            if(!Giver.Tasks.IsCompleted) {
                // OverallGameTask.UpdateEndValue(Giver.Tasks.CurrentProgress, 1);
                // OverallGameTask.StartAnimation();
                UpdateSubProgress(Giver.CurrentTask);
                return;
            }

            Image.UpdateUI();
            OverallGameTask.UpdateEndValue(Giver.Tasks.CurrentProgress, 1);
            OverallGameTask.StartAnimation();
        }

        private void UpdateQuestion(BaseTask task){
            MissionContent.text = task.TaskDescription;
            MissionProgress.SetupAnimation(task.Goal);
        }
        private void UpdateSubProgress(BaseTask task){
            MissionProgressNumber.text = $"{task.CurrentProgress} / {task.Goal}";
            MissionProgress.UpdateEndValue(task.CurrentProgress, 0.2f);
            MissionProgress.StartAnimation();
        }
    }
}