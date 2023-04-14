using System.Text;
using Project.QuizSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel
{
    public class QuizPagerUI : ViewPagerUI<PreloadableQuizPanelView>
    {
        [SerializeField] private LayoutGroup answersContainer;
        [SerializeField] private Transform content;
        [SerializeField] private TMPro.TextMeshProUGUI questionText;

        private IQuestion currentQuestion;
        protected override void InitT(PreloadableQuizPanelView panelView)
        {
            Manager.HideNavigator();
            panelView.AnswerUI.OnAnswerCorrect -= AskShowNavigator;
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
            panelView.ContentUI.CreateUI(content, currentQuestion);
            panelView.AnswerUI.CreateUI(answersContainer, currentQuestion);
            questionText.text = currentQuestion.GetQuestion();
            panelView.AnswerUI.OnAnswerCorrect += AskShowNavigator;
        }
        private void AskShowNavigator(bool answerResult)
        {
            // Tell Manager to show navigator
            Manager.ShowNavigator();
        }
    }
}