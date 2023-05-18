using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


using Firebase.Firestore;
using Project.Managers;
using System.Threading.Tasks;
using Firebase;
using RobinBird.FirebaseTools.Storage.Addressables;
using Firebase.Extensions;

public class DatabaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] QuizData quizData;
    // [SerializeField] string collection = "quizzes";
    // private string userID;
    // [SerializeField] private string Email;
    // [SerializeField] private string Password;
    [SerializeField] private int chapter;
    [SerializeField] private int unit;
    [SerializeField] private int semester = 1;


    // Firebase.FirebaseApp app;
    public static FirebaseFirestore FirebaseFireStore;
    public static Firebase.Auth.FirebaseAuth Auth;
    public static Firebase.Storage.FirebaseStorage Storage;
    public static DatabaseManager Instance { get; private set; }
    private FirebaseApp app;
    public UserController UserController { get; private set; }
    public AchievementController AchievementController { get; private set; }
    public LessonController LessonController { get; private set; }
    public GameController GameController { get; private set; }
    public QuizController QuizController { get; private set; }
    public TestController TestController { get; private set; }
    public bool IsConfigured {get;private set;}
    public event Action OnFirebaseConfigured;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitFirebase();
            IsConfigured = true;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    public void UploadData<T>(string collection, System.Func<T> builder)
    {
        T model = builder.Invoke();

        FirebaseFireStore.Collection(collection).Document().SetAsync(model);
    }
    private async void InitFirebase()
    {
        AddressableManager.AddDependencies();

        await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Connect successfully");
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                FirebaseAddressablesManager.IsFirebaseSetupFinished = true;
                
                FirebaseFireStore = FirebaseFirestore.GetInstance(app);
                Auth = Firebase.Auth.FirebaseAuth.GetAuth(app);
                Storage = Firebase.Storage.FirebaseStorage.GetInstance(app);
                OnFirebaseConfigured?.Invoke();
                Debug.Log("loaded Database");

                UserController = new UserController();
                AchievementController = new AchievementController();
                LessonController = new();
                GameController = new();
                QuizController = new();
                TestController = new();
                FirebaseFireStore.Collection("chapter_max_unit_count").Document("semester").GetSnapshotAsync().ContinueWithOnMainThread(
                    task => {
                        UserManager.Instance.CourseModel = task.Result.ConvertTo<CourseModel>();
                    }
                );
                // QuizController.GetQuizzesByLesson(unit,chapter);
                // TestController.GetTest(semester);
                // LessonController.UploadLesson(LessonData);
                //LessonController.GetVideo(1, 2);
                // CreateUser(Username, Password);

                // Debug.Log(LessonController.GetLessonID(1,1).Result);
            
                //bool result = await UserController.SignInAuth(Email,Password);
                // UserController.RegisterAuth(Email,Password);
                // Debug.Log(LessonController.GetLessonID(1,1).Result);

                // UploadData<QuizModel>(collection, ()=>new QuizModel(){
                //     QuizUnit = quizData.QuizUnit,
                //     QuizTitle = quizData.QuizTitle,
                //     QuizIMG = quizData.QuizIMG,
                //     QuizAnswer = quizData.QuizAnswer,
                //     QuizCorrectAnswer = quizData.QuizCorrectAnswer,
                //     QuizSemester = quizData.QuizSemester,
                //     QuizChapter = quizData.QuizChapter,
                // });
                // LessonController.GetVideo(1,2);
                // var user = await UserController.RegisterAuth("freefire@gmail.com","pubgmobile");
                // UserController.UploadModel(user);
                // try{GameController.SavingGameToUser("wqMomNaMtNeNvHbENiMxzzmcxh72","oHmusqZVcR9BKG5EMekh",1);
                // }
                // catch(Exception e){
                //     Debug.Log(e.Message);
                // }
                // try{QuizController.GetQuizzesByLesson(1,2);
                // }
                // catch(Exception e){
                //     Debug.Log(e.Message);
                // }
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }

            return Task.CompletedTask;
        });
    }
}
