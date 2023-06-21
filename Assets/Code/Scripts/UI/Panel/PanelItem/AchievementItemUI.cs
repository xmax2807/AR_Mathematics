using Project.UI.Panel;
using Gameframe.GUI.PanelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel.PanelItem
{
    public class AchievementItemUI : BasePanelItemUI<UnlockableImageButtonData>
    {
        [SerializeField] private Image presenter;
        [SerializeField] protected Button button;
        [SerializeField] private PanelType panelType;
        [SerializeField] private PanelStackSystem modalStack;

        private UnlockableImageButtonData uiData;
        private RewardSystem.RewardPopup rewardPopup;
        private RewardSystem.RewardPopup rewardPopupObj;
        private PanelViewController popupPanel;

        private void Awake()
        {
            if (button == null)
            {
                button = GetComponentInChildren<Button>();
                if (button == null)
                {
                    Debug.LogError($"Button is not presented in this {this.name}");
                }
            }

            GetRewardPopup();
        }

        public override void SetUI(UnlockableImageButtonData data)
        {
            this.uiData = data;
            presenter.sprite = uiData.GetImage();

            if (rewardPopup == null)
            {
                GetRewardPopup();
            }

            if (button == null) return;
            if (!uiData.isUnlocked)
            {
                button.interactable = false;
            }
            else
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(Push);
            }
        }
        private void Push()
        {
            rewardPopup?.SetRewardData(uiData.Name, uiData.GetImage());
            popupPanel ??= new PanelViewController(panelType);
            if (popupPanel != null)
            {
                modalStack.Push(popupPanel);
                rewardPopupObj = modalStack.CurrentTopPanel.View.GetComponent<RewardSystem.RewardPopup>();
                if(rewardPopupObj != null){
                    rewardPopupObj.OnClose += OnPopupClosed;
                } 
            }
        }

        public override void SetDefaultUI(ButtonData data)
        {
            // Do nothing for now
        }

        private void GetRewardPopup()
        {
            if (panelType == null)
            {
                return;
            }

            bool hasRewardPopup = panelType.Prefab.gameObject.TryGetComponent<RewardSystem.RewardPopup>(out rewardPopup);
            if (!hasRewardPopup && button != null)
            {
                button.enabled = false;
            }
        }

        private void OnPopupClosed(){
            modalStack.Pop();
        }
    }
}