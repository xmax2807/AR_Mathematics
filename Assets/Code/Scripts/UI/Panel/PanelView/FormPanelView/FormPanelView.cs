using Gameframe.GUI.PanelSystem;
namespace Project.UI.Panel.Form{
    public class FormPanelView : OkCancelPanelView{
        public event System.Action<string[]> onUserConfirmed;

        private void OnEnable(){
        }
        private void OnDisable(){}
    } 
}