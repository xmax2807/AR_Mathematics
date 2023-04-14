using System.Collections;
using Project.QuizSystem;
using Project.QuizSystem.UIFactory;
using Project.QuizSystem.QuizUIContent;
using UnityEngine.UI;

namespace Project.UI.Panel{
    public class PreloadableQuizPanelView : PreloadablePanelView
    {
        public IQuestion Question {get;private set;}
        public QuizUIContent ContentUI {get; private set;}
        public AnswerUI AnswerUI {get; private set;}
        public void Setup(IQuestion question, QuizUIContent contentUI, AnswerUI answerUI){
            this.Question = question;
            this.ContentUI = contentUI;
            this.AnswerUI = answerUI;
        }
        public override IEnumerator PrepareAsync()
        {
            isPrepared = true;
            yield break;
        }

        public override IEnumerator UnloadAsync()
        {
            isPrepared = false;
            yield break;
        }
    }
}