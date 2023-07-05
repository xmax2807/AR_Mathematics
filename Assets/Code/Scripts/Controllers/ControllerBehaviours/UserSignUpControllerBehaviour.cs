using Project.Utils.ExtensionMethods;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Managers;
using Gameframe.GUI.PanelSystem;
using System;
using System.Threading.Tasks;
using Project.UI.Panel;
using Project.UI.Event.Popup;

public class UserSignUpControllerBehaviour : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField rePasswordField;
    [SerializeField] Button BackToSignInButton;
    [SerializeField] Button SignUpButton;
    [SerializeField] OkCancelPanelPusher pusher;
    [SerializeField] PanelType notificationViewType; 
    [SerializeField] PanelStackSystem stackSystem; 

    private string username;
    private string password;
    private UserController Controller => DatabaseManager.Instance.UserController;
    private Firebase.Auth.FirebaseAuth auth => DatabaseManager.Auth;
    private void OnEnable()
    {
        SignUpButton.interactable = false;
        userNameField.onValueChanged.AddListener(EndedEditUsername);
        rePasswordField.onValueChanged.AddListener(EndedEditPassword);
        passwordField.onValueChanged.AddListener(OnChangingPasswordField);
        SignUpButton.onClick.AddListener(SignUp);
    }

    private void OnDisable()
    {
        userNameField.onValueChanged.RemoveListener(EndedEditUsername);
        rePasswordField.onValueChanged.RemoveListener(EndedEditPassword);
        passwordField.onValueChanged.RemoveListener(OnChangingPasswordField);
        SignUpButton.onClick.RemoveListener(SignUp);
    }

    private void EndedEditUsername(string text)
    {
        username = text;
        SignUpButton.interactable = IsValid();
    }
    private void EndedEditPassword(string password)
    {
        this.password = password;
        SignUpButton.interactable = IsValid();
    }
    private void OnChangingPasswordField(string password){
        SignUpButton.interactable = IsValid();
    }

    private bool IsValid(){
       return username.IsEmail() && password != "" && password == passwordField.text;
    }

    Firebase.Auth.FirebaseUser user;
    private async void SignUp()
    {
        AuthResult authResult;
        (user, authResult) = await Controller.RegisterAuth(username, password);
        
        if (user == null || !authResult.IsSuccessful) {
            PopupDataWithButtonBuilder builder = new();
            PopupDataWithButton data = builder.StartCreating().AddText("Lỗi đăng ký", authResult.ErrorMessage).AddButtonData("Tôi đã hiểu", null).GetResult();
            GeneralPopupUI ui = new(notificationViewType, data);
            PopupUIQueueManager.Instance.EnqueueEventPopup(ui);
            return;
        }

        pusher.Push();
        await user.SendEmailVerificationAsync();
        isWaitingEmailVerify = true;
    }

    private async Task<bool> ReloadUser()
    {
        if(auth.CurrentUser == null){
            return false;
        }
        Debug.Log("reloading");
        await auth.CurrentUser.ReloadAsync();
        Debug.Log("Done reloading");
        user = auth.CurrentUser;
        Debug.Log($"Verifying: {user.IsEmailVerified}");
        if(user.IsEmailVerified){
            Controller.UploadModel(user);
            //
            BackToSignInButton.onClick?.Invoke();
            pusher.OnConfirm?.Invoke();
            return true;
        }

        return false;
    }

    private bool isWaitingEmailVerify = false;

    async void OnApplicationPause(bool pauseStatus)
    {
        if (isWaitingEmailVerify && !pauseStatus)
        {
            //Debug.Log("is Pausing " + pauseStatus);
            await ReloadUser();
            Controller.UploadModel(user);
            //
            BackToSignInButton.onClick?.Invoke();
            pusher.OnConfirm?.Invoke();
        }
    }
}