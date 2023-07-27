using System.Collections.Generic;
using UnityEngine.UI;
namespace Project.UI.Event.Popup{
    public class PopupData {
        
        public string Title {get;private set;}
        public string MainText {get;private set;}

        public PopupData(){
        }
        public PopupData(PopupData clone){
            this.Title = clone.Title;
            this.MainText = clone.MainText;
        }
        
        public void AddText(string title, string mainText){
            this.Title = title;
            this.MainText = mainText;
        }
        
    }

    public class AutoClosePopupData : PopupData{
        public float Timeout {get;private set;}
        public bool IsClosed {get; private set;}
        public AutoClosePopupData(){
            Timeout = float.MaxValue;
        }
        public AutoClosePopupData(AutoClosePopupData clone) : base(clone){
            this.Timeout = clone.Timeout;
            
        }
    }
}