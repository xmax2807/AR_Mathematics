using UnityEngine.UI;
using UnityEngine;
using Project.Managers;
using Project.UI.TrueFalseUI;
using Project.UI.QuizAnswerUI;

namespace Project.QuizSystem.UIFactory
{
    public abstract class AnswerUI
    {
        public event System.Action<bool> OnUserAnswered;
        protected GameObject prefab;
        protected IQuestion question;
        protected AnswerUIController controller;
        protected AnswerUIState uiState;
        protected BaseAnswerUIState answerGroupState;
        public AnswerUI(GameObject prefabSample)
        {
            prefab = prefabSample;
            uiState = AnswerUIState.NotAnswered;
        }
        public void CreateUI(AnswerUIController layoutGroup, IQuestion question)
        {
            controller = layoutGroup;
            GetQuestionInfo(question);
            BuildUI(layoutGroup);
        }
        protected abstract void BuildUI(AnswerUIController layoutGroup);
        protected void InvokeAnswerEvent(bool result) => OnUserAnswered?.Invoke(result);
        protected virtual void GetQuestionInfo(IQuestion question)
        {
            this.question = question;
        }

        public void ChangeUIState(AnswerUIState state) => uiState = state;
        public void SetAnswer()
        {
            if (!question.HasAnswered()) return;
            
            if (uiState == AnswerUIState.Result)
            {
                controller.LockUI();
            }
            else
            {
                controller.UnlockUI();
            }

            SetUserAnswer();
            OnUserAnswered?.Invoke(question.IsCorrect());
        }

        protected abstract void SetUserAnswer();
    }

    public class SingleChoiceAnswerUI : AnswerUI
    {
        private string[] options;
        private ButtonAnswerUI button;
        private ButtonAnswerUI[] optionButtons;
        private int answerIndex;
        private int userAnsweredIndex;

        public SingleChoiceAnswerUI(GameObject prefabSample) : base(prefabSample)
        {
            if (!prefabSample.TryGetComponent<ButtonAnswerUI>(out button))
            {
                throw new MissingComponentException("Button is missing in prefab");
            }
        }
        protected override void GetQuestionInfo(IQuestion question)
        {
            if (question is not ISingleChoice singleChoiceQuestion)
            {
                this.options = new string[] { };
                Debug.LogWarning("question type is not SingleChoice");
            }
            else
            {
                answerIndex = singleChoiceQuestion.AnswerIndex;
                this.question = question;
                this.options = singleChoiceQuestion.GetOptions();
                optionButtons = new ButtonAnswerUI[options.Length];
            }
        }
        protected override void BuildUI(AnswerUIController layoutGroup)
        {
            for (int i = 0; i < options.Length; i++)
            {
                SpawnerManager.Instance.SpawnObjectInParent(button, layoutGroup.transform, (btn) => OnBuildUIElement(btn, i));
            }
            answerGroupState = new ButtonAnswerUIState(answerIndex, optionButtons);
        }
        private void OnBuildUIElement(ButtonAnswerUI obj, int index)
        {
            optionButtons[index] = obj;
            obj.Button.onClick.AddListener(() => CheckAnswer(index));
            obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = options[index];
        }
        private void CheckAnswer(int index)
        {
            question.SetAnswer(index);
            userAnsweredIndex = index;
            bool result = question.IsCorrect();

            InvokeAnswerEvent(result);
            SetUserAnswer();
        }

        protected void ChangeButtonsUI(int index)
        {
            answerGroupState.ChangeUserAnswer(index);
            answerGroupState.ChangeUI(uiState);
        }

        protected override void SetUserAnswer()
        {
            ChangeButtonsUI(userAnsweredIndex);
        }
    }
}