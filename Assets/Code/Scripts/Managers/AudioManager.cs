using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;

namespace Project.Managers{
    public class AudioManager : MonoBehaviour{
        public static AudioManager Instance;

        private const string googleSTT_URL = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=vi&q=";
        [SerializeField] private AudioSource VoiceVolume;
        [SerializeField] private AudioSource BackgroundFX;
        [SerializeField] private AudioSource SoundFX;
        public void Awake(){
            if(Instance == null){
                Instance = this;
            }
            if(VoiceVolume == null){
                VoiceVolume = this.gameObject.AddChildWithComponent<AudioSource>("VoiceVolume");
            }
            if(BackgroundFX == null){
                BackgroundFX = this.gameObject.AddChildWithComponent<AudioSource>("BackgroundFX");
            }
            if(SoundFX == null){
                SoundFX = this.gameObject.AddChildWithComponent<AudioSource>("SoundFX");
            }
        }
        public void Start(){
            Speak("Và giờ anh biết cuộc tình mình chẳng còn gì. Khi nắng xuân sang để lòng mình chẳng thầm thì");
        }

        public void PlayEffect(AudioClip clip) => SoundFX.PlayOneShot(clip);
        public void Speak(AudioClip clip) => VoiceVolume.PlayOneShot(clip);
        public void Speak(string speech) {
            string url = googleSTT_URL + Uri.EscapeDataString(speech);
            NetworkManager.Instance.GetAudioClip(url,(clip)=>VoiceVolume.PlayOneShot(clip));
        }
    }
}