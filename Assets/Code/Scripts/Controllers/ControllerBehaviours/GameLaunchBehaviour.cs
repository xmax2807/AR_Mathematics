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

public class GameLaunchBehaviour : UnityEngine.MonoBehaviour
{
    [SerializeField] private SingleSceneLoadBehaviour SceneLoadBehaviour;
    [SerializeField] Button startGameButton;
    [SerializeField] private PanelType loginPanelType;
    [SerializeField] private PanelType notificationWithConfirmPanelType;
    [SerializeField] PanelType autoNotificationPanel;
    [SerializeField] private PanelStackSystem stackSystem;
    private const string LoggedIn = "isLoggedIn";
    private const string SceneName = "MainMenuScene";

    private void Awake()
    {
        SceneLoadBehaviour.SceneName = SceneName;
        startGameButton.gameObject.SetActive(false);
    }

    private void Start(){
        startGameButton.gameObject.SetActive(true);
        startGameButton.onClick.AddListener(StartTheFlow);
    }

    private void StartTheFlow()
    {
        startGameButton.gameObject.SetActive(false);
        RequestPermissionManager.AskForPermission(AppPermission.Camera);
        TimeCoroutineManager.Instance.WaitUntil(()=>RequestPermissionManager.AskedPermission == true, CheckInternetConnection);
        //CheckInternetConnection();
    }

    private async void CheckInternetConnection()
    {
        bool result = await NetworkManager.Instance.CheckInternetConnectionAsync();
        Debug.Log("Checking Internet");

        if (result == false)
        {
            PopupDataWithButtonBuilder builder = new();
            PopupDataWithButton data = builder.StartCreating()
            .AddText("Mất kết nối", "Không thể kết nối tới mạng, vui lòng kiểm tra mạng thiết bị trước khi tiếp tục")
            .AddButtonData("Thử lại", CheckInternetConnection)
            .GetResult();

            GeneralPopupUI popupUI = new(notificationWithConfirmPanelType, data);
            PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);
            return;
        }
        else{
            InitDatabase();
        }
    }

    private async void InitDatabase(){
        bool result = await DatabaseManager.Instance.InitFirebase();
        if (result == false)
        {
            PopupDataWithButtonBuilder builder = new();
            PopupDataWithButton data = builder.StartCreating()
            .AddText("Lỗi khi tạo dữ liệu", "Đây có thể là lỗi do nhà phát triển gây nên, chúng tôi thành thật xin lỗi vì sự bất tiện này.")
            .AddButtonData("Thử lại", InitDatabase)
            .GetResult();

            GeneralPopupUI popupUI = new(notificationWithConfirmPanelType, data);
            PopupUIQueueManager.Instance.EnqueueEventPopup(popupUI);
        }
        else{
            StartCoroutine(ResourceManager.Instance.LoadEssentialResources());
            AddressableManager.Instance.InitAddressable();
            HandleLogin();
        }
    }

    private async void HandleLogin()
    {
        if(PlayerPrefs.GetInt(LoggedIn) == 1){
            string passphrase = SystemInfo.deviceUniqueIdentifier;
            string email = SecurePlayerPrefs.GetString("email", passphrase);
            string pass = SecurePlayerPrefs.GetString("password", passphrase);

            await AutoSignIn(email, pass);
        }
        else{
            ShowSignInPanel();
        }
    }

    private async Task AutoSignIn(string email, string password)
    {
        AutoClosePopupUI popupUI = PopAutoClose("Đang đăng nhập");
        UserController controller = DatabaseManager.Instance.UserController; 
        AuthResult result = await controller.SignInAuth(email, password);
        popupUI.ManuallyClose();
        if (result.IsSuccessful)
        {
            string message = GameManager.Instance.IsAdmin ? $"Đăng nhập admin thành công: {email}" : $"Đăng nhập thành công\n Xin chào {email}";
            popupUI = PopAutoClose(message);
            Debug.Log("Signed");
            TimeCoroutineManager.Instance.WaitForSeconds(1.5f, ()=>{
                popupUI.ManuallyClose();
                SceneLoadBehaviour.Load();
            });
        }
        else
        {
            PopError("Lỗi đăng nhập", result.ErrorMessage, ShowSignInPanel);
        }
    }

    private void ShowSignInPanel(){
        stackSystem.PushAsync(new PanelViewController(loginPanelType));
    }

    private void PopError(string title, string message, System.Action OnConfirm)
    {
        PopupDataWithButtonBuilder dataPopupBuilder = new();
        string rebuiltMessage = $"<size=80%>Đăng nhập không thành công do:</size>\n{message}";
        PopupDataWithButton data = dataPopupBuilder.StartCreating().AddText(title, rebuiltMessage).AddButtonData("Đăng nhập lại", OnConfirm).GetResult();
        GeneralPopupUI popupUI = new(notificationWithConfirmPanelType, data);
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