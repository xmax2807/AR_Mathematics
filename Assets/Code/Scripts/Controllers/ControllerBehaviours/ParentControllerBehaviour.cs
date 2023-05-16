using UnityEngine;
using Firebase.Auth;
using Project.Managers;
using Project.UI.Panel;
using Project.QuizSystem.SaveLoadQuestion;
using Project.UI.Panel.PanelItem;
using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using System.Collections.Generic;

public class ParentControllerBehaviour : MonoBehaviour{
    [SerializeField] private LoadingPanelView loadingView;

    #region Database/Server
    FirebaseUser CurrentUser => DatabaseManager.Auth.CurrentUser;
    private bool acceessGranted;
    private TestController TestController => DatabaseManager.Instance.TestController;

    [Header("AccessView")]
    [SerializeField] private OkCancelPanelView passwordFieldView;
    #endregion
    
    #region TestResults
    private List<SemesterTestSaveData> allTests;
    [Header("Detail Test Pager")]
    [SerializeField] private TestResultPagerController pagerController;
    [SerializeField] private PanelViewBase pagerUIView;
    
    [Header("Tests Overview")]
    [SerializeField] private GridPanelController testOverviews;
    [SerializeField] private GridPanelViewData testOverviewData;
    [SerializeField] private TestResultItemUI ItemUIPrefab;
    #endregion

    private void OnEnable(){
        pagerUIView.HideAsync();
        passwordFieldView.onConfirm += HandleAccess;
    }
    private async void Start(){
        if(!acceessGranted){
            //Require user type password
            await passwordFieldView.ShowAsync();
            return;
        }
        await LoadChildTest();
        SetUpOverviewTestResult();
        await testOverviews.Show();
    }

    private void HandleAccess(){
        
    }

    public async Task LoadChildTest(){
        allTests = new (2);

        var testSemester1 = await ResourceManager.Instance.LoadTestAsync(semester: 1);
        if(testSemester1 != null) allTests.Add(testSemester1);
        var testSemester2 = await ResourceManager.Instance.LoadTestAsync(semester: 2);
        if(testSemester2 != null) allTests.Add(testSemester2);
    }

    public void SetUpOverviewTestResult(){
        if(allTests.Count == 0) return;

        ButtonData[] buttons = new ButtonData[allTests.Count];
        for(int i = 0; i < allTests.Count; i++){
            int currentIndex = i;

            var clickEvent = new UnityEngine.UI.Button.ButtonClickedEvent();
            clickEvent.AddListener(()=>ViewTestDetail(currentIndex));
            buttons[i] = new ButtonData(){
                Name = "Xem chi tiết",
                OnClick = clickEvent, 
            };
        }
        testOverviewData.ButtonNames = buttons;

        testOverviews.OnBuildUI += OnBuildUIItem;
        testOverviews.SetUI(ItemUIPrefab,testOverviewData);
    }

    private void OnBuildUIItem(BasePanelItemUI itemUI, int index){
        if(index >= allTests.Count || itemUI is not TestResultItemUI testResultItem){
            return;
        }

        testResultItem.ToggleButtonClick(allTests[index] != null);
        if(allTests[index] != null) {
            testResultItem.SetTestResultData(allTests[index]);
        }
    }

    public async void ViewTestDetail(int index){
        // show Loading view
        await Task.WhenAll(loadingView.ShowAsync(),testOverviews.Hide());

        // Query from database
        (TestModel testModel, QuizModel[] quizzes) = await TestController.GetTest(allTests[index].Semester);
        UserManager.Instance.CurrentTestModel = testModel;
        UserManager.Instance.CurrentQuizzes = quizzes;

        //Setup pager
        pagerController.ManualSetSavedTest(allTests[index]);
        pagerController.ManualSetup();

        // hide Loading view
        await Task.WhenAll(loadingView.HideAsync(), pagerUIView.ShowAsync());
    }
}