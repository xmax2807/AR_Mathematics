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

        [SerializeField] bool showCamValues = false;

        /// <summary>
        /// Initialize GameManager with required Managers
        /// </summary>
        private void Init(){
            Instance = this;
            gameObject.EnsureChildComponent<SpawnerManager>(childName: "Spawn Manager");
            gameObject.EnsureChildComponent<AudioManager>(childName:"Audio Manager");
            gameObject.EnsureChildComponent<TimeCoroutineManager>(childName: "Time Manager");
            gameObject.EnsureChildComponent<DatabaseManager>(childName: "Database Manager");
            gameObject.EnsureChildComponent<UserManager>(childName: "User Manager");
            gameObject.EnsureChildComponent<NetworkManager>(childName: "Network Manager");
            gameObject.EnsureChildComponent<AddressableManager>(childName: "Addressable Manager");
            gameObject.EnsureChildComponent<ResourceManager>(childName: "Resource Manager");

            // OnSceneLoaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
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
            DontDestroyOnLoad(this);
            Application.targetFrameRate = 30;
            Screen.SetResolution(1280, 720, fullscreen: true);
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
    }
}
