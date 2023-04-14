using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel{
    public class RequestPagerUI : ViewPagerUI<PreloadableTransitionView>{
        [SerializeField] private Transform scrollRect;
        [SerializeField] private Button SampleButton;
        protected override void InitT(PreloadableTransitionView panelView)
        {
            if(currentPanelView != null){
                currentPanelView.OnCreateLinkButton -= CreateLinkButton;
            }

            base.InitT(panelView);

            if(currentPanelView != null){
                currentPanelView.OnCreateLinkButton += CreateLinkButton;
            }
        }
        private Button CreateLinkButton(){
            return Instantiate(SampleButton, scrollRect);
        }
    }
}