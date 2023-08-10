using UnityEngine;
using Gameframe.GUI.PanelSystem;
using Project.UI.CountdownTimer;
using System;
using Project.UI.Indicator;
using Project.QuizSystem.UIFactory;
using UnityEngine.UI;
using Project.Managers;
using Project.QuizSystem.SaveLoadQuestion;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project.UI.Panel{
    public class TimerPagerUIManager : ViewPagerUIManager{
        [SerializeField] private Button SubmitTest;
        [SerializeField] private BaseCountdownTimer timer;
        [SerializeField] private ProgressIndicator Indicator;
        [SerializeField] private OkCancelPanelView readyToStartTestView;
        [SerializeField] private QuizResultPanelView quizResult;
        private TestModel TestModel => UserManager.Instance.CurrentTestModel;
        private UserModel CurrentUser => UserManager.Instance.CurrentUser;
        private QuizPagerUI quizPagerUI;
        
        private QuizPagerController realController; 
        protected override void Awake()
        {
            if(pagerController is not QuizPagerController controller){
                throw new Exception("Controller is not the same type of pager");
            }
            realController = controller;
            
            readyToStartTestView.ShowAsync();
            readyToStartTestView.onConfirm += StartTheTest;

            Indicator.OnIndexChanged += MoveTo;
            base.Awake();
            
            timer.SetMinutes(TestModel.TestTime);
            timer.DoneCountingdown += FinishTest;
        }

        private void PopResultPanel()
        {
            quizResult.CalculateResult();
            quizResult.ShowAsync();
        }

        private void StartTheTest(){
            SubmitTest.interactable = false;

            quizResult.SetupQuestions(realController.AllQuestions);
            readyToStartTestView.HideAsync();
            // quizNavigationController?.Show();
            // SetupNavigator();

            SetupIndicator();
            MoveTo(0);
            if(currentPagerUI is QuizPagerUI pagerUI){
                AddQuizPager(pagerUI);
            }
            timer.StartTimer();
        }

        private void AddQuizPager(QuizPagerUI pagerUI){
                quizPagerUI = pagerUI;
                quizPagerUI.PrefferedUIState = AnswerUIState.Answered;
                quizPagerUI.OnUserAnsweredHandler += OnUserAnswered;
        }

        // private void SetupNavigator(){
        //     if(quizNavigationController == null || data == null) return;

        //     data.ButtonNames = new ButtonData[pagerController.PreloadList.Count];
        //     for(int i = 0; i < data.ButtonNames.Length; i++){
        //         int currentIndex = i;

        //         var buttonClickedEvent = new UnityEngine.UI.Button.ButtonClickedEvent();
        //         buttonClickedEvent.AddListener(()=>MoveTo(currentIndex));

        //         data.ButtonNames[i] = new ButtonData{
        //             Name = $"{i + 1}",
        //             OnClick = buttonClickedEvent,
        //         };
        //     }

        //     quizNavigationController.SetUI(data);
        // }


        // private void SetupNavigator(){
        //     if(quizNavigationController == null || data == null) return;
            
        //     data.ButtonNames = new ButtonData[pagerController.PreloadList.Count];
        //     for(int i = 0; i < data.ButtonNames.Length; i++){
        //         int currentIndex = i;
                
        //         var buttonClickedEvent = new UnityEngine.UI.Button.ButtonClickedEvent();
        //         buttonClickedEvent.AddListener(()=>MoveTo(currentIndex));
                
        //         data.ButtonNames[i] = new ButtonData{
        //             Name = $"{i + 1}",
        //             OnClick = buttonClickedEvent,
        //         };
        //     }

        //     quizNavigationController.SetUI(data);
        // }

        private void OnUserAnswered(QuizSystem.IQuestion question){
            Indicator.MarkAsCompleted();
            SubmitTest.interactable = this.quizResult.IsAllQuestionAnswered();
        }

        private void SetupIndicator(){
            Indicator.ManualFetchItem(pagerController.PreloadList.Count);
            pagerController.OnPageChanged += (view)=>Indicator.MoveTo(pagerController.CurrentIndex);
        }
    
        public void ViewQuizAnswer(){
            quizResult.HideAsync();
            quizPagerUI.PrefferedUIState = AnswerUIState.Result;
            Indicator.RevealAnswer(quizResult.AnsResults);
            MoveTo(0);
        }
        public void FinishTest(){
            timer.StopTimer();
            quizPagerUI.OnUserAnsweredHandler -= OnUserAnswered;

            PopResultPanel();
            Save();
        }

        private void Save(){
            QuizSystem.IQuestion[] allQuestions = realController.AllQuestions;
            List<QuestionSaveData> questionSaveList = new();

            foreach (var question in allQuestions)
            {
                if(question is ISavableQuestion savableQuestion){
                    questionSaveList.Add(savableQuestion.ConvertToData());
                }
            }
            
            string userId = "test";
            if(CurrentUser != null){
                userId = CurrentUser.UserID;
            }
            SemesterTestSaveData data = new(){
                UserId = userId,
                Id = $"{userId}_{TestModel.TestSemester}",
                Title = $"{TestModel.TestTitle}",
                Semester = TestModel.TestSemester,
                NumberOfCorrections = quizResult.CorrectedQuestionCount,
                NumberOfQuestions = allQuestions.Length,
                ListData = questionSaveList.ToArray(),
            };
            ResourceManager.Instance.SaveTestAsync(data, null);
        }
        
    }
}