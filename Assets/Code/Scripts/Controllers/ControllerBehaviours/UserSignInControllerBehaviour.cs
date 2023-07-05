using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Gameframe.GUI.TransitionSystem;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.PanelSystem;
using Project.UI.Panel;
using Project.UI.Event.Popup;

public class UserSignInControllerBehaviour : MonoBehaviour{
    [SerializeField] private SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] TMP_InputField userNameField;
    [SerializeField] TMP_InputField passwordField;
    [SerializeField] Button SignInButton;
    [SerializeField] PanelType notificationPanel;
    [SerializeField] PanelType autoNotificationPanel;
    [SerializeField] PanelStackSystem stackSystem;
    private PopupDataWithButtonBuilder dataPopupBuilder;
    private string username;
    private string password;
    private UserController Controller => DatabaseManager.Instance.UserController;

    private void Awake(){
        dataPopupBuilder = new();
    }

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
        AutoClosePopupUI popupUI = PopAutoClose("Đang đăng nhập");
        
        AuthResult result = await Controller.SignInAuth(username, password);
        
        popupUI.ManuallyClose();
        UIEventManager.Current.Unlock();
        
        if(result.IsSuccessful){
            Debug.Log("Signed");
            GetComponent<PanelPopper>().Pop();
            SceneLoadBehaviour.Load();
        }
        else{
            PopError("Lỗi đăng nhập",result.ErrorMessage);
        }
    }

    private void PopError(string title,string message){
        
        string rebuiltMessage = $"<size=80%>Đăng nhập không thành công do:</size>\n{message}";
        PopupDataWithButton data = dataPopupBuilder.StartCreating().AddText(title, rebuiltMessage).AddButtonData("Tôi đã hiểu", null).GetResult();
        GeneralPopupUI popupUI = new(notificationPanel,data);
        Project.Managers.PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);
    }

    private AutoClosePopupUI PopAutoClose(string message){
        PopupData data = dataPopupBuilder.StartCreating().AddText("", message).GetResult();
        AutoClosePopupUI popupUI = new(autoNotificationPanel,data);
        Project.Managers.PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);
        return popupUI;
    }
}