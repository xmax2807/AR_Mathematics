using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.ProgressBar{
    [RequireComponent(typeof(Slider))]
    public class SlideBarController : MonoBehaviour
    {
        protected Slider _slider;
        protected IProgressAnim _animation;
        protected void Awake(){
            SetupSlider();
            SetupAnimation(200);
        }
        public virtual void SetupAnimation(float maxValue){
            if(_slider == null){
                SetupSlider();
            }
            _slider.value = 0;
            _animation = new BaseProgressAnim(_slider,maxValue);
        }
        public virtual void UpdateEndValue(float endValue, float duration = 1){
            _animation.SettingUp(endValue, duration);
        }
        private void SetupSlider(){
            _slider = GetComponent<Slider>();
            _slider.interactable = false;
            _slider.navigation = new(){
                mode = Navigation.Mode.None, 
            };
            _slider.minValue = 0f;
            _slider.maxValue = 1f;
        }
        public void ChangeAnimation(BaseProgressAnim anim){
            _animation.EndAnimation();
            _animation = anim;
        }
        public void StartAnimation()=>_animation?.StartAnimation();
    }
}
