using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using Project.Managers;
using Project.QuizSystem;
using Project.QuizSystem.SaveLoadQuestion;
using UnityEngine;
namespace Project.UI.Panel{
    public class TestResultPagerController : ViewPagerControllerT<PreloadableQuizPanelView>{
        [SerializeField] private LoadingPanelView loadingView;
        QuizModel[] QuizModels => UserManager.Instance.CurrentQuizzes;
        [SerializeField] private QuizGenerator generator;

        public IQuestion[] AllQuestions {get;private set;}
        private TestModel TestModel => UserManager.Instance.CurrentTestModel;
        private SemesterTestSaveData savedData;

        // protected override async void Awake()
        // {
        //     base.Awake();
        //     await generator.InitAsset();
        // }

        protected override async void SetupList (){
            loadingView.SetupUI("Đang tải câu hỏi");
            await loadingView.ShowAsync();
            await generator.InitAsset();
            //await LoadSavedData();
            AllQuestions = new IQuestion[QuizModels.Length];
            await FetchPanelView(QuizModels.Length, OnBuildUIView);
            SetResults();
            if(preloadList.Count == 0){
                await loadingView.HideAsync();
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

            IQuestion question = CreateQuestion(current, index);

            var answerUI = generator.GetAnswerUI(question.QuestionType);
            var quizUIContent = generator.GetQuizUIContent(question.QuestionContentType);

            view.Setup(question, quizUIContent, answerUI);
            AllQuestions[index] = question;
            
            return Task.CompletedTask;   
        }

        private IQuestion CreateQuestion(QuizModel current, int index){
            int quizChapter = current.QuizChapter;
            int quizUnit = current.QuizUnit;
            string quizTItle = current.QuizTitle;
            IQuestion question = generator.CreateQuestion(quizUnit, quizChapter, quizTItle);
            
            if(question == null)// Can't random question
            {
                question = new GeneralQuestion(current.QuizTitle, current.QuizAnswer, current.QuizCorrectAnswer, current.QuizIMG);
            }
            Debug.Log($"{index}:{question.GetQuestion()}");
            return question;
        }

        // private async Task LoadSavedData(){
        //     savedData = await ResourceManager.Instance.LoadTestAsync(TestModel.TestSemester);
        // }
        private void SetResults(){
            if(savedData == null) return;
            int currentDataIndex = 0;
            for(int i = 0; i < AllQuestions.Length; ++i){
                if(AllQuestions[i] is ISavableQuestion savableQuestion){
                    savableQuestion.SetData(savedData.ListData[currentDataIndex]);
                    ++currentDataIndex;
                }
                else{
                    Debug.Log("Skipped " + AllQuestions[i].GetQuestion());
                }
            }
        }

        public void ManualSetSavedTest(SemesterTestSaveData data){
            savedData = data;
        }
    }
}