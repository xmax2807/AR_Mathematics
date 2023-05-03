using UnityEngine;
using UnityEngine.UI;
using Gameframe.GUI.PanelSystem;

namespace Project.RewardSystem{
    public class RewardPopup : MonoBehaviour{
        [SerializeField] private Image rewardImg;
        [SerializeField] private TMPro.TextMeshProUGUI rewardName;
        private OkCancelPanelView view;
        private void Awake(){
            view = GetComponent<OkCancelPanelView>();
            view.HideAsync();
        }
        private void OnEnable(){
            view.onConfirm += Hide;
        }
        private void OnDisable(){
            view.onConfirm -= Hide;
        }
        public void OnRewardAcquired(RewardBadgeSTO badgeSTO){
            rewardImg.sprite = badgeSTO.GetReward();
            rewardName.text  =  badgeSTO.Title;
            view.ShowAsync();
        }

        private void Hide(){
            view.HideAsync();
        }
    }
}