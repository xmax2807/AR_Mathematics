using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


using Firebase.Firestore;
using Project.Utils.ExtensionMethods;
using System.Threading.Tasks;
using Firebase;

public class DatabaseManager : MonoBehaviour
{
    // Start is called before the first frame update

    // private string userID;
    // [SerializeField] private string Username;
    // [SerializeField] private string Password;
    // Firebase.FirebaseApp app;
    public static FirebaseFirestore FirebaseFireStore;
    public static Firebase.Auth.FirebaseAuth Auth;
    public static DatabaseManager Instance {get;private set;}
    private FirebaseApp app;
    public UserController UserController {get;private set;}

    void Awake()
    {
        if(Instance == null){
            Instance = this;
            InitFirebase();
        }
        else if(Instance != this){
            Destroy(this.gameObject);
        }
    }

    private async void InitFirebase()
    {
        FirebaseFireStore = FirebaseFirestore.DefaultInstance;
        Auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        UserController = new UserController();
        
        await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Connect successfully");
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                // CreateUser(Username, Password);

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
