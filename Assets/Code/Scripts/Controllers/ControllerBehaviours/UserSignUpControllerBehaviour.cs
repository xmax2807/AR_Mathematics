using Project.Utils.ExtensionMethods;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Managers;
using Gameframe.GUI.PanelSystem;

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

    private async void SignUp()
    {
        var user = await Controller.RegisterAuth(username, password);
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

        TimeCoroutineManager.Instance.DoLoopAction(
        () =>
        {
            user.ReloadAsync();
            Debug.Log(user.IsEmailVerified);
        },
        () => user.IsEmailVerified,
        3,
        () =>
        {
            BackToSignInButton.onClick?.Invoke();
            pusher.OnConfirm?.Invoke();
        });
    }
}