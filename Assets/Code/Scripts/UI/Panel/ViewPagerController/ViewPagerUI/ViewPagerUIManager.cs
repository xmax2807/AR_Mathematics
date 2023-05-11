using UnityEngine;
using Project.Utils.ExtensionMethods;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Project.UI.Panel
{
    public class ViewPagerUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas loadingView;
        [SerializeField] protected Transform mainView;
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] protected ViewPagerController pagerController;

        private Dictionary<string,ViewPagerUI> cache;
        protected ViewPagerUI currentPagerUI;
        
        protected virtual void Awake()
        {
            cache = new (1);
            if (pagerController == null)
            {
                Debug.Log($"Can't initialize {this.name}");
                enabled = false;
                return;
            }
            pagerController.OnPageChanged += PagerChangedPage;
        }

        // private void OnEnable(){
        //     prevButton.onClick.AddListener(MovePrev);
        //     nextButton.onClick.AddListener(MoveNext);
        // }
        // private void OnDisable(){
        //     prevButton.onClick.RemoveListener(MovePrev);
        //     nextButton.onClick.RemoveListener(MoveNext);
        // }

        protected virtual void PagerChangedPage(PreloadablePanelView panelView)
        {
            UpdateViewUI(panelView);
            UpdateButtonUI();
        }

        private void UpdateViewUI(PreloadablePanelView panelView){
            var pagerUI = panelView.PagerUI;
            string panelName = panelView.GetType().Name;

            currentPagerUI?.Hide();

            if(cache.ContainsKey(panelName)){
                currentPagerUI = cache[panelName];
                currentPagerUI.InitUI(panelView);
            }
            else{
                var newPagerUI = Instantiate(pagerUI, mainView);
                newPagerUI.Manager = this;
                newPagerUI.InitUI(panelView);
                
                cache.Add(panelName, newPagerUI);
                currentPagerUI = newPagerUI;
            }

            currentPagerUI?.Show();
        }

        protected virtual void UpdateButtonUI()
        {
            prevButton.interactable = pagerController.CanMovePrev();
            nextButton.interactable = pagerController.CanMoveNext();
        }
        public void MovePrev()=>pagerController.MovePrev();
        public void MoveNext()=>pagerController.MoveNext();
        public void MoveTo(int index) => pagerController.MoveTo(index);

        public void HideNavigator(){
            if(prevButton != null) prevButton.gameObject.SetActive(false);
            if(nextButton != null) nextButton.gameObject.SetActive(false);
        }
        public void ShowNavigator(){
            if(prevButton != null) prevButton.gameObject.SetActive(true);
            if(nextButton != null) nextButton.gameObject.SetActive(true);
        }

        public void ToggleLoadingView(bool isOn){
            if(loadingView == null) return;

            loadingView.enabled = isOn;
        }
    }
}