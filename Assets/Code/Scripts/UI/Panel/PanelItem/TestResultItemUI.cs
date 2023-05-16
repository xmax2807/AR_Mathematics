using UnityEngine;
using UnityEngine.UI;
using Project.QuizSystem.SaveLoadQuestion;
using TMPro;

namespace Project.UI.Panel.PanelItem{
    public class TestResultItemUI : BasePanelItemUI
    {
        [SerializeField] private Button viewDetailButton;
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI TestResult;
        public override void SetUI(ButtonData data)
        {
            viewDetailButton.onClick = data.OnClick;
            viewDetailButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.Name;
        }

        public void SetTestResultData(SemesterTestSaveData data){
            Title.text = data.Title;
            TestResult.text = $"{data.NumberOfCorrections}/{data.NumberOfQuestions}";
        }
        public void ToggleButtonClick(bool isOn) => viewDetailButton.interactable = isOn;
    }
}