using Project.Utils.ExtensionMethods;
using System.Threading.Tasks;
using Project.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel
{
    [CreateAssetMenu(fileName = "RequestViewPanelData", menuName = "STO/PanelViewData/RequestViewPanelData")]
    public class RequestPanelViewData : PanelViewData
    {
        [SerializeField] private PanelViewData realData;
        [SerializeField] private BasePanelController wrappedController;
        public BasePanelController WrappedController => wrappedController;
        public PanelViewData RealPanelViewData => realData;
        [SerializeField] private string stringData;
        [SerializeField] private SingleRequestData.RequestType requestType;
        [SerializeField] private SingleRequestData requestData;
        [SerializeField] private SingleSceneLoadData sceneLoadData;
        public override PanelEnumType Type => PanelEnumType.Request;

        public bool CheckType(){
            return realData.Type == wrappedController.Type;
        }
        public async Task<bool> RequestData()
        {
            bool result = await requestData.Request(requestType, stringData);
            
            if(result == true){
                
                PopulateButton();
            }
            
            return result;
        }
        private void PopulateButton()
        {

            switch (requestType)
            {
                case SingleRequestData.RequestType.Lesson:
                    
                    break;
                case SingleRequestData.RequestType.Quiz:
                    break;
                case SingleRequestData.RequestType.Game:
                    PopulateGameButtons();
                    break;
                case SingleRequestData.RequestType.Achievement:
                    PopulateAchievementButton();
                    break;
            }
        }

        private void PopulateGameButtons(){
            GameModel[] models = UserManager.Instance.CurrentGameScenes;
            realData.ButtonNames = new ButtonData[models.Length];
            for(int i = 0; i < models.Length; i++){
                int currentIndex = i;
                Button.ButtonClickedEvent buttonClicked = new();
                buttonClicked.AddListener(()=>{
                    UserManager.Instance.CurrentGame = models[currentIndex];
                    sceneLoadData.LoadScene(models[currentIndex].GameScene);
                });
                realData.ButtonNames[currentIndex] = new ButtonData(){
                    Name = models[currentIndex].GameTitle,
                    OnClick = buttonClicked
                };
            }
        }

        private void PopulateAchievementButton(){
            AchievementModel[] accquiredAchivements = new AchievementModel[0];
            var currentUser = UserManager.Instance.CurrentUser;
            if(currentUser != null){
                accquiredAchivements = UserManager.Instance.AcquiredAchivements;
            }

            RewardSystem.RewardPackSTO pack = ResourceManager.Instance.RewardPack;
            string[] packKeys = pack.Keys;
            realData.ButtonNames = new UnlockableImageButtonData[pack.PackCount];
            for(int i = 0; i < pack.PackCount; i++){
                RewardSystem.RewardBadgeSTO badge =  pack.GetReward(packKeys[i]);
                UnlockableImageButtonData data = new(){
                    Name = badge.Title,
                    Image = badge.Visual,
                    isUnlocked = accquiredAchivements.IsContains((x)=>x.AchieveTitle == packKeys[i]),
                    LockedImage = pack.defaultSprite,
                };
                realData.ButtonNames[i] = data;
            }
        }

    }
}