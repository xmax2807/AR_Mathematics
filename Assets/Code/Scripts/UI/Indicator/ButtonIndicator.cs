using Project.UI.TrueFalseUI;

namespace Project.UI.Indicator{
    public class ButtonIndicator : BaseIndicator<TrueFalseButton>{
        public event System.Action<int> ItemClickedCallback;
        private string textFormat = "#i";

        public ButtonIndicator ChangeTextFormat(string newFormat){
            textFormat = newFormat;
            return this;
        }
        protected override void OnBuildComponent(TrueFalseButton item, int index)
        {
            base.OnBuildComponent(item, index);
            string text = textFormat.Replace("#i", $"{index + 1}");
            item.Button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
            item.Button.onClick.AddListener(()=>ItemClickedCallback?.Invoke(index));
        }
    }
}