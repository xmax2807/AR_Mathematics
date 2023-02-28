using UnityEngine.UI;
using Project.Managers;
using UnityEngine;

namespace Project.UI.ProgressBar{
    public class BaseProgressAnim{
        Slider _slider;
        private float currentValue;
        private readonly float maxValue;
        private float endValue;
        private float deltaValue;
        private Coroutine animationProgress;
        public BaseProgressAnim(Slider slider, float maxValue){
            _slider = slider;
            this.maxValue = maxValue;
        }
        public virtual void StartAnimation(){
            currentValue = _slider.value;
            animationProgress = TimeCoroutineManager.Instance.DoLoopAction(UpdateUI, StopCondition, 1/60f, EndAnimation);
        }
        public virtual void SettingUp(float start, float end, float duration){
            endValue = end/maxValue * _slider.maxValue + _slider.minValue;
            deltaValue = (endValue - _slider.value) / duration/30f;
        }
        public virtual void EndAnimation(){
            TimeCoroutineManager.Instance.StopCoroutine(animationProgress);
            _slider.value = endValue;
        }
        protected virtual bool StopCondition()=>currentValue > endValue;

        protected virtual void UpdateUI(){
            currentValue += deltaValue;
            _slider.value = currentValue;
        }
    }
    public sealed class ProgressSoundAnim : BaseProgressAnim{
        private AudioClip _clip;

        public ProgressSoundAnim(AudioClip clip, Slider slider, float maxValue) : base(slider, maxValue)
        {
            _clip = clip;
        }

        public override void StartAnimation()
        {
            AudioManager.Instance.PlayEffect(_clip);
            base.StartAnimation();
        }
    }
}