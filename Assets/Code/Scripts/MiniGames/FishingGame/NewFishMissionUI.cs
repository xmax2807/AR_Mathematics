using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using Project.QuizSystem;
using Project.MiniGames.UI;
using UnityEngine;
namespace Project.MiniGames.FishingGame{
    public class NewFishMissionUI : ToggleTaskUIAppearance{
        public void SpeakText(){
            Project.Managers.AudioManager.Instance.Speak(giver.CurrentTask.TaskDescription);
        }

        protected override void UpdateUI(BaseTask task)
        {
            base.UpdateUI(task);
            ToggleUI();
        }
    }
}