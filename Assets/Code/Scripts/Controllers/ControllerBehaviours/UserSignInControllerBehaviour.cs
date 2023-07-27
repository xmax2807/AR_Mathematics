using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Gameframe.GUI.TransitionSystem;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.PanelSystem;
using Project.UI.Panel;
using Project.UI.Event.Popup;
using Project.Utils;
using Project.Managers;

public class UserSignInControllerBehaviour : MonoBehaviour
{
    [SerializeField] private SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] Button SignInButton;
    [SerializeField] PanelType notificationPanel;
    [SerializeField] PanelType autoNotificationPanel;
    [SerializeField] Toggle rememberCheck;

    private const string LoggedIn = "isLoggedIn";
    private string username;
    private string password;
    private UserController Controller => DatabaseManager.Instance.UserController;

    private void OnEnable()
    {
        userNameField.onEndEdit.AddListener(EndedEditUsername);
        passwordField.onEndEdit.AddListener(EndedEditPassword);
        SignInButton.onClick.AddListener(SignIn);
    }

    private void OnDisable()
    {
        userNameField.onEndEdit.RemoveListener(EndedEditUsername);
        passwordField.onEndEdit.RemoveListener(EndedEditPassword);
        SignInButton.onClick.RemoveListener(SignIn);
    }

    private void EndedEditUsername(string text)
    {
        SignInButton.interactable = text.IsEmail();
        username = text;
    }
    private void EndedEditPassword(string password)
    {
        this.password = password;
    }
    private void Start()
    {
        SignInButton.interactable = false;
    }

    public async void SignIn()
    {
        UIEventManager.Current.Lock();
        AutoClosePopupUI popupUI = PopAutoClose("Đang đăng nhập");

        AuthResult result = await Controller.SignInAuth(username, password);

        popupUI.ManuallyClose();
        UIEventManager.Current.Unlock();

        if (result.IsSuccessful)
        {
            Debug.Log("Signed");
            GetComponent<PanelPopper>().Pop();

            HandleRememberCheck();

            string message = GameManager.Instance.IsAdmin ? 
                            $"Đăng nhập admin thành công: {username}" : 
                            $"Đăng nhập thành công\n Xin chào {username}";

            popupUI = PopAutoClose(message);
            Debug.Log("Signed");
            TimeCoroutineManager.Instance.WaitForSeconds(1.5f, ()=>{
                popupUI.ManuallyClose();
                SceneLoadBehaviour.Load();
            });
        }
        else
        {
            PopError("Lỗi đăng nhập", result.ErrorMessage);
        }
    }

    public void HandleRememberCheck()
    {
        if(Project.Managers.GameManager.Instance.IsAdmin){
            PlayerPrefs.SetInt(LoggedIn, 0);
            PlayerPrefs.Save();
            return;
        }
        
        if (rememberCheck != null && rememberCheck.isOn)
        {
            PlayerPrefs.SetInt(LoggedIn, 1);
            RememberAccount();
        }
        else
        {
            PlayerPrefs.SetInt(LoggedIn, 0);
        }
        PlayerPrefs.Save();
    }

    private void RememberAccount()
    {
        string passphrase = SystemInfo.deviceUniqueIdentifier;
        SecurePlayerPrefs.SetString("email", username, passphrase);
        SecurePlayerPrefs.SetString("password", password, passphrase);
    }

    private void PopError(string title, string message)
    {
        PopupDataWithButtonBuilder dataPopupBuilder = new();
        string rebuiltMessage = $"<size=80%>Đăng nhập không thành công do:</size>\n{message}";
        PopupDataWithButton data = dataPopupBuilder.StartCreating().AddText(title, rebuiltMessage).AddButtonData("Tôi đã hiểu", null).GetResult();
        GeneralPopupUI popupUI = new(notificationPanel, data);
        Project.Managers.PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);
    }

    private AutoClosePopupUI PopAutoClose(string message, string title = "Thông báo")
    {
        PopupDataBuilder dataPopupBuilder = new();
        PopupData data = dataPopupBuilder.StartCreating().AddText(title, message).GetResult();
        AutoClosePopupUI popupUI = new(autoNotificationPanel, data);
        Project.Managers.PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);
        return popupUI;
    }
}