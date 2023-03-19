using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserControllerBehaviour : MonoBehaviour{
    
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    private string username;
    private string password;
    private UserController controller;

    private void OnEnable(){
        userNameField.onEndEdit.AddListener(EndedEditUsername);
        passwordField.onEndEdit.AddListener(EndedEditPassword);
    }

    private void OnDisable(){
        userNameField.onEndEdit.RemoveListener(EndedEditUsername);
        passwordField.onEndEdit.RemoveListener(EndedEditPassword);
    }

    private void EndedEditUsername(string text){
        
        // if(controller.CheckifUsernameExist(text)){
        //     Debug.Log("Existed");
        // }
        // username = text;
    }
    private void EndedEditPassword(string password){
        this.password = password;
    }
    private void Start(){
        controller = new();
    }

    public async void SignIn(){
        // bool result = await controller.SignIn(username, password);

        // if(result){
        //     Debug.Log("Signed");
        // }

        
    }
}