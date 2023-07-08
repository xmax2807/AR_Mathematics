using UnityEngine;
using Project.Managers;
using UnityEngine.UI;
using Project.Utils.ObjectPooling;
using System;
using System.Collections;

namespace Project.UI.Panel{
    public class MenuPanelController : BasePanelController<MenuPanelViewData>{
        [SerializeField] private Transform ButtonGroupTransform;
        [SerializeField] private Button ButtonPrefab;
        [SerializeField] private TMPro.TextMeshProUGUI Title;
        [SerializeField] private TMPro.TextMeshProUGUI Description;

        public override PanelEnumType Type => PanelEnumType.Menu;

        public override bool CheckType(PanelViewData data)
        {
            return Type == data.Type;
        }

        public override void SetUI(MenuPanelViewData Data){
            
            SpawnerManager.Instance.SpawnObjectsList<Button>(ButtonPrefab, 
            Data.ButtonNames.Length, 
            ButtonGroupTransform,
            (obj,i)=>{
                obj.name = Data.ButtonNames[i].Name;
                obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Data.ButtonNames[i].Name;
                obj.onClick = Data.ButtonNames[i].OnClick;
            });

            if(Title != null){
                Title.text = Data.Title;
            }

            if(Description != null){
                Description.text = Data.Description;
            }

            StartCoroutine(CenterElements());
        }

        IEnumerator CenterElements(){
            yield return new WaitForEndOfFrame();
            
            
            Canvas rootCanvas = Managers.GameManager.RootCanvas;

            if(rootCanvas != null){
                
                RectTransform rootCanvasRect = rootCanvas.GetComponent<RectTransform>();
                float canvasWidth = rootCanvasRect.rect.width - 60;

                RectTransform buttonGroupRect = ButtonGroupTransform.GetComponent<RectTransform>();
                LayoutRebuilder.ForceRebuildLayoutImmediate(buttonGroupRect);
                yield return new WaitForEndOfFrame();
                float currentButtonGroupWidth = buttonGroupRect.rect.width;

                //Debug.Log($"Canvas: {canvasWidth}, Group: {currentButtonGroupWidth}");
                if(canvasWidth <= currentButtonGroupWidth){
                    yield break;
                }

                float emptyObjWidth = (canvasWidth - currentButtonGroupWidth) / 2;

                GameObject emptyObj = new GameObject("Empty");
                RectTransform rectTransform = emptyObj.AddComponent<RectTransform>();
                rectTransform.offsetMin = new Vector2(0,0);
                rectTransform.offsetMax = new Vector2(0, 0);
                emptyObj.transform.SetParent(ButtonGroupTransform, false);
                emptyObj.transform.SetAsFirstSibling();
                rectTransform.sizeDelta = new Vector2(emptyObjWidth,0);

                // buttonGroupRect.ForceUpdateRectTransforms();
            }
        }
    }
}