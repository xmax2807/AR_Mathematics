using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Gameframe.GUI.TransitionSystem;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.PanelSystem;
using Project.UI.Panel;

public class UserSignInControllerBehaviour : MonoBehaviour{
    
    [SerializeField] SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] Button SignInButton;
    [SerializeField] PanelType notificationPanel;
    [SerializeField] PanelStackSystem stackSystem;
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
        UIEventManager.Current.Lock();
        AuthResult result = await Controller.SignInAuth(username, password);
        UIEventManager.Current.Unlock();
        
        if(result.IsSuccessful){
            Debug.Log("Signed");
            GetComponent<PanelPopper>().Pop();
            SceneLoadBehaviour.Load();
        }
        else{
            PopError(result.ErrorMessage);
        }
    }

    private async void PopError(string message){
        OkCancelPanelViewController controller = new(notificationPanel, (isOk)=>{
            stackSystem.Pop();
        });
        //await controller.LoadViewAsync();
        await stackSystem.PushAsync(controller);
        var view =  (NotificationPanelView)controller.View;

        string rebuiltMessage = $"<size=80%>Đăng nhập không thành công do:</size>\n{message}";
        view.SetUI(rebuiltMessage, "Lỗi đăng nhập");
        Debug.Log("Pushed error");
    }
}