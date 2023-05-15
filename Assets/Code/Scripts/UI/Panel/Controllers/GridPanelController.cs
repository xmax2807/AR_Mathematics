using UnityEngine;
using Project.Managers;
using UnityEngine.UI;
using UnityEngine.Events;
using Project.UI.Panel.PanelItem;

namespace Project.UI.Panel{
    public class GridPanelController : BasePanelController<GridPanelViewData>{
        public event System.Action<BasePanelItemUI, int> OnBuildUI;
        [SerializeField] private Transform ButtonGroupTransform;
        [SerializeField] private BasePanelItemUI Item;
        [SerializeField] private TMPro.TextMeshProUGUI Title;
        [SerializeField] private TMPro.TextMeshProUGUI Description;
        public override PanelEnumType Type => PanelEnumType.Grid;

        public override bool CheckType(PanelViewData data)
        {
            return Type == data.Type;
        }
        
        public void SetUI(BasePanelItemUI itemUI, GridPanelViewData data){
            Item = itemUI;
            SetUI(data);
        }

        public override void SetUI(GridPanelViewData Data){
            
            SpawnerManager.Instance.SpawnObjectsList<BasePanelItemUI>(Item, 
            Data.ButtonNames.Length, 
            ButtonGroupTransform,
            (obj,i)=>{
                obj.SetUI(Data.ButtonNames[i]);
                OnBuildUI?.Invoke(obj, i);
                // obj.name = Data.ButtonNames[i].Name;
                // obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Data.ButtonNames[i].Name;
                // obj.onClick.AddListener(()=>OnItemClick?.Invoke(i));
            });

            if(Title != null){
                Title.text = Data.Title;
            }
            if(Description != null){
                Description.text = Data.Description;
            }
        }
    }
}