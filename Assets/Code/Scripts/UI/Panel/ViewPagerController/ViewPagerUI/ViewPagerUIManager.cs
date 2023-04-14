using UnityEngine;
using Project.Utils.ExtensionMethods;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Project.UI.Panel
{
    public class ViewPagerUIManager : MonoBehaviour
    {
        [SerializeField] private Transform mainView;
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private ViewPagerController pagerController;

        private Dictionary<string,ViewPagerUI> cache;
        private ViewPagerUI currentPagerUI;
        
        private void Awake()
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

        private void OnEnable(){
            prevButton.onClick.AddListener(MovePrev);
            nextButton.onClick.AddListener(MoveNext);
        }
        private void OnDisable(){
            prevButton.onClick.RemoveListener(MovePrev);
            nextButton.onClick.RemoveListener(MoveNext);
        }

        private void PagerChangedPage(PreloadablePanelView panelView)
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
                newPagerUI.InitUI(panelView);
                newPagerUI.Manager = this;
                
                cache.Add(panelName, newPagerUI);
                currentPagerUI = newPagerUI;
            }

            currentPagerUI?.Show();
        }

        private void UpdateButtonUI()
        {
            prevButton.interactable = pagerController.CanMovePrev();
            nextButton.interactable = pagerController.CanMoveNext();
        }
        public void MovePrev()=>pagerController.MovePrev();
        public void MoveNext()=>pagerController.MoveNext();
    }
}