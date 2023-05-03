using UnityEngine;
using UnityEngine.UI;

namespace Project.MiniGames{
    public class QuizTaskUI : BaseTaskUI
    {
        [SerializeField] private Button[] options;
        [SerializeField] private TMPro.TextMeshProUGUI question;
        protected override void OnDisable()
        {
            base.OnDisable();
            foreach(Button option in options){
                option.onClick.RemoveAllListeners();
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
                options[i].onClick.RemoveAllListeners();
                options[i].onClick.AddListener(()=>OnOptionChosen(quizTask, currentIndex));  
            }
        }

        private void OnOptionChosen(IVisitableQuizTask task ,int index){
            Debug.Log(index);
            if(task.IsCorrect(index)){
                giver.Tasks.UpdateProgress(1);
            }
        }
    }
}