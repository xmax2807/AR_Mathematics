namespace Project.UI.Event.Popup{
    public class PopupDataBuilder{
        private PopupData data;

        public PopupDataBuilder(){
            data = new();
        }
        public PopupDataBuilder(PopupData clone){
            data = new(clone);
        }

        public PopupDataBuilder StartCreating(){
            data = new();
            return this;
        }

        public PopupDataBuilder AddText(string title, string mainText){
            data.AddText(title, mainText);
            return this;
        }
        public PopupData GetResult() => data;
    }

    public class PopupDataWithButtonBuilder{
        private PopupDataWithButton data;

        public PopupDataWithButtonBuilder(){
            data = new();
        }
        public PopupDataWithButtonBuilder(PopupDataWithButton clone){
            data = new(clone);
        }

        public PopupDataWithButtonBuilder StartCreating(){
            data = new();
            return this;
        }

        public PopupDataWithButtonBuilder AddText(string title, string mainText, bool isRichText = false){
            data.AddText(title, mainText);
            return this;
        }
        public PopupDataWithButtonBuilder AddButtonData(string title, System.Action callback){
            UnityEngine.UI.Button.ButtonClickedEvent buttonClickedEvent = new();
            buttonClickedEvent.AddListener(()=>callback?.Invoke());
            data.AddButtonData(title, buttonClickedEvent);
            return this;
        }

        public PopupDataWithButtonBuilder AddButtonData(PopupButtonData buttonData){
            data.AddButtonData(buttonData);
            return this;
        }
        public PopupDataWithButton GetResult() => data;
    }
}