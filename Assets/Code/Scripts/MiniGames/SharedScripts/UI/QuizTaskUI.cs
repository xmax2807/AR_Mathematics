using UnityEngine;
using UnityEngine.UI;
using Project.UI.TrueFalseUI;
using Project.Managers;

namespace Project.MiniGames{
    public class QuizTaskUI : BaseTaskUI
    {
        [SerializeField] private TrueFalseButton[] options;
        [SerializeField] private TMPro.TextMeshProUGUI question;
        protected override void OnDisable()
        {
            base.OnDisable();
            foreach(TrueFalseButton option in options){
                option.Button.onClick.RemoveAllListeners();
            }
        }
        protected override void UpdateUI(BaseTask task)
        {
            if(task is not IVisitableQuizTask quizTask){
                return;
            }
            
            question.text = quizTask.GetQuestion();
            string[] quizOptions = quizTask.GetOptions();
            for(int i = 0; i < options.Length; i++){
                int currentIndex = i;
                options[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = quizOptions[i];
                options[i].Button.onClick.RemoveAllListeners();
                options[i].Button.onClick.AddListener(()=>OnOptionChosen(quizTask, currentIndex));  
            }
        }

        private void OnOptionChosen(IVisitableQuizTask task ,int index){
            
            bool isCorrect = task.IsCorrect(index);
            options[index].ChangeUI(isCorrect);
            
            if(isCorrect){
                TimeCoroutineManager.Instance.WaitForSeconds(1, NextQuestion);
            }
            else{
                options[task.GetAnswerIndex()].ChangeUI(isCorrect);
                TimeCoroutineManager.Instance.WaitForSeconds(1, ResetOptions);
            }
        }

        private void NextQuestion(){
            giver.Tasks.UpdateProgress(1);
        }
        private void ResetOptions(){
            foreach(TrueFalseButton button in options){
                button.Reset();
            }
        }
    }
}