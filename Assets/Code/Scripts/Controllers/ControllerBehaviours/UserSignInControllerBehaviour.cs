using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Gameframe.GUI.TransitionSystem;

public class UserSignInControllerBehaviour : MonoBehaviour{
    
    [SerializeField] SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] Button SignInButton;
    private string username;
    private string password;
    private UserController Controller => DatabaseManager.Instance.UserController;

    private void OnEnable(){
        userNameField.onEndEdit.AddListener(EndedEditUsername);
        passwordField.onEndEdit.AddListener(EndedEditPassword);
        SignInButton.onClick.AddListener(SignIn);
    }

    private void OnDisable(){
        userNameField.onEndEdit.RemoveListener(EndedEditUsername);
        passwordField.onEndEdit.RemoveListener(EndedEditPassword);
        SignInButton.onClick.RemoveListener(SignIn);
    }

    private void EndedEditUsername(string text){
        SignInButton.interactable = text.IsEmail();
        username = text;
    }
    private void EndedEditPassword(string password){
        this.password = password;
    }
    private void Start(){
        SignInButton.interactable = false;
    }
  
    public async void SignIn(){
        bool result = await Controller.SignInAuth(username, password);

        if(result){
            Debug.Log("Signed");
            SceneLoadBehaviour.Load();
        }
    }
}