using System.Collections;
using Project.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Project.UI.ProgressBar
{
    public class ProgressBarController : SlideBarController
    {
        [SerializeField] private AudioClip buildUpSFX;
        [SerializeField] private AudioClip RewardSFX;
        [SerializeField] UnityEvent<float> OnValueChanged;
        
        
        void OnEnable(){
            _slider.onValueChanged.AddListener(OnSliderChanged);
        }
        void OnDisable(){
            _slider.onValueChanged.RemoveListener(OnSliderChanged);
        }
        void Start(){
            _animation.SettingUp(153,buildUpSFX.length);
            StartAnimation();
        }

        protected override void SetupAnimation()
        {
            _animation = new ProgressSoundAnim(buildUpSFX, new InfiniteProgressAnim(_slider, 100));
        }

        private void OnSliderChanged(float value){
            OnValueChanged?.Invoke(value * 100);
        }
    }
}
