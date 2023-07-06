using System;
using Gameframe.GUI.PanelSystem;
using Project.UI.Panel;

namespace Project.UI.Event.Popup{
    public class GeneralPopupUI : IPopupUI
    {
        private OkCancelPanelViewController m_controller;
        private PopupDataWithButton m_data;
        
        public event Action<bool> OnClose;

        public PanelViewController GetPanelViewController() => m_controller;

        public GeneralPopupUI(PanelType panelType, PopupDataWithButton data){
            this.m_data = data;
            this.m_controller = new OkCancelPanelViewController(panelType, OnConfirmCallback);
            Managers.TimeCoroutineManager.Instance.WaitUntil(()=>this.m_controller.IsViewLoaded,SetupView);
        }

        private void SetupView(){
            if(m_controller.View is not NotificationPanelView realView){
                UnityEngine.Debug.LogError("PopUI can't set ui since the view is not NotificationView");
                return;
            }
            realView.SetPopupData(m_data);
        }

        private void OnConfirmCallback(bool result){
            OnClose?.Invoke(result);
            // int index = result == true ? 1 : 0;
            // m_data.InvokeButtonClickAt(index);
        }
    }
}