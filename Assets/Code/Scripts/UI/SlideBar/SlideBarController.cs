using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.ProgressBar{
    [RequireComponent(typeof(Slider))]
    public class SlideBarController : MonoBehaviour
    {
        private Slider _slider;
        protected BaseProgressAnim _animation;
        protected void Awake(){
            SetupSlider();
            _animation = new(_slider, 200);
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
