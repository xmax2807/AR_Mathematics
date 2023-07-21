using Project.UI.Panel;
using UnityEngine;
namespace Project.MiniGames.TutorialGames{
    public class TutorialBehaviour : MonoBehaviour, ITutorial{
        [SerializeField] private ContextGroup contexts;
        [SerializeField] private MenuPanelController menuPanelController;
        private ITutorial _tutorial;
        private Commander _commander;
        private ITutorial Tutorial{
            get{
                _tutorial ??= SettingUp();
                return _tutorial;
            }
        }

        private ITutorial SettingUp(){
            _commander = new Commander(this, menuPanelController);
            _commander.OnStageEnded += NextStage;
            ITutorial result = new Tutorial(contexts, _commander);
            return result;
        }

        private void Awake(){
            _tutorial = SettingUp();
            _ = menuPanelController.Hide();
        }

        public void Start(){
            Begin();
        }

        public void Begin(){
            Tutorial.Begin();
        }   

        public void End(){
            Tutorial.End();
        }

        public void NextStage(){
            Tutorial.NextStage();
        }
    }
}