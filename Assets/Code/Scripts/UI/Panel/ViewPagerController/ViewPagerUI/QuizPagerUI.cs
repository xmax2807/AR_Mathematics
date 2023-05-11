using System.Text;
using Project.QuizSystem;
using Project.QuizSystem.UIFactory;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel
{
    public class QuizPagerUI : ViewPagerUI<PreloadableQuizPanelView>
    {
        [SerializeField] private AnswerUIController answersContainer;
        [SerializeField] private Transform content;
        [SerializeField] private TMPro.TextMeshProUGUI questionText;
        public event System.Action<IQuestion> OnUserAnsweredHandler;
        private IQuestion currentQuestion;
        public AnswerUIState PrefferedUIState;
        protected override async void InitT(PreloadableQuizPanelView panelView)
        {
            Manager.HideNavigator();
            Manager.ToggleLoadingView(true);
            
            panelView.AnswerUI.OnUserAnswered -= OnUserAnswered;
            foreach (Transform child in answersContainer.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            //Before updating current view
            base.InitT(panelView);
            //After updating current view
            currentQuestion = panelView.Question;
            await panelView.ContentUI.CreateUI(content, currentQuestion);
            panelView.AnswerUI.CreateUI(answersContainer, currentQuestion);
            questionText.text = currentQuestion.GetQuestion();
            panelView.AnswerUI.OnUserAnswered += OnUserAnswered;
            
            panelView.ChangeAnswerUIState(PrefferedUIState);
            panelView.AnswerUI.SetAnswer();// Set prev answer if any

            Manager.ToggleLoadingView(false);
        }
        protected virtual void OnUserAnswered(bool answerResult){
            AskShowNavigator();
            OnUserAnsweredHandler?.Invoke(currentQuestion);
        }
        private void AskShowNavigator()
        {
            // Tell Manager to show navigator
            Manager.ShowNavigator();
        }
    }
}