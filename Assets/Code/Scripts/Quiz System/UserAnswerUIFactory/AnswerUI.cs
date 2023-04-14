using UnityEngine.UI;
using UnityEngine;
using Project.Managers;

namespace Project.QuizSystem.UIFactory
{
    public abstract class AnswerUI
    {
        public event System.Action<bool> OnAnswerCorrect;
        protected GameObject prefab;
        protected IQuestion question;
        public AnswerUI(GameObject prefabSample){
            prefab = prefabSample;
        }
        public void CreateUI(UnityEngine.UI.LayoutGroup layoutGroup, IQuestion question){
            GetQuestionInfo(question);
            BuildUI(layoutGroup);
        }
        protected abstract void BuildUI(LayoutGroup layoutGroup);
        protected void InvokeAnswerEvent(bool result) => OnAnswerCorrect?.Invoke(result);
        protected virtual void GetQuestionInfo(IQuestion question){
            this.question = question;
        }
    }

    public class SingleChoiceAnswerUI : AnswerUI
    {
        private string[] options;
        private Button button;
        public SingleChoiceAnswerUI(GameObject prefabSample) : base(prefabSample){
            if (!prefabSample.TryGetComponent<Button>(out button)){
                throw new MissingComponentException("Button is missing in prefab");
            }
        }
        protected override void GetQuestionInfo(IQuestion question){
            if (question is not ISingleChoice singleChoiceQuestion)
            {
                this.options = new string[] { };
                Debug.LogWarning("question type is not SingleChoice");
            }
            else
            {
                this.question = question;
                this.options = singleChoiceQuestion.GetOptions();
            }
        }
        protected override void BuildUI(LayoutGroup layoutGroup)
        {
            for(int i = 0; i < options.Length; i++){
                SpawnerManager.Instance.SpawnObjectInParent(button,layoutGroup.transform,(btn)=>OnBuildUIElement(btn, i));
            }
        }
        private void OnBuildUIElement(Button obj, int index){
            obj.onClick.AddListener(()=>CheckAnswer(index));
            obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = options[index];
        }
        private void CheckAnswer(int index){
            question.SetAnswer(index);
            bool result = question.IsCorrect();
            Debug.Log(result);
            InvokeAnswerEvent(result);
        }
    }
}