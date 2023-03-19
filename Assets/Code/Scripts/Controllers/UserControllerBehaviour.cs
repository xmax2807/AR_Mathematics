using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Gameframe.GUI.TransitionSystem;

public class UserControllerBehaviour : MonoBehaviour{
    
    [SerializeField] SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] Button SignInButton;
    private string username;
    private string password;
    private UserController controller;

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

    private async void EndedEditUsername(string text){
        bool result = await controller.CheckifUsernameExist(text);
        if(result){
            Debug.Log("Existed");
        }
        SignInButton.interactable = !result;
        username = text;
    }
    private void EndedEditPassword(string password){
        this.password = password;
    }
    private void Start(){
        controller = new();
        SignInButton.interactable = false;
    }
  
    public async void SignIn(){
        bool result = await controller.SignInAuth(username, password);

        if(result){
            Debug.Log("Signed");
            SceneLoadBehaviour.Load();
        }
    }
}