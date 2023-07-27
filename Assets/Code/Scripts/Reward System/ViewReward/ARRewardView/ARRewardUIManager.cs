using System;
using System.Linq;
using System.Collections.Generic;
using Project.UI.Panel;
using Project.Utils.ExtensionMethods;
using UnityEngine;
using UnityEngine.UI;
using Project.UI.GameObjectUI;

namespace Project.RewardSystem.ViewReward{
    public class ARRewardUIManager : MonoBehaviour{
        
        #region Main GUI Canvas
        [SerializeField] private Button CreateNewModelButton;
        [SerializeField] private Canvas MainGUICanvas;

        #endregion

        #region Item List GUI
        [SerializeField] private GridPanelController listModelChooser;
        private List<string> acquiredRewardUniqueNames;
        public event Action<int> OnItemChooseEvent;
        #endregion

        public void Awake(){
            if(listModelChooser == null){
                Debug.LogError("AR Reward List UI: No list model chooser");
                return;
            }
            listModelChooser?.Hide();
            listModelChooser.OnPanelHideViewEvent += OnListItemHide;

            CreateNewModelButton.onClick.AddListener(ShowItemList);
        }

        public void SetupUI(ARModelRewardPackSTO listRewardPrefabs){
            
            acquiredRewardUniqueNames = Managers.UserManager.Instance.CurrentUser?.UserAcquiredModel;
            acquiredRewardUniqueNames ??= new UserLocalModel("").ListOfAcquiredARModel;

            GridPanelViewData data = ScriptableObject.CreateInstance<GridPanelViewData>();

            data.Title = "Em hãy chọn 1 mô hình có sẵn";

            UnlockableImageButtonData[] imageButtonDatas = new UnlockableImageButtonData[listRewardPrefabs.Count];
            for(int i = 0; i < imageButtonDatas.Length; ++i){
                int cacheIndex = i;
                ARRewardSTO reward = listRewardPrefabs.GetAt(i);
                if(reward == null) continue;

                reward.InitModel();
                UnityEngine.UI.Button.ButtonClickedEvent clickEvent = new();
                clickEvent.AddListener(()=>OnRewardItemClicked(cacheIndex));

                imageButtonDatas[i] = new UnlockableImageButtonData(){
                    isUnlocked = acquiredRewardUniqueNames.IsContains((x)=>x == reward.UniqueName),
                    Image = reward.Avatar,
                    OnClick = clickEvent,
                    LockedImage = listRewardPrefabs.LockedImage,
                };
            }

            data.ButtonNames = imageButtonDatas.OrderBy((x)=> false == x.isUnlocked).ToArray();
            listModelChooser.SetUI(data);
        }

        private void OnRewardItemClicked(int cacheIndex)
        {
            listModelChooser.Hide();
            OnItemChooseEvent?.Invoke(cacheIndex);
        }

        private void ShowItemList(){
            InteractionEventsBehaviour.Instance.BlockRaycast();
            if(MainGUICanvas != null){
                MainGUICanvas.enabled = false;
            }
            _ = listModelChooser.Show();
        }

        private void OnListItemHide(){
            InteractionEventsBehaviour.Instance.UnblockRaycast();
            if(MainGUICanvas != null){
                MainGUICanvas.enabled = true;
            }
        }
    }
}