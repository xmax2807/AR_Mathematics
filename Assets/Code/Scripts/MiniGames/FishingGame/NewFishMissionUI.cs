using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using Project.QuizSystem;
using Project.MiniGames.UI;
using UnityEngine;
namespace Project.MiniGames.FishingGame{
    public class NewFishMissionUI : ToggleTaskUIAppearance{
        private AudioClip cache;
        public void SpeakText(){
            if(cache != null){
                Debug.Log("Playing cache");
                Managers.AudioManager.Instance.Speak(cache);
                return;
            }
            Project.Managers.AudioManager.Instance.Speak(giver.CurrentTask.TaskDescription, (clip) => {
                cache = clip;
            });
        }

        protected override void UpdateUI(BaseTask task)
        {
            base.UpdateUI(task);
            ToggleUI();
            cache = null;
        }
    }
}