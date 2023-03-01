using UnityEngine.UI;
using Project.Managers;
using UnityEngine;

namespace Project.UI.ProgressBar{
    public interface IProgressAnim{
        void StartAnimation();
        void EndAnimation();
        void SettingUp(float endValue, float duration);
    }
    public class BaseProgressAnim : IProgressAnim{
        protected Slider _slider;
        protected float currentValue;
        private readonly float maxValue;
        protected readonly float sliderMaxVal;
        protected float endValue;
        protected float deltaValue;
        protected Coroutine animationProgress;
        public BaseProgressAnim(Slider slider, float maxValue){
            _slider = slider;
            sliderMaxVal = _slider.maxValue;
            this.maxValue = maxValue;
        }
        public virtual void StartAnimation(){
            currentValue = _slider.value;
            animationProgress = TimeCoroutineManager.Instance.DoLoopAction(UpdateUI, StopCondition, 1/60f, EndAnimation);
        }
        public virtual void SettingUp(float end, float duration){
            endValue = end/maxValue * _slider.maxValue + _slider.minValue;
            deltaValue = (endValue - _slider.value) / duration/40f;
        }
        public virtual void EndAnimation(){
            if(animationProgress == null) return;

            TimeCoroutineManager.Instance.StopCoroutine(animationProgress);
            _slider.value = endValue;
        }
        protected virtual bool StopCondition()=>currentValue > endValue;

        protected virtual void UpdateUI(){
            currentValue += deltaValue;
            _slider.value = currentValue;
        }
    }
    public class InfiniteProgressAnim : BaseProgressAnim
    {
        public InfiniteProgressAnim(Slider slider, float maxValue) : base(slider, maxValue)
        {
        }
        protected override void UpdateUI()
        {
            currentValue += deltaValue;
            if(currentValue > sliderMaxVal){
                currentValue -= sliderMaxVal;
                endValue -= sliderMaxVal;
            }
            _slider.value = currentValue;
        }
    }
    public class WrapperProgressAnim : IProgressAnim
    {
        private IProgressAnim wrappee;

        public WrapperProgressAnim(IProgressAnim anim){
            wrappee = anim;
        }
        public virtual void EndAnimation()
        {
            wrappee?.EndAnimation();
        }

        public virtual void SettingUp(float endValue, float duration)
        {
            wrappee?.SettingUp(endValue, duration);
        }

        public virtual void StartAnimation()
        {
            wrappee?.StartAnimation();
        }
    }
    public sealed class ProgressSoundAnim : WrapperProgressAnim{
        private AudioClip _clip;

        public ProgressSoundAnim(AudioClip clip, IProgressAnim anim) : base(anim)
        {
            _clip = clip;
        }

        public override void StartAnimation()
        {
            AudioManager.Instance.PlaySoundFX(_clip);
            base.StartAnimation();
        }
    }
}