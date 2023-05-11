using Project.UI.TrueFalseUI;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel.PanelItem
{
    public class SelectButtonItemUI : BasePanelItemUI
    {
        [SerializeField] protected TrueFalseButton button;
        private static SelectButtonItemUI currentSelectItem;
        private bool isClicked;
        public override void SetUI(ButtonData data)
        {
            isClicked = false;
            button.name = data.Name;
            button.Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.Name;
            button.Button.onClick = data.OnClick;
            button.Button.onClick.AddListener(ToggleUI);
            button.ChangeUI(isClicked);
        }

        private void ToggleUI(){
            if(currentSelectItem != this){
                currentSelectItem?.ToggleUI();
                currentSelectItem = this;
            }
            isClicked = !isClicked;
            button.ChangeUI(isClicked);
        }
    }
}