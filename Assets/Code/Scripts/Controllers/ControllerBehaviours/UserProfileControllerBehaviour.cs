using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Gameframe.GUI.TransitionSystem;

public class UserProfileControllerBehaviour : MonoBehaviour{
    
    [SerializeField] SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] Button SignOutButton;
    private UserController Controller => DatabaseManager.Instance.UserController;

    private void OnEnable(){
        SignOutButton.onClick.AddListener(SignOutAuth);
    }

    private void OnDisable(){
        SignOutButton.onClick.RemoveListener(SignOutAuth);
    }

  
    public  void SignOutAuth(){
        Controller.SignOutAuth();
        SceneLoadBehaviour.Load();
    }
}