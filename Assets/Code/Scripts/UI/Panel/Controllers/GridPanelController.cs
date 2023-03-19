using UnityEngine;
using Project.Managers;
using UnityEngine.UI;
using Project.Utils.ObjectPooling;
using System;
using UnityEngine.Events;

namespace Project.UI.Panel{
    public class GridPanelController : BasePanelController<GridPanelViewData>{
        [SerializeField] private Transform ButtonGroupTransform;
        [SerializeField] private Button Item;
        [SerializeField] private TMPro.TextMeshProUGUI Title;
        [SerializeField] private TMPro.TextMeshProUGUI Description;
        [SerializeField] private UnityEvent<int> OnItemClick;
        public override PanelEnumType Type => PanelEnumType.Grid;

        public override void SetUI(GridPanelViewData Data){
            
            SpawnerManager.Instance.SpawnObjectsList<Button>(Item, 
            Data.ButtonNames.Length, 
            ButtonGroupTransform,
            (obj,i)=>{
                obj.name = Data.ButtonNames[i].Name;
                obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Data.ButtonNames[i].Name;
                obj.onClick.AddListener(()=>OnItemClick?.Invoke(i));
            });

            Title.text = Data.Title;
            Description.text = Data.Description;
        }
    }
}