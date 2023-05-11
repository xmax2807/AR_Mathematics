using UnityEngine;
using UnityEngine.UI;
using Project.UI.Indicator;

namespace Project.UI.TrueFalseUI{

    [RequireComponent(typeof(Button),typeof(Image))]
    public class TrueFalseButton : BaseTrueFalseUI<Color>, IIndicatorItem
    {
        public Button Button{get;private set;}
        private Image background;
        protected override void Awake(){
            base.Awake();
            Button = GetComponent<Button>();
            background = GetComponent<Image>();
        }
        public override void ChangeUI(bool isTrue)
        {
            background.color = isTrue ? TrueUI : FalseUI;
        }

        public override void Reset()
        {
            background.color = DefaultUI;
        }

        public void SwitchToUnselectedUI()
        {
            ChangeUI(false);
        }

        public void SwitchToSelectedUI()
        {
            ChangeUI(true);
        }
    }
}