using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Gameframe.GUI.TransitionSystem;
using Gameframe.GUI.PanelSystem;
using System;
using Project.UI.Panel.Form;
using Project.Managers;

public class UserProfileControllerBehaviour : MonoBehaviour{
    private const string SceneName = "LoginScene";
    [SerializeField] SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] PanelStackSystem stackSystem;
    [SerializeField] Button SignOutButton;
    [SerializeField] Button DeleteAccountButton;
    [Header("Essential Panel UI")]
    [SerializeField] private PanelType Reconfirm;
    private TextFormPanelView passwordFormView;
    private PanelViewController<TextFormPanelView> panelViewController;
    
    #region User Account
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
        panelViewController = new(Reconfirm);
        await stackSystem.PushAsync(panelViewController);
        passwordFormView = panelViewController.View;
        passwordFormView.SetTitle(CurrentUser.Email);
        passwordFormView.onUserConfirmed += HandleAccess;
    }

    private async void HandleAccess(string[] data)
    {
        if (data.Length == 0) return;
        string password = data[0];

        accessGranted = await UserController.ReAuthenticateWithCredential(CurrentUser.Email,password);
        if(accessGranted == false){
            //Pop fail
            Debug.Log($"Failed to access: {CurrentUser.Email}");
            stackSystem.Pop();
            return;
        }
        else{

            //for now, return to login scene
            await UserController.DeleteCurrentUser();
            stackSystem.Pop();
            LoadScene(SceneName);
            //await stackSystem.PopAndPushAsync(1, new PanelViewController(Reconfirm));
        }
        
    }


  
    public  void SignOutAuth(){
        Controller.SignOutAuth();
        // return login scene
        LoadScene(SceneName);
    }

    private void LoadScene(string name){
        SceneLoadBehaviour.SceneName = name;
        SceneLoadBehaviour.Load();
    }
}