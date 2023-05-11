using System.Threading;
using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using Project.QuizSystem;
using UnityEngine;
using UnityEngine.UI;
namespace Project.UI.Panel{
    public class QuizResultPanelView : OkCancelPanelView{
        [SerializeField] private TMPro.TextMeshProUGUI resultText;
        private IQuestion[] questions;
        private int correctedQuestionCount;
        private bool isCalculated;
        public void SetupQuestions(IQuestion[] questions) {
            this.questions = questions;
            isCalculated = false;
        }
        public void CalculateResult(){
            if(isCalculated) return;
            
            foreach(IQuestion question in questions){
                if(question.IsCorrect()){
                    correctedQuestionCount++;
                }
            }
            isCalculated = true;
        }

        public override Task ShowAsync(CancellationToken cancellationToken)
        {
            resultText.text = $"{correctedQuestionCount}/{questions.Length}";
            return base.ShowAsync(cancellationToken);
        }

        public bool IsAllQuestionAnswered(){
            foreach(IQuestion question in questions){
                if(!question.HasAnswered()) return false;
            }
            return true;
        }
    }
}