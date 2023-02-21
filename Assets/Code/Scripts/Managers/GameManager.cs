using System.Collections;
using Project.Utils.ExtensionMethods;
using UnityEngine;

namespace Project.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// Initialize GameManager with required Managers
        /// </summary>
        private void Init(){
            Instance = this;
            gameObject.EnsureChildComponent<AudioManager>(childName:"Audio Manager");
            gameObject.EnsureChildComponent<TimeCoroutineManager>(childName: "Time Manager");
            gameObject.EnsureChildComponent<NetworkManager>(childName: "Network Manager");
        }
        private void Awake()
        {
            if (Instance == null)
            {
                Init();
                DontDestroyOnLoad(this.gameObject);
            }
            if (Instance != this)
            {
                Destroy(this);
            }
        }
    }
}
