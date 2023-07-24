using System.Threading.Tasks;
using Project.UI.Panel;
using UnityEngine;
using UnityEngine.UI;
namespace Project.MiniGames.TutorialGames
{
    public class PracticeTaskUIController : MonoBehaviour{
        [SerializeField] private Gameframe.GUI.PanelSystem.PanelViewBase panelView;
        [SerializeField] private TMPro.TextMeshProUGUI questionText;
        [SerializeField] private Button speakButton;

        void OnEnable(){
            speakButton.onClick.AddListener(Speak);
        }
        void OnDisable(){
            speakButton.onClick.RemoveListener(Speak);
        }

        private void Speak(){
            Managers.AudioManager.Instance.Speak(questionText ==  null ? "" : questionText.text);
        }

        public Task ShowAsync(){
            return panelView.ShowAsync();
        }
        public void ShowImmediate(){
            panelView.ShowImmediate();
        }
        public Task HideAsync(){
            return panelView.HideAsync();
        }
        public void HideImmediate(){
            panelView.HideImmediate();
        }

        public void SetQuestionText(string text){
            if(questionText != null){
                questionText.text = text;
            }
        }
    }
}