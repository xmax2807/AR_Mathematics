using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.SaveLoad;
using Project.QuizSystem.SaveLoadQuestion;
using UnityEngine.Video;
using Project.Utils.ExtensionMethods;
using Project.UI.Panel;

namespace Project.Managers
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }
        private const string RewardPackPath = "Material UI/Reward/RewardPackSTO";
        private const string ARRewardPackPath = "Material UI/ModelReward/ARModelRewardPackSTO";
        private const string ErrorUIPath = "Material UI/ErrorUIs";
        public RewardSystem.RewardPackSTO RewardPack { get; private set; }
        public RewardSystem.ViewReward.ARModelRewardPackSTO ARModelRewardPack {get;private set;}
        public HistoryPanelDataSO HistorySO { get; private set; }
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
            //StartCoroutine(LoadEssentialResources());
        }
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if(scene.name == "LoginScene"){
                UnloadAssets();
            }
        }

        private void OnDestroy()
        {
            UnloadAssets();
        }

        private void UnloadAssets()
        {
            HistorySO?.Clear();
        }

        public IEnumerator LoadEssentialResources()
        {
            if (RewardPack == null)
            {
                ResourceRequest request = Resources.LoadAsync<RewardSystem.RewardPackSTO>(RewardPackPath);
                yield return request;
                RewardPack = request.asset as RewardSystem.RewardPackSTO;
            }

            if(ARModelRewardPack == null){
                ResourceRequest request = Resources.LoadAsync<RewardSystem.ViewReward.ARModelRewardPackSTO>(ARRewardPackPath);
                yield return request;
                ARModelRewardPack = request.asset as RewardSystem.ViewReward.ARModelRewardPackSTO;
            }

            if(HistorySO == null){
                ResourceRequest request = Resources.LoadAsync<HistoryPanelDataSO>("HistoryPanelData");
                yield return request;
                HistorySO = request.asset as HistoryPanelDataSO;
            }
        }

        public IEnumerator AskForAsset<T>(string filePath, System.Action<T> OnComplete) where T : Object{
            ResourceRequest request = Resources.LoadAsync<T>(filePath);
            yield return request;
            OnComplete?.Invoke(request.asset as T);
        }

        public async void SaveTestAsync(SemesterTestSaveData data, System.Action onResult = null){
            QuestionJsonIO jsonIO = new ();
            await jsonIO.SaveSemester(data);
            onResult?.Invoke();
        }
        public async Task<SemesterTestSaveData> LoadTestAsync(int semester){
            QuestionJsonIO jsonIO = new ();
            SemesterTestSaveData result;
            try{
                result = await jsonIO.LoadSemester(semester);
            }
            catch{
                result = null;
            }
            return result;
        }

        public IEnumerator GetLocalFile<T>(string relativePath, System.Action<T> OnResult) where T : Object{
            ResourceRequest req = Resources.LoadAsync<T>(relativePath);
            yield return req;
            req.asset.TryCastTo<T>(out T result);
            OnResult?.Invoke(result);
        }
    }
}