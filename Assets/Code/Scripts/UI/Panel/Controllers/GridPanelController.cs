using UnityEngine;
using Project.Managers;
using UnityEngine.UI;
using UnityEngine.Events;
using Project.UI.Panel.PanelItem;

namespace Project.UI.Panel{
    public class GridPanelController : BasePanelController<GridPanelViewData>{
        [SerializeField] private Transform ButtonGroupTransform;
        [SerializeField] private BasePanelItemUI Item;
        [SerializeField] private TMPro.TextMeshProUGUI Title;
        [SerializeField] private TMPro.TextMeshProUGUI Description;
        public override PanelEnumType Type => PanelEnumType.Grid;

        public override bool CheckType(PanelViewData data)
        {
            return Type == data.Type;
        }

        public override void SetUI(GridPanelViewData Data){
            
            SpawnerManager.Instance.SpawnObjectsList<BasePanelItemUI>(Item, 
            Data.ButtonNames.Length, 
            ButtonGroupTransform,
            (obj,i)=>{
                obj.SetUI(Data.ButtonNames[i]);
                // obj.name = Data.ButtonNames[i].Name;
                // obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Data.ButtonNames[i].Name;
                // obj.onClick.AddListener(()=>OnItemClick?.Invoke(i));
            });

            Title.text = Data.Title;
            Description.text = Data.Description;
        }
    }
}