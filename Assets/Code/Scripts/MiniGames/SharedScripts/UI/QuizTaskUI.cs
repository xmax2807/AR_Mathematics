using UnityEngine;
using UnityEngine.UI;
using Project.UI.TrueFalseUI;
using Project.Managers;

namespace Project.MiniGames{
    public class QuizTaskUI : BaseTaskUI
    {
        [SerializeField] private TrueFalseButton buttonPrefab;
        [SerializeField] private Transform buttonGroupTransform;
        private TrueFalseButton[] options;
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
            if(task is not IVisitableSCQTask quizTask){
                return;
            }
            
            question.text = quizTask.GetQuestion();
            string[] quizOptions = quizTask.GetOptions();
            
            options = new TrueFalseButton[quizOptions.Length];

            SpawnerManager.Instance.SpawnObjectsList(buttonPrefab, quizOptions.Length, buttonGroupTransform, 
            (obj, index)=>{
                options[index].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = quizOptions[index];
                options[index].Button.onClick.RemoveAllListeners();
                options[index].Button.onClick.AddListener(()=>OnOptionChosen(quizTask, index));  
            });
        }

        private void OnOptionChosen(IVisitableSCQTask task ,int index){
            
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