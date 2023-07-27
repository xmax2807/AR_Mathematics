using System.Threading.Tasks;
using Gameframe.GUI.PanelSystem;
using TMPro;

namespace Project.UI.Panel.Form{
    public abstract class FormPanelView : OkCancelPanelView{
        public event System.Action<string[]> onUserConfirmed;
        [UnityEngine.SerializeField] protected TextMeshProUGUI TitleText;
        public Task ShowAsync(string title){
            if(TitleText != null){
                TitleText.text = title;
            }
            return ShowAsync();
        }
        public void SetTitle(string title){
            if(TitleText != null){
                TitleText.text = title;
            }
        }
        private void OnEnable(){
            onConfirm += InvokeConfirm;
        }
        private void OnDisable(){
            onConfirm -= InvokeConfirm;
        }

        private void InvokeConfirm() => onUserConfirmed?.Invoke(GetFieldDatas());
        public abstract string[] GetFieldDatas();
    } 
}