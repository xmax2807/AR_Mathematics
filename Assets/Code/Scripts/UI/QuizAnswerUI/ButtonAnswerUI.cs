using Project.QuizSystem.UIFactory;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.QuizAnswerUI{
    [RequireComponent(typeof(Button),typeof(Image))]
    public class ButtonAnswerUI : BaseQuizAnswerUI<Color>
    {
        public Button Button{get;private set;}
        private Image background;
        protected override void Awake(){
            base.Awake();
            Button = GetComponent<Button>();
            background = GetComponent<Image>();
        }

        public override void ChangeToSelectState()
        {
            background.color = SelectedUI;
        }

        public override void ChangeToTrueFalseState(bool isTrue)
        {
            background.color = isTrue ? TrueUI : FalseUI;
        }

        public override void Reset()
        {
            background.color = DefaultUI;
        }
    }
}