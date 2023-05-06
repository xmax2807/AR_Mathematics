using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Managers
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }
        private const string RewardPackPath = "Material UI/Reward/RewardPackSTO";
        public RewardSystem.RewardPackSTO RewardPack { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            StartCoroutine(LoadEssentialResources());
        }
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadEssentialResources();
        }
        private IEnumerator LoadEssentialResources()
        {
            if (RewardPack == null)
            {
                ResourceRequest request = Resources.LoadAsync<RewardSystem.RewardPackSTO>(RewardPackPath);
                yield return request;
                RewardPack = request.asset as RewardSystem.RewardPackSTO;
            }

        }
    }
}