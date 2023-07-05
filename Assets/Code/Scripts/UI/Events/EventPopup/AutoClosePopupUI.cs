using System;
using Gameframe.GUI.PanelSystem;
using Project.UI.Panel;

namespace Project.UI.Event.Popup{
    public class AutoClosePopupUI : IPopupUI
    {
        private OkCancelPanelViewController m_controller;
        private PopupData m_data;

        public AutoNotificationPanelView view;
        
        public event Action<bool> OnClose;

        public PanelViewController GetPanelViewController() => m_controller;

        public AutoClosePopupUI(PanelType panelType, PopupData data){
            this.m_data = data;
            this.m_controller = new OkCancelPanelViewController(panelType, OnConfirmCallback);
            Managers.TimeCoroutineManager.Instance.WaitUntil(()=>this.m_controller.IsViewLoaded,SetupView);
        }

        private void SetupView(){
            
            if(m_controller.View is not AutoNotificationPanelView realView){
                UnityEngine.Debug.LogError("AutoPopUI can't set ui since the view is not AutoNotificationPanelView");
                return;
            }
            realView.SetPopupData(m_data);
            view = realView;
        }

        private void OnConfirmCallback(bool result){
            OnClose?.Invoke(result);
        }

        public void ManuallyClose(bool result = false){
            OnClose?.Invoke(result);
        }
    }
}