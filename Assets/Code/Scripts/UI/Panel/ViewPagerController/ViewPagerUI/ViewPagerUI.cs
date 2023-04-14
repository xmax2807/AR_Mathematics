using UnityEngine;
using UnityEngine.UI;
using Gameframe.GUI.PanelSystem;
using System.Threading.Tasks;

namespace Project.UI.Panel{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public abstract class ViewPagerUI : MonoBehaviour{
        private Canvas canvas;
        private void Awake(){
            canvas = GetComponent<Canvas>();
        }
        [HideInInspector]
        public ViewPagerUIManager Manager;
        public abstract void InitUI(PreloadablePanelView panelView);
        public virtual void Hide(){
            canvas.enabled = false;
        }
        public virtual void Show(){
            canvas.enabled = true;
        }
    }
    public class ViewPagerUI<TController> : ViewPagerUI where TController : PreloadablePanelView{
        protected virtual void InitT(TController panelView) => currentPanelView = panelView;
        protected TController currentPanelView;
        sealed public override void InitUI(PreloadablePanelView panelView)
        {
            if(panelView is TController view){
                InitT(view);
            }
        }
    }
}