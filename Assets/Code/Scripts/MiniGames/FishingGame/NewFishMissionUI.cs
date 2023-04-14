using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using Project.QuizSystem;
using Project.UI;
using UnityEngine;
namespace Project.MiniGames.FishingGame{
    public class NewFishMissionUI : MonoBehaviour{
        [SerializeField] private TMPro.TextMeshProUGUI MissionContent;
        [SerializeField] private TMPro.TextMeshProUGUI MissionGoal;
        private AutoGeneratedTaskGiver Giver;
        [SerializeField] PanelViewBase panelView;

        private bool canClose;
        private void Awake(){
            Giver = FindObjectOfType<AutoGeneratedTaskGiver>();

            if(Giver == null){
                Debug.LogWarning("Mission Giver is not existed for progress");
                return;
            }
            Giver.OnTaskChanged += Show;
        }
        private async void Show(BaseTask task){
            canClose = false;
            UpdateQuestion(task);
            await panelView.ShowAsync();
            await Task.Delay(1000);
            canClose = true;
        }
        public void Hide(){
            if(!canClose) return;

            panelView.HideAsync();
        }

        private void UpdateQuestion(BaseTask task){
            MissionContent.text = task.TaskDescription;
            MissionGoal.text = $"\nSố lượng: {task.Goal}";
        }

        public void SpeakText(){
            Project.Managers.AudioManager.Instance.Speak(MissionContent.text);
        }
    }
}