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

        public override void SetupAnimation(float maxValue)
        {
            _slider.value = 0;
            _animation = new ProgressSoundAnim(buildUpSFX, new InfiniteProgressAnim(_slider, maxValue));
        }

        private void OnSliderChanged(float value){
            OnValueChanged?.Invoke(value * 100);
        }
    }
}
