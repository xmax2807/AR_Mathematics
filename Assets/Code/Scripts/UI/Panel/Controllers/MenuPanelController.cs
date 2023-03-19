using UnityEngine;
using Project.Managers;
using UnityEngine.UI;
using Project.Utils.ObjectPooling;
using System;

namespace Project.UI.Panel{
    public class MenuPanelController : BasePanelController<MenuPanelViewData>{
        [SerializeField] private Transform ButtonGroupTransform;
        [SerializeField] private Button ButtonPrefab;
        [SerializeField] private TMPro.TextMeshProUGUI Title;
        [SerializeField] private TMPro.TextMeshProUGUI Description;

        public override PanelEnumType Type => PanelEnumType.Menu;

        public override void SetUI(MenuPanelViewData Data){
            
            SpawnerManager.Instance.SpawnObjectsList<Button>(ButtonPrefab, 
            Data.ButtonNames.Length, 
            ButtonGroupTransform,
            (obj,i)=>{
                obj.name = Data.ButtonNames[i].Name;
                obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Data.ButtonNames[i].Name;
                obj.onClick.AddListener(Data.ButtonNames[i].OnClick);
            });

            Title.text = Data.Title;
            Description.text = Data.Description;
        }
    }
}