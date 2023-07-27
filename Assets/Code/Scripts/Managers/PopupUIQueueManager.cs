using System;
using System.Collections.Generic;
using Project.UI.Event.Popup;
using UnityEngine;
using Gameframe.GUI.PanelSystem;
using Gameframe.GUI.Camera.UI;
using UnityEngine.SceneManagement;

namespace Project.Managers{
    public class PopupUIQueueManager : MonoBehaviour{
        public static PopupUIQueueManager Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            Instance.m_queuePopup = new();
            Instance.m_stackSystem = ScriptableObject.CreateInstance<PanelStackSystem>();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
            MainCanvas = GameObject.FindGameObjectWithTag("MainCanvas")?.GetComponent<Canvas>();
            EnsureContainer();
        }
        private void OnSceneUnloaded(Scene scene){
            m_stackSystem.RemoveController(m_popupContainer);
            if(m_popupContainer != null){
                Destroy(m_popupContainer.gameObject);
                m_popupContainer = null;
            }
            m_stackSystem.Clear();
            m_isStarted = false;
        }

        private Queue<IPopupUI> m_queuePopup;
        private bool m_isStarted = false;
        private Gameframe.GUI.PanelSystem.PanelStackSystem m_stackSystem;
        private PopupContainer m_popupContainer;
        //private PanelStackController m_stackController;
        private Canvas MainCanvas;

        public void EnqueueEventPopup(IPopupUI popupUI, bool immediateStart = true){
            m_queuePopup.Enqueue(popupUI);
            if(immediateStart){
                StartPopup();
            }
        }

        private void StartPopup()
        {
            if(m_isStarted == true){// the queue is already started
                return;
            }

            m_isStarted = true;
            EnsureContainer();
            EnsureLastSibling();

            ContinuePop();
        }

        private void ContinuePop(){
            bool hasTop = m_queuePopup.TryPeek(out IPopupUI popupUI);
            if(hasTop == false){
                m_isStarted = false;
                return;
            }
            popupUI.OnClose += Dequeue;
            var controller = popupUI.GetPanelViewController();
            
            _ = m_stackSystem.PushAsync(controller);
        }

        private async void Dequeue(bool isConfirm)
        {
            bool hasUI = m_queuePopup.TryDequeue(out IPopupUI ui);
            
            if(!hasUI){
                m_isStarted = false;
                return;
            }
            await m_stackSystem.PopAsync();

            var view = ui.GetPanelViewController().View;
            if(view != null){
                Destroy(view.gameObject);
            }

            m_isStarted = m_queuePopup.Count > 0;
            if(m_isStarted){
                ContinuePop();
            }
        }

        private void EnsureContainer(){
            if(MainCanvas == null){
                Debug.Log("Canvas is null");
                return;
            }
            if(m_popupContainer == null){
                var newObj = new GameObject("Popup Container");
                newObj.transform.SetParent(MainCanvas.transform, false);

                //Set up rect
                var rectTrans = newObj.AddComponent<RectTransform>();
                rectTrans.SetAsLastSibling();
                rectTrans.anchorMin = new Vector2(0, 0);
                rectTrans.anchorMax = new Vector2(1, 1);
                rectTrans.offsetMin = new Vector2(0, 0);
                rectTrans.offsetMax = new Vector2(0, 0);
                rectTrans.ForceUpdateRectTransforms();
                //
                m_popupContainer = newObj.AddComponent<PopupContainer>();
                m_popupContainer.SettingUp(m_stackSystem, UIEventManager.Current);

                //m_stackController = new PanelStackController(this.m_stackSystem, m_popupContainer, UIEventManager.Current);
                m_stackSystem.AddController(m_popupContainer);
                return;
            }
            
            // if(m_stackController == null){
            //     m_stackController = new PanelStackController(this.m_stackSystem, m_popupContainer, UIEventManager.Current);
            //     m_stackSystem.AddController(m_stackController);
            // }
        }

        private void EnsureLastSibling(){
            if(m_popupContainer == null){
                EnsureContainer();
            }
            m_popupContainer.ParentTransform.SetAsLastSibling();
        }
    }
}