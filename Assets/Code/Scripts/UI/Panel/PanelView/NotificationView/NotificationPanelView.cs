using Gameframe.GUI.PanelSystem;
using Project.UI.Event.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel{
    public abstract class NotificationPanelView : OkCancelPanelView{
        [SerializeField] private TextMeshProUGUI Notification;
        [SerializeField] private TextMeshProUGUI Title;
        

        private void OnEnable(){
            if(!Notification.richText){
                Notification.richText = true;
            }
        }
        public void SetUI(string message, string title){
            Notification.text = message;
            Title.text = title;
        }
        public void SetPopupData(PopupData data){
            SetUI(data.MainText,data.Title);
            SetAdditionData(data);
        }
        protected abstract void SetAdditionData(PopupData data);
    }
}