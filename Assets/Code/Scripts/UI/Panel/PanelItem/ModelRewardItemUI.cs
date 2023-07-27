using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel.PanelItem{
    public class ModelRewardItemUI : BasePanelItemUI<UnlockableImageButtonData>
    {
        [SerializeField] private Image presenter;
        [SerializeField] private Button button;
        public override void SetDefaultUI(ButtonData data)
        {
            button.onClick = data.OnClick;
        }

        public override void SetUI(UnlockableImageButtonData data)
        {
            button.interactable = data.isUnlocked;
            button.onClick = data.OnClick;
            presenter.sprite = data.GetImage();
        }
    }
}