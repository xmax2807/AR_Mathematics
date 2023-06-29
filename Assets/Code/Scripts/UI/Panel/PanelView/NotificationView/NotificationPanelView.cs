using Gameframe.GUI.PanelSystem;
using TMPro;
using UnityEngine;

namespace Project.UI.Panel{
    public class NotificationPanelView : OkCancelPanelView{
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
    }
}