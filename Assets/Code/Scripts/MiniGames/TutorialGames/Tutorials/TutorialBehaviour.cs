using UnityEngine;
namespace Project.MiniGames.TutorialGames{
    public class TutorialBehaviour : MonoBehaviour, ITutorial{
        [SerializeField] private ContextGroup contexts;
        private ITutorial _tutorial;
        private ITutorial Tutorial{
            get{
                _tutorial ??= SettingUp();
                return _tutorial;
            }
        }

        private ITutorial SettingUp(){
            ITutorial result = new Tutorial(contexts, new Commander(this));
            return result;
        }

        private void Awake(){
            _tutorial = SettingUp();
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