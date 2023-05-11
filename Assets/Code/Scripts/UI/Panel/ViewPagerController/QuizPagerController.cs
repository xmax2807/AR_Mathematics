using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using Project.Managers;
using Project.QuizSystem;
using UnityEngine;
namespace Project.UI.Panel{
    public class QuizPagerController : ViewPagerControllerT<PreloadableQuizPanelView>{
        [SerializeField] private LoadingPanelView loadingView;
        QuizModel[] quizModels => UserManager.Instance.CurrentQuizzes;
        [SerializeField] private QuizGenerator generator;

        public IQuestion[] AllQuestions {get;private set;}

        protected override void Awake()
        {
            base.Awake();
            _ = generator.InitAsset();
            AllQuestions = new IQuestion[quizModels.Length];
        }

        private async void Start(){
            loadingView.SetupUI("Đang tải câu hỏi, bé chờ chút nhé...");
            await loadingView.ShowAsync();
            //await generator.InitAsset();
            
            await FetchPanelView(quizModels.Length, OnBuildQuizUIView);
            
            InvokeOnPageChanged(0);
            LoadMore();

            _=preloadList[0].ShowAsync();
            _=loadingView.HideAsync();
        }

        private Task OnBuildQuizUIView(PreloadableQuizPanelView view, int index)
        {
            QuizModel current = quizModels[index];
            int quizChapter = current.QuizChapter;
            int quizUnit = current.QuizUnit;

            IQuestion question = generator.CreateRandomQuestion(quizUnit, quizChapter);
            
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