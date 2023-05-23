using Project.UI.Panel;
using Gameframe.GUI.PanelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel.PanelItem{
    public class AchievementItemUI : BasePanelItemUI<UnlockableImageButtonData>
    {
        [SerializeField] private Image presenter;
        [SerializeField] protected Button button;
        [SerializeField] private PanelType panelType;
        [SerializeField] private PanelStackSystem modalStack;
        
        private UnlockableImageButtonData uiData;
        private RewardSystem.RewardPopup rewardPopup;

        private void Awake(){
            if(button == null){
                button = GetComponentInChildren<Button>();
                if(button == null){
                    Debug.LogError($"Button is not presented in this {this.name}");
                }
            }

            GetRewardPopup();
        }

        public override void SetUI(UnlockableImageButtonData data){
            this.uiData = data;
            presenter.sprite = uiData.GetImage();
            
            if(rewardPopup == null){
                GetRewardPopup();
            }

            if(button == null) return;
            if(!uiData.isUnlocked){
                button.enabled = false;
            }
            else{
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(Push);
            }
        }
        private void Push(){
            rewardPopup?.SetRewardData(uiData.Name, uiData.GetImage());
            modalStack.Push(new PanelViewController(panelType));
        }

        public override void SetDefaultUI(ButtonData data)
        {
            // Do nothing for now
        }

        private void GetRewardPopup(){
            if(panelType != null){
                bool hasRewardPopup = panelType.Prefab.gameObject.TryGetComponent<RewardSystem.RewardPopup>(out rewardPopup); 
                if(!hasRewardPopup && button != null){
                    button.enabled = false;
                }
            }
        }
    }
}