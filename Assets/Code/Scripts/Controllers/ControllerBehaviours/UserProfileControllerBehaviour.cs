using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Gameframe.GUI.TransitionSystem;
using Gameframe.GUI.PanelSystem;
using System;
using Project.UI.Panel.Form;
using Project.Managers;
using Project.UI.Panel;
using Project.UI.Event.Popup;

public class UserProfileControllerBehaviour : MonoBehaviour{
    private const string SceneName = "LoginScene";
    [SerializeField] SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] PanelType notificationPanelViewType;
    [SerializeField] PanelStackSystem stackSystem;
    [SerializeField] Button SignOutButton;
    [SerializeField] Button DeleteAccountButton;
    [Header("Essential Panel UI")]
    [SerializeField] private PanelType Reconfirm;
    private TextFormPanelView passwordFormView;
    private PanelViewController<TextFormPanelView> panelViewController;
    private OkCancelPanelViewController notificationController;
    
    #region User Account
    private const string LoggedIn = "isLoggedIn";
    private bool accessGranted;
    private Firebase.Auth.FirebaseUser CurrentUser => DatabaseManager.Auth.CurrentUser;
    private UserController UserController => DatabaseManager.Instance.UserController;
    #endregion
    private UserController Controller => DatabaseManager.Instance.UserController;

    private void OnEnable(){
        SignOutButton.onClick.AddListener(SignOutAuth);
        DeleteAccountButton.onClick.AddListener(DeleteAccount);
    }
    private void OnDisable(){
        SignOutButton.onClick.RemoveListener(SignOutAuth);
        DeleteAccountButton.onClick.RemoveListener(DeleteAccount);
    }

    private async void DeleteAccount()
    {
        panelViewController ??= new(Reconfirm);
        await stackSystem.PushAsync(panelViewController);
        passwordFormView = panelViewController.View;
        passwordFormView.SetTitle($"Email: {CurrentUser.Email}");
        passwordFormView.onUserConfirmed += HandleAccess;
        passwordFormView.onCancel += stackSystem.Pop;
    }

    private async void HandleAccess(string[] data)
    {
        if (data.Length == 0) return;
        string password = data[0];

        accessGranted = await UserController.ReAuthenticateWithCredential(CurrentUser.Email,password);
        if(accessGranted == false){
            //Pop fail
            Debug.Log($"Failed to access: {CurrentUser.Email}");
            
            if(notificationController != null && notificationController.IsViewLoaded){
                Destroy(notificationController.View.gameObject);
            }

            notificationController = new(notificationPanelViewType, (isOk)=>{
                stackSystem.Pop();
            });

            await stackSystem.PushAsync(notificationController);
            NotificationPanelView view = (NotificationPanelView)notificationController.View;
            view.SetUI("Mật khẩu bạn nhập không đúng", "Sai mật khẩu");
            return;
        }
        else{

            //for now, return to login scene
            await UserController.DeleteCurrentUser();
            //stackSystem.Pop();
            AutoClosePopupUI popupUI = PopAutoClose("Xóa tài khoản thành công");
            TimeCoroutineManager.Instance.WaitForSeconds(1.5f, ()=>{
                popupUI.ManuallyClose();
                LoadScene(SceneName);
            });
            // if(notificationController != null && notificationController.IsViewLoaded){
            //     Destroy(notificationController.View.gameObject);
            // }
            // notificationController = new(notificationPanelViewType, (isOk)=>{
            //     stackSystem.Pop();    
            //     LoadScene(SceneName);
            // });
            // await stackSystem.PushAsync(notificationController);
            // NotificationPanelView view = (NotificationPanelView)notificationController.View;
            // view.SetUI("Xóa tài khoản thành công", "Thông báo");
            // //await stackSystem.PopAndPushAsync(1, new PanelViewController(Reconfirm));
        }
        
    }

    private AutoClosePopupUI PopAutoClose(string message, string title = "Thông báo")
    {
        PopupDataBuilder dataPopupBuilder = new();
        PopupData data = dataPopupBuilder.StartCreating().AddText(title, message).GetResult();
        AutoClosePopupUI popupUI = new(notificationPanelViewType, data);
        Project.Managers.PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);
        return popupUI;
    }


  
    public  void SignOutAuth(){
        Controller.SignOutAuth();
        if(PlayerPrefs.HasKey(LoggedIn)){
            PlayerPrefs.SetInt(LoggedIn, 0);
        }
        // return login scene
        LoadScene(SceneName);
    }

    private void LoadScene(string name){
        SceneLoadBehaviour.SceneName = name;
        SceneLoadBehaviour.Load();
    }
}