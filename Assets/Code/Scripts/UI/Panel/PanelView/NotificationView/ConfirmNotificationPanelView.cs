using Project.UI.Event.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel{
    public class ConfirmNotificationPanelView : NotificationPanelView{
        [SerializeField] private Transform ButtonContainer;
        [SerializeField] private Button buttonPrefab;

        protected override void SetAdditionData(PopupData data)
        {
            if(data is not PopupDataWithButton realData){
                Debug.Log("Confirm Notification Panel View: Try to pop ui with wrong data");
                return;
            }
            PopulateButton(realData);
        }

        private void PopulateButton(PopupDataWithButton data){
            int count = data.ButtonDatas.Count;
            for(int i = 0; i < count; ++i){
                var newButton = Instantiate(buttonPrefab, ButtonContainer);
                newButton.onClick = data.ButtonDatas[i].ClickedEvent;
                newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.ButtonDatas[i].Title;
            }

            if(count <= 0){
                return;
            }
            var cancelButtonData = data.ButtonDatas[0];
            cancelButtonData.ClickedEvent?.AddListener(Cancel);
            
            if(count <= 1){
                return;
            }
            var confirmButtonData = data.ButtonDatas[1];
            cancelButtonData.ClickedEvent?.AddListener(Ok);
        }
    }
}