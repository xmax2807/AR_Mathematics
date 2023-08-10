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
        public int CorrectedQuestionCount {get;private set;}
        public bool[] AnsResults {get;private set;} 
        private bool isCalculated;
        public void SetupQuestions(IQuestion[] questions) {
            this.questions = questions;
            isCalculated = false;
        }
        public void CalculateResult(){
            if(isCalculated) return;
            AnsResults = new bool[questions.Length];

            for(int i = 0; i < questions.Length; ++i){
                bool result = questions[i].IsCorrect();
                if(result == true){
                    ++CorrectedQuestionCount;
                }
                AnsResults[i] = result;
            }
            
            isCalculated = true;
        }

        public override Task ShowAsync(CancellationToken cancellationToken)
        {
            resultText.text = $"{CorrectedQuestionCount}/{questions.Length}";
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