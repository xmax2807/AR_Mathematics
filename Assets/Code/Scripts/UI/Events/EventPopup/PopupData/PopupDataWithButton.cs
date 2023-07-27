using System;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Project.UI.Event.Popup
{
    public class PopupButtonData
    {
        public string Title;
        public Button.ButtonClickedEvent ClickedEvent;

        public bool IsRichText;

        public PopupButtonData(string title, Button.ButtonClickedEvent clickedEvent, bool isRichText = false){
            Title = title;
            ClickedEvent = clickedEvent;
            IsRichText = isRichText;
        }

        public virtual void ConfigButton(Button target){
            target.onClick = ClickedEvent;
            TMPro.TextMeshProUGUI text = target.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
            if(text == null) return;

            text.text = Title;
            text.richText = IsRichText;
        }
    }

    public class DelayPopupButtonData : PopupButtonData
    {
        private int _delayTime;
        public DelayPopupButtonData(string title, Button.ButtonClickedEvent clickedEvent, int delayTime, bool isRichText = false) : base(title, clickedEvent, isRichText)
        {
            _delayTime = delayTime;
        }

        public override void ConfigButton(Button target)
        {
            base.ConfigButton(target);
            target.interactable = false;
            TMPro.TextMeshProUGUI text = target.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);

            if(text == null) return;

            int count = _delayTime;
            // note: this method will invoke loop action first.
            Managers.TimeCoroutineManager.Instance.DoLoopAction(() => {
               text.text = Title + $" ({count})";
               --count;
            }, Duration: _delayTime, delayInterval: 1, ()=>{
                target.interactable = true;
                text.text = Title;
            });
        }
    }

    public class PopupDataWithButton : PopupData
    {

        public List<PopupButtonData> ButtonDatas { get; private set; }

        public PopupDataWithButton()
        {
            this.ButtonDatas = new(0);
        }

        public PopupDataWithButton(PopupDataWithButton clone) : base(clone)
        {
            this.ButtonDatas = new(clone.ButtonDatas);
        }

        public void AddButtonData(string title, Button.ButtonClickedEvent clickedEvent)
        {
            ButtonDatas ??= new();
            ButtonDatas.Add(
                new PopupButtonData(title, clickedEvent)
            );
        }

        public void InvokeButtonClickAt(int index)
        {
            if (index < 0 || index >= ButtonDatas.Count)
            {
                return;
            }

            ButtonDatas[index].ClickedEvent?.Invoke();
        }

        internal void AddButtonData(PopupButtonData buttonData)
        {
            if(ButtonDatas == null) return;

            ButtonDatas ??= new();
            ButtonDatas.Add(buttonData);
        }
    }

}