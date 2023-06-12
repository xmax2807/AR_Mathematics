using UnityEngine;
using UnityEngine.UI;
using Gameframe.GUI.PanelSystem;

namespace Project.RewardSystem{
    public class RewardPopup : MonoBehaviour{
        [SerializeField] private Image rewardImg;
        [SerializeField] private TMPro.TextMeshProUGUI rewardName;
        private OkCancelPanelView view;
        private Canvas canvas;

        public event System.Action OnClose;
        private void Awake(){
            view = GetComponent<OkCancelPanelView>();
            //canvas = GetComponent<Canvas>();
//            view.HideAsync();
        }
        private void Start(){
            view ??= GetComponent<OkCancelPanelView>();
            //canvas ??= GetComponent<Canvas>();
            //view.HideAsync();
        }
        private void OnEnable(){
            view.onConfirm += Hide;
        }
        private void OnDisable(){
            view.onConfirm -= Hide;
        }
        public void OnRewardAcquired(RewardBadgeSTO badgeSTO){
            SetRewardData(badgeSTO.Title, badgeSTO.GetReward());


            if(view == null){
                view = GetComponent<OkCancelPanelView>();
            }
            // if(canvas == null){
            //     canvas = GetComponent<Canvas>();
            // }
        }

        public void SetRewardData(string title, Sprite image){
            rewardImg.sprite = image;
            rewardName.text = title;
        }

        private async void Hide(){
            OnClose?.Invoke();
            await view?.HideAsync();
            //canvas.enabled = false;
        }

        public void Show(){
            if(view == null){
                view = GetComponent<OkCancelPanelView>();
            }
            // if(canvas == null){
            //     canvas = GetComponent<Canvas>();
            // }
            //canvas.enabled = true;
            view?.ShowAsync();
        }
    }
}