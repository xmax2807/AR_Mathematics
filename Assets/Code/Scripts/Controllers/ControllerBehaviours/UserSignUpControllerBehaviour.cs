using Project.Utils.ExtensionMethods;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Managers;
using Gameframe.GUI.PanelSystem;
using System;
using System.Threading.Tasks;

public class UserSignUpControllerBehaviour : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] TMP_InputField rePasswordField;
    [SerializeField] Button BackToSignInButton;
    [SerializeField] Button SignUpButton;
    [SerializeField] OkCancelPanelPusher pusher;

    private string username;
    private string password;
    private UserController Controller => DatabaseManager.Instance.UserController;
    private Firebase.Auth.FirebaseAuth auth => DatabaseManager.Auth;
    private void OnEnable()
    {
        SignUpButton.interactable = false;
        userNameField.onEndEdit.AddListener(EndedEditUsername);
        rePasswordField.onEndEdit.AddListener(EndedEditPassword);
        SignUpButton.onClick.AddListener(SignUp);
    }

    private void OnDisable()
    {
        userNameField.onEndEdit.RemoveListener(EndedEditUsername);
        rePasswordField.onEndEdit.RemoveListener(EndedEditPassword);
        SignUpButton.onClick.RemoveListener(SignUp);
    }

    private void EndedEditUsername(string text)
    {
        SignUpButton.interactable = text.IsEmail();
        username = text;
    }
    private void EndedEditPassword(string password)
    {
        SignUpButton.interactable = !(password.Length == 0 || password != passwordField.text);
        this.password = password;
    }

    Firebase.Auth.FirebaseUser user;
    private async void SignUp()
    {
        user = await Controller.RegisterAuth(username, password);
        // if (task.IsCanceled)
        // {
        //     Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
        //     return;
        // }
        // if (task.IsFaulted)
        // {
        //     Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
        //     return;
        // }
        // var user = task.Result;
        if (user == null) return;

        pusher.Push();
        await user.SendEmailVerificationAsync();
        isWaitingEmailVerify = true;


        // TimeCoroutineManager.Instance.DoLoopAction(
        // ReloadUser,
        // () => user.IsEmailVerified, // stop condition
        // 1,// interval
        // () => // post process
        // {
        //     //Up model len6 db
        //     Controller.UploadModel(user);
        //     //
        //     BackToSignInButton.onClick?.Invoke();
        //     pusher.OnConfirm?.Invoke();
        // });
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