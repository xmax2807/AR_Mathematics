using UnityEngine;
using UnityEngine.UI;
using Project.Managers;
using System.Threading.Tasks;
using Project.UI.Event.Popup;
using Gameframe.GUI.PanelSystem;
using System;
using Project.Utils;
using Gameframe.GUI.TransitionSystem;
using Project.AppPermission;

public class AutoLoginDebugger : UnityEngine.MonoBehaviour
{
    private const string email ="xmax28072001@gmail.com";
    private const string password ="quanghuy2807";
    private async void Start(){
        bool result = await NetworkManager.Instance.CheckInternetConnectionAsync();
        if(result == false){
            Debug.Log("Failed to connect internet");
            return;
        }
        result = await DatabaseManager.Instance.InitFirebase();
        if(result == false){
            Debug.Log("Failed to init firebase");
            return;
        }

        UserController controller = DatabaseManager.Instance.UserController; 
        AuthResult authResult = await controller.SignInAuth(email, password);
        if (authResult.IsSuccessful)
        {
            Debug.Log("Login successful");
            AddressableManager.Instance.InitAddressable();
        }
    }

}