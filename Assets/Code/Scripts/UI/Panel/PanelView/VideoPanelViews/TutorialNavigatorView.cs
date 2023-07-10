using System;
using Project.UI.TrueFalseUI;
using UnityEngine;
using UnityEngine.UI;
using static Project.UI.Panel.TutorialPagerController;

namespace Project.UI.Panel.Navigator{
    public class TutorialNavigatorView : MonoBehaviour
    {
        [SerializeField] TutorialPagerController videoPager;
        [SerializeField] Indicator.ButtonIndicator buttonIndicator;
        [SerializeField] Gameframe.GUI.PanelSystem.PanelViewBase indicatorView;
        [SerializeField] private TMPro.TextMeshProUGUI TitleText;
        [SerializeField] private Button ToggleIndicator;

        void Awake(){
            videoPager.OnBuildComplete += Setup;
        }

        void OnEnable(){
            ToggleIndicator.onClick.AddListener(ShowIndicator);
            videoPager.OnPageChanged += OnPageChanged;
            buttonIndicator.OnIndexChanged += IndicatorIndexChanged;
        }

        void OnDisable(){
            ToggleIndicator.onClick.RemoveListener(ShowIndicator);
            videoPager.OnPageChanged -= OnPageChanged;
            buttonIndicator.OnIndexChanged -= IndicatorIndexChanged;
        }

        private void IndicatorIndexChanged(int index)
        {
            videoPager.MoveTo(index);
        }

        private void OnPageChanged(PreloadablePanelView view)
        {
            buttonIndicator.MoveTo(videoPager.CurrentIndex);
            TitleText.text = videoPager.Datas[videoPager.CurrentIndex].TutorialName;
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
            TutorialData[] datas = videoPager.Datas;
            buttonIndicator.ManualFetchItemCallback(datas.Length, BuildCallback, false);
            ShowIndicator();
        }

        private void BuildCallback(TrueFalseButton button, int index)
        {
            TMPro.TextMeshProUGUI text = button.Button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if(text != null){
                text.text = videoPager.Datas[index].TutorialName;
            }
            button.Button.onClick.AddListener(HideIndicator);
        }
    }
}