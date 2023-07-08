using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Indicator{
    public class ProgressIndicatorButton : MonoBehaviour, IProgressIndicatorItem, IClickableIndicatorItem
    {
        [SerializeField] private Button button;
        [SerializeField] private TMPro.TextMeshProUGUI text;
        public TMPro.TextMeshProUGUI Text => text;
        
        #region Button Color
        [Header("Button Color")]
        [SerializeField] private Color ButtonUnselectedColor = Color.white;
        [SerializeField] private Color ButtonSelectedColor = Color.white;
        [SerializeField] private Color ButtonCompletedColor = Color.white;
        [SerializeField] private Color ButtonLockedColor = Color.white;
        #endregion

        #region Text Color
        [Header("Text Color")]
        [SerializeField] private Color TextUnselectedColor = Color.white;
        [SerializeField] private Color TextSelectedColor = Color.white;
        [SerializeField] private Color TextCompletedColor = Color.white;
        [SerializeField] private Color TextLockedColor = Color.white;
        #endregion

        public event Action OnClick;
        private bool m_isCompleted = false;
        protected void Awake(){
            if(button == null){
                Debug.LogError("Indicator Item can't be interact since Button Component is null");
                return;
            }
            button.onClick.AddListener(InvokeButtonClick);
        }

        private void InvokeButtonClick()=> OnClick?.Invoke();
        public void SwitchToCompletedUI()
        {
            m_isCompleted = true;
            if(button != null){
                button.interactable = true;
                button.image.color = ButtonCompletedColor;
            }
        }

        public void SwitchToSelectedUI()
        {
            if(button != null){
                button.interactable = true;
                button.image.color = ButtonSelectedColor;
            }
        }

        public void SwitchToUnReachableUI()
        {
            m_isCompleted = false;
            if(button != null){
                button.interactable = false;
                button.image.color = ButtonLockedColor;
            }
        }

        public void SwitchToUnselectedUI()
        {
            if(m_isCompleted){
                SwitchToCompletedUI();
                return;
            }
            if(button != null){
                button.interactable = true;
                button.image.color = ButtonUnselectedColor;
            }
        }
    }
}