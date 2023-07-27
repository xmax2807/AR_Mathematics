using UnityEngine;
using Project.UI.Event;
namespace Project.UI.Panel{
    public class TogglePanel : MonoBehaviour, IUIEventListener{
        [SerializeField] private Canvas mainPanel;
        [SerializeField] private Canvas hiddenPanel;
        [SerializeField] private UIEventScriptableObject eventSO;

        public string UniqueID => "TogglePanel";

        private void OnEnable(){
            ShowUI();
            eventSO?.AddListener(this);
        }
        private void OnDisable(){
            eventSO?.RemoveListener(this);
        }

        public void HideUI()
        {
            mainPanel.enabled = false;
            hiddenPanel.enabled = true;
        }

        public void ShowUI()
        {
            mainPanel.enabled = true;
            hiddenPanel.enabled = false;
        }
    } 
}