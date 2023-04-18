using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel{
    public class RequestPagerUI : ViewPagerUI<PreloadableTransitionView>{
        [SerializeField] private Transform scrollRect;
        [SerializeField] private Button SampleButton;
        protected override void InitT(PreloadableTransitionView panelView)
        {
            DeleteLinkButtons();
            base.InitT(panelView);
            CreateLinkButtons();
        }
        private Button CreateLinkButton(){
            return Instantiate(SampleButton, scrollRect);
        }
        private void CreateLinkButtons(){
            foreach(var task in currentPanelView.buttonCreationTasks){
                Button button = CreateLinkButton();
                button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = task.Item1;
                button.onClick.AddListener(task.Item2); 
            }
        }
        private void DeleteLinkButtons(){
            foreach(Transform child in scrollRect.transform){
                Destroy(child.gameObject); 
            }
        } 
    }
}