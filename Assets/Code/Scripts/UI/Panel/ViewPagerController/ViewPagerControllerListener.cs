using UnityEngine;
using Project.UI.Event;
namespace Project.UI.Panel{
    public class ViewPagerControllerListener : MonoBehaviour, IPagerUIEventListener
    {
        [SerializeField] private UIPagerEventSO EventSO;
        [SerializeField] private ViewPagerUIManager manager;
        [SerializeField] private bool shouldHideUIOnStart = false;
        public string UniqueID => "ViewPagerController";

        private void Awake(){
            manager ??= GetComponent<ViewPagerUIManager>();
            if(shouldHideUIOnStart == true){
                manager.HideUI();
            } 
            EventSO?.AddListener(this);
        }
        private void OnDestroy(){
            EventSO?.RemoveListener(this);
        }
        public void HideUI()
        {
            manager?.HideUI();
        }

        public void MoveTo(int index)
        {
            ShowUI();
            manager?.MoveTo(index);
        }

        public void ShowUI()
        {
            manager?.ShowUI();
        }
    }
}