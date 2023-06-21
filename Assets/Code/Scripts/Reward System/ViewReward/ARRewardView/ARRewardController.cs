using System;
using System.Collections.Generic;
using Project.UI.GameObjectUI;
using Project.UI.Panel;
using UnityEngine;
using UnityEngine.Events;
using Project.Utils.ExtensionMethods;
using UnityEngine.UI;

namespace Project.RewardSystem.ViewReward{
    public class ARRewardController : MonoBehaviour{
        private MenuPanelController m_UIController;
        private GameObject modelObj;
        private GameObjectButton modelObjectButton;
        public void SetUIController(MenuPanelController controllerPrefab){
            m_UIController = Instantiate(controllerPrefab, this.transform);
            m_UIController.Hide();
            Bounds bounds = modelObj.GetBoundsFromRenderer();
            m_UIController.transform.localPosition = new Vector3(0, bounds.size.y * 7/5, 0);

            Canvas canvas = m_UIController.EnsureComponent<Canvas>();
            canvas.worldCamera = Managers.GameManager.MainGameCam;
            m_UIController.EnsureComponent<GraphicRaycaster>();

            CreateUI();
        }
        public void SetModel(GameObject modelObj){
            this.modelObj = modelObj;
            modelObjectButton = this.modelObj.AddComponent<GameObjectButton>();
            modelObjectButton.OnButtonTouchPosition += OnModelTouch;
        }

        private void OnModelTouch(GameObjectButton button, Vector3 touchPosition)
        {
            if(!button.Equals(modelObjectButton)){
                return;
            }
            //InteractionEventsBehaviour.Instance.BlockRaycast();
            //m_UIController.transform.localPosition = touchPosition;
            _ = m_UIController.Show();
        }

        private void CreateUI(){
            MenuPanelViewData menuData = ScriptableObject.CreateInstance<MenuPanelViewData>();
            

            List<ButtonData> listButtonData = new()
            {
                CreateButton("Xóa", OnDeleteClicked),
                CreateButton("Hủy", HideUI),
            };
            menuData.ButtonNames = listButtonData.ToArray();

            m_UIController.SetUI(menuData);
        }

        private ButtonData CreateButton(string name, UnityAction OnClick){
            UnityEngine.UI.Button.ButtonClickedEvent buttonClickEvent = new();
            buttonClickEvent.AddListener(OnClick);
            return new ButtonData(){
                Name = name,
                OnClick = buttonClickEvent
            };
        }

        #region Button click handler methods
        private void OnDeleteClicked(){
            Destroy(this.gameObject);
            InteractionEventsBehaviour.Instance.UnblockRaycast();
        }

        public async void HideUI(){
            await m_UIController.Hide();
            InteractionEventsBehaviour.Instance.UnblockRaycast();
        }
        #endregion

    }
}