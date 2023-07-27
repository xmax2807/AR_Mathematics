using UnityEngine;
using Project.MiniGames.UI;
using UnityEngine.UI;

namespace Project.MiniGames.HouseBuilding{
    public class HouseBuildingInGameUI : InGameUI{
        [SerializeField] private Button Undo;

        private void OnEnable(){
            Undo.onClick.AddListener(RebuildPrevBlock);
        }

        protected void RebuildPrevBlock(){
            HouseBuildingEventManager.Instance.RaiseEvent(HouseBuildingEventManager.ReturnPrevBuildEventName);
        }
    }
}