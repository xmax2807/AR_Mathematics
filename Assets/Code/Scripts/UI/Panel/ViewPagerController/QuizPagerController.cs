using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using Project.Managers;
using Project.QuizSystem;
using UnityEngine;
namespace Project.UI.Panel{
    public class QuizPagerController : ViewPagerControllerT<PreloadableQuizPanelView>{
        [SerializeField] private LoadingPanelView loadingView;
        QuizModel[] QuizModels => UserManager.Instance.CurrentQuizzes;
        [SerializeField] private QuizGenerator generator;

        public IQuestion[] AllQuestions {get;private set;}

        protected override void Awake()
        {
            AllQuestions = new IQuestion[QuizModels.Length];
            base.Awake();
        }

        protected override async void SetupList (){
            loadingView.SetupUI("Đang tải câu hỏi, em chờ chút nhé...");
            await loadingView.ShowAsync();
            await generator.InitAsset();
            
            await FetchPanelView(QuizModels.Length, OnBuildUIView);
            if(preloadList.Count == 0){
                await loadingView.HideAsync();
                // Báo là k có câu hỏi
                return; 
            }
            InvokeOnPageChanged(0);
            LoadMore();

            _=preloadList[0].ShowAsync();
            _=loadingView.HideAsync();
        }

        protected override Task OnBuildUIView(PreloadableQuizPanelView view, int index)
        {
            QuizModel current = QuizModels[index];
            int quizChapter = current.QuizChapter;
            int quizUnit = current.QuizUnit;
            string quizTitle = current.QuizTitle;

            //Debug.Log($"Quiz info: img:{current.QuizIMG}, title: {current.QuizTitle}, chap-unit:{current.QuizChapter} - {current.QuizUnit}");

            IQuestion question = generator.CreateRandomQuestion(quizUnit, quizChapter, quizTitle);
            
            if(question == null)// Can't random question
            {
                question = new GeneralQuestion(current.QuizTitle, current.QuizAnswer, current.QuizCorrectAnswer, current.QuizIMG);
            }
            var answerUI = generator.GetAnswerUI(question.QuestionType);
            var quizUIContent = generator.GetQuizUIContent(question.QuestionContentType);
            view.Setup(question, quizUIContent, answerUI);
            AllQuestions[index] = question;
            return Task.CompletedTask;
        }
    }
}