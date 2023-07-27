using System.Collections;
using System.Threading.Tasks;
using Project.Utils.ExtensionMethods;
using UnityEngine;
using Project.Utils;
using UnityEngine.SceneManagement;
using System;

namespace Project.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static Camera MainGameCam => Instance?.mainGameCam;
        public static Canvas RootCanvas => Instance?.rootCanvas;
        private Camera mainGameCam;
        private Canvas rootCanvas;
        private Scene currentScene;

        [SerializeField] bool showCamValues = false;

        /// <summary>
        /// Initialize GameManager with required Managers
        /// </summary>
        private void Init(){
            Instance = this;
            gameObject.EnsureChildComponent<ResourceManager>(childName: "Resource Manager");
            gameObject.EnsureChildComponent<TimeCoroutineManager>(childName: "Time Manager");
            gameObject.EnsureChildComponent<NetworkManager>(childName: "Network Manager");
            gameObject.EnsureChildComponent<SpawnerManager>(childName: "Spawn Manager");
            gameObject.EnsureChildComponent<PopupUIQueueManager>(childName: "Popup UI Queue Manager");
            gameObject.EnsureChildComponent<AudioManager>(childName:"Audio Manager");
            gameObject.EnsureChildComponent<DatabaseManager>(childName: "Database Manager");
            gameObject.EnsureChildComponent<UserManager>(childName: "User Manager");
            gameObject.EnsureChildComponent<AddressableManager>(childName: "Addressable Manager");

            // OnSceneLoaded
            SceneManager.sceneLoaded += OnSceneLoaded;
            mainGameCam = Camera.main;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene;
            Instance.mainGameCam = Camera.main;
            Instance.rootCanvas = GameObject.FindGameObjectWithTag("MainCanvas")?.GetComponent<Canvas>();
            if(showCamValues){
                this.EnsureComponent<CameraValueLogger>(autoCreate: true);
            }
        }

        protected void Awake()
        {
            if (Instance == null)
            {
                Init();
            }
            else if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
            Application.targetFrameRate = 30;
            //Screen.SetResolution(1280, 720, fullscreen: true);
        }

        protected void OnDisable(){
            Instance.mainGameCam = null;
        }
        protected void OnEnable(){
            Instance.mainGameCam = Camera.main;
            Instance.rootCanvas = GameObject.FindGameObjectWithTag("MainCanvas")?.GetComponent<Canvas>();
            if(showCamValues){
                this.EnsureComponent<CameraValueLogger>(autoCreate: true);
            }
        }

        public System.Action OnGameFinishLoading;

        public bool IsAdmin {get; private set;}
        private void SwitchToDebuggingMode(bool isAdmin = false){
            IsAdmin = isAdmin;
            DebugLog logger = this.EnsureComponent<DebugLog>(autoCreate: true);
            logger.enabled = isAdmin;
        }

        internal Task CheckIsAdminLoggedIn(string userId)
        {
            bool isCheckCompleted = false;
            StartCoroutine(ResourceManager.Instance.AskForAsset<TextAsset>("admin_ids", 
            (asset)=>{
                GotAdminFile(asset, userId);
                isCheckCompleted = true;
            }));
            while(!isCheckCompleted){
                return Task.Delay(100);
            }
            return Task.CompletedTask;
        }

        private void GotAdminFile(TextAsset text, string userId){
            string[] adminIds = text.text.Split(',');
            foreach(string id in adminIds){
                if(id == userId){
                    SwitchToDebuggingMode(true);
                    return;
                }
            }

            SwitchToDebuggingMode(false);
        }

        public void ReLoadCurrentScene(){
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
