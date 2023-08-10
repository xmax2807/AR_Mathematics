namespace Project.UI.Indicator{
    public class ProgressIndicator : BaseIndicator<ProgressIndicatorButton>{
        
        private LimitNextNavigationCondition cacheCondition;
        protected override INavigationCondition InitNavigation(int count)
        {
            cacheCondition = new LimitNextNavigationCondition(count);
            return cacheCondition;
        }

        protected override void OnBuildComponent(ProgressIndicatorButton item, int index)
        {
            base.OnBuildComponent(item, index);
            string text = $"{index + 1}";
            if(item.Text != null){
                item.Text.text = text;
            }
            item.OnClick += ()=>InvokeIndexChanged(index);
            item.SwitchToUnReachableUI();
        }

        public void MarkAsCompleted(){
            items[currentIndex].SwitchToCompletedUI();
            UnlockNextItem();
        }
        private void UnlockNextItem(){
            if(currentIndex + 1 >= items.Length) return;

            items[currentIndex + 1].SwitchToUnselectedUI();
            cacheCondition.UnlockNext(currentIndex);
        }

        public void RevealAnswer(bool[] ansResults){
            int count = UnityEngine.Mathf.Min(ansResults.Length, items.Length);

            for(int i = 0; i < count; ++i){
                items[i].SwitchToTrueOrFalseAnswerUI(ansResults[i]);
            }
        }
    }
}