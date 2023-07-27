using UnityEngine;

namespace Project.UI.Indicator
{
    public class TextIndicator : BaseIndicator<TextIndicatorItem>{
        private string textFormat = "#i";
        public void ChangeTextFormat(string newFormat){
            textFormat = newFormat;
        }
        protected override void OnBuildComponent(TextIndicatorItem item, int index)
        {
            base.OnBuildComponent(item, index);
            item.ChangeText(textFormat.Replace("#i", $"{index + 1}"));
        }
    }
}