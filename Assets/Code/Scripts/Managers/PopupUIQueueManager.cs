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
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
            EnsureContainer();
        }

        private Queue<IPopupUI> m_queuePopup;
        private bool m_isStarted = false;
        private Gameframe.GUI.PanelSystem.PanelStackSystem m_stackSystem;
        private PopupContainer m_popupContainer;
        private PanelStackController m_stackController;
        private Canvas MainCanvas => GameManager.RootCanvas;

        public void EnqueueEventPopup(IPopupUI popupUI, bool immediateStart = true){
            m_queuePopup.Enqueue(popupUI);
            if(immediateStart){
                StartPopup();
            }
        }

        private async void StartPopup()
        {
            if(m_isStarted == true){// the queue is already started
                return;
            }

            EnsureContainer();
            EnsureLastSibling();

            m_isStarted = true;
            bool hasTop = m_queuePopup.TryPeek(out IPopupUI popupUI);
            if(hasTop == false){
                return;
            }
            popupUI.OnClose += Dequeue;
            var controller = popupUI.GetPanelViewController();
            
            await m_stackSystem.PushAsync(controller);
        }

        private async void Dequeue(bool isConfirm)
        {
            bool hasUI = m_queuePopup.TryDequeue(out IPopupUI ui);
            m_isStarted = m_queuePopup.Count > 0;
            if(!hasUI){
                return;
            }
            await m_stackSystem.PopAsync();

            var view = ui.GetPanelViewController().View;
            if(view != null){
                Destroy(view.gameObject);
            }
            if(m_isStarted){
                StartPopup();
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

                m_stackController = new PanelStackController(this.m_stackSystem, m_popupContainer, UIEventManager.Current);
                m_stackSystem.AddController(m_stackController);
                return;
            }
            
            if(m_stackController == null){
                m_stackController = new PanelStackController(this.m_stackSystem, m_popupContainer, UIEventManager.Current);
                m_stackSystem.AddController(m_stackController);
            }
        }

        private void EnsureLastSibling(){
            if(m_popupContainer == null){
                EnsureContainer();
            }
            m_popupContainer.ParentTransform.SetAsLastSibling();
        }
    }
}