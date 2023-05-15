using System.Collections;
using Project.QuizSystem;
using Project.QuizSystem.UIFactory;
using Project.QuizSystem.QuizUIContent;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Project.UI.Panel{
    public class PreloadableQuizPanelView : PreloadablePanelView
    {
        public IQuestion Question {get;private set;}
        public QuizUIContent ContentUI {get; private set;}
        public AnswerUI AnswerUI {get; private set;}
        public AnswerUIState AnswerUIState {get;private set;}
        public void Setup(IQuestion question, QuizUIContent contentUI, AnswerUI answerUI){
            this.Question = question;
            this.ContentUI = contentUI;
            this.AnswerUI = answerUI;
        }
        public override IEnumerator PrepareAsync()
        {
            if(isPrepared) yield break;

            Task getContentTask =  ContentUI.GetQuestionInfo(Question);
            yield return new UnityEngine.WaitUntil(()=>getContentTask.IsCompleted);
            isPrepared = true;
        }

        public override IEnumerator UnloadAsync()
        {
            isPrepared = false;
            yield break;
        }
        public void ChangeAnswerUIState(AnswerUIState state){
            AnswerUIState = state;
            AnswerUI.ChangeUIState(state);
        }
    }
}