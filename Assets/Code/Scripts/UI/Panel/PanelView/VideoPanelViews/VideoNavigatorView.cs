using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel.Navigator{
    public class VideoNavigatorView : MonoBehaviour{
        [SerializeField] VideoPagerController videoPager;
        [SerializeField] Indicator.TextIndicator textIndicator;
        [SerializeField] Gameframe.GUI.PanelSystem.PanelViewBase indicatorView;
        [SerializeField] private TMPro.TextMeshProUGUI TitleText;
        [SerializeField] private Button ToggleIndicator;

        void Awake(){
            videoPager.OnBuildComplete += Setup;
        }

        void OnEnable(){
            ToggleIndicator.onClick.AddListener(ShowIndicator);
            videoPager.OnPageChanged += OnPageChanged;
            HideIndicator();
        }

        

        void OnDisable(){
            ToggleIndicator.onClick.RemoveListener(ShowIndicator);
            videoPager.OnPageChanged -= OnPageChanged;
        }

        private void OnPageChanged(PreloadablePanelView view)
        {
            textIndicator.MoveTo(videoPager.CurrentIndex);
        }

        private void ShowIndicator()
        {
            indicatorView.ShowAsync();
        }
        public void HideIndicator()
        {
            indicatorView.HideAsync();
        }

        private void Setup()
        {
            LessonModel model = videoPager.LessonModel;
            TitleText.text = model.LessonTitle;
            textIndicator.ChangeTextFormat("Phần số #i");
            textIndicator.ManualFetchItem(model.VideoNumbers);
        }
    }
}