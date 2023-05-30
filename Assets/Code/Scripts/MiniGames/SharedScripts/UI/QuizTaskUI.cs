using UnityEngine;
using UnityEngine.UI;
using Project.UI.TrueFalseUI;
using Project.Managers;
using Gameframe.GUI.Camera.UI;

namespace Project.MiniGames{
    public class QuizTaskUI : BaseTaskUI
    {
        protected UIEventManager UIEventManager => UIEventManager.Current; 
        [SerializeField] private TrueFalseButton buttonPrefab;
        [SerializeField] private Transform buttonGroupTransform;
        private TrueFalseButton[] options = new TrueFalseButton[0];
        [SerializeField] private TMPro.TextMeshProUGUI question;

        protected Canvas canvas;

        private void Awake(){
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

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
            if(IsDebugging){
                canvas.enabled = true;
            }
            
            question.text = quizTask.GetQuestion();
            string[] quizOptions = quizTask.GetOptions();
            
            foreach(TrueFalseButton button in options){
                Destroy(button.gameObject);
            }

            options = new TrueFalseButton[quizOptions.Length];

            SpawnerManager.Instance.SpawnObjectsList(buttonPrefab, quizOptions.Length, buttonGroupTransform, 
            (obj, index)=>{
                options[index] = obj;
                options[index].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = quizOptions[index];
                options[index].Button.onClick.RemoveAllListeners();
                options[index].Button.onClick.AddListener(()=>OnOptionChosen(quizTask, index));  
            });
        }

        private void OnOptionChosen(IVisitableSCQTask task ,int index){
            
            bool isCorrect = task.IsCorrect(index);

            options[index].ChangeUI(isCorrect);
            options[task.GetAnswerIndex()].ChangeUI(true);

            UIEventManager?.Lock();

            if(isCorrect){
                OnCorrectAnswer();
            }
            else{
                OnWrongAnswer();
            }
        }

        protected virtual void OnCorrectAnswer(){
            TimeCoroutineManager.Instance.WaitForSeconds(1, NextQuestion);
        }
        protected virtual void OnWrongAnswer(){    
            TimeCoroutineManager.Instance.WaitForSeconds(1, ResetOptions);
        }

        protected void NextQuestion(){
            giver.Tasks.UpdateProgress(1);
            UIEventManager?.Unlock();
        }
        private void ResetOptions(){
            foreach(TrueFalseButton button in options){
                button.Reset();
            }
            UIEventManager?.Unlock();
        }
    }
}