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
using System.Collections;
using Project;

public class UserSignUpControllerBehaviour : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField rePasswordField;
    [SerializeField] Button BackToSignInButton;
    [SerializeField] Button SignUpButton;
    //[SerializeField] OkCancelPanelPusher pusher;
    [SerializeField] PanelType notificationViewType;
    [SerializeField] PanelType confirmLinkViewType;
    [SerializeField] PanelStackSystem stackSystem;

    private string username;
    private string password;
    //qhuyvo28072001@gmail.com
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
    private void OnChangingPasswordField(string password)
    {
        SignUpButton.interactable = IsValid();
    }

    private bool IsValid()
    {
        return username.IsEmail() && password != "" && password == passwordField.text;
    }

    Firebase.Auth.FirebaseUser user;
    private async void SignUp()
    {

        PopupDataBuilder autoBuilder = new();
        PopupData popupData = autoBuilder.StartCreating().AddText("Thông báo", "Đang đăng ký").GetResult();
        AutoClosePopupUI popupUI = new(notificationViewType, popupData);
        PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);

        AuthResult authResult;
        (user, authResult) = await Controller.RegisterAuth(username, password);

        popupUI.ManuallyClose();
        if (user == null || !authResult.IsSuccessful)
        {
            PopupDataWithButtonBuilder builder = new();
            PopupDataWithButton data = builder.StartCreating().AddText("Lỗi đăng ký", authResult.ErrorMessage).AddButtonData("Tôi đã hiểu", null).GetResult();
            GeneralPopupUI ui = new(notificationViewType, data);
            PopupUIQueueManager.Instance.EnqueueEventPopup(ui);
            return;
        }

        OkCancelPanelViewController controller = new(confirmLinkViewType, null);
        await stackSystem.PushAsync(controller);
        //pusher.Push();
        await user.SendEmailVerificationAsync();
        //isWaitingEmailVerify = true;

        StartCoroutine(WaitingForEmailVerification());
    }

    private IEnumerator WaitingForEmailVerification()
    {
        Task<bool> reloadTask = ReloadUser();
        bool result = false;

        while (result == false)
        {
            yield return new TaskAwaitInstruction(reloadTask);
            result = reloadTask.Result;
            reloadTask = ReloadUser();
        }
    }

    private async Task<bool> ReloadUser()
    {
        if (auth.CurrentUser == null)
        {
            return false;
        }
        Debug.Log("reloading");
        await auth.CurrentUser.ReloadAsync();
        Debug.Log("Done reloading");
        user = auth.CurrentUser;
        Debug.Log($"Verifying: {user.IsEmailVerified}");
        if (user.IsEmailVerified)
        {
            Controller.UploadModel(user);
            //
            BackToSignInButton.onClick?.Invoke();
            await stackSystem.PopAsync();
            Gameframe.GUI.Camera.UI.UIEventManager.Current?.Unlock();
            return true;
        }

        return false;
    }

    //private bool isWaitingEmailVerify = false;

    // async void OnApplicationPause(bool pauseStatus)
    // {
    //     if (isWaitingEmailVerify && !pauseStatus)
    //     {
    //         //Debug.Log("is Pausing " + pauseStatus);
    //         await ReloadUser();
    //         Controller.UploadModel(user);
    //         //
    //         BackToSignInButton.onClick?.Invoke();
    //         pusher.OnConfirm?.Invoke();
    //     }
    // }
}