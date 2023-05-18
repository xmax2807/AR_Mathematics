using System.Collections;
using System.Threading.Tasks;
using Project.Utils.ExtensionMethods;
using UnityEngine;

namespace Project.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static Camera MainGameCam => Instance?.mainGameCam;
        private Camera mainGameCam;

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
        }

        public System.Func<Task> OnGameFinishLoading;
    }
}
