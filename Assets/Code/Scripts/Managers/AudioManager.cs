using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;

namespace Project.Managers{
    public class AudioManager : MonoBehaviour{
        
        public static AudioManager Instance;

        private const string googleSTT_URL = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=vi&q=";
        private AudioSource VoiceVolume;
        private AudioSource BackgroundFX;
        private Project.Audio.SoundFXController SoundFXController;
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
            if(SoundFXController == null){
                SoundFXController = this.gameObject.AddChildWithComponent<Audio.SoundFXController>("SoundFX");
            }
        }
        public void Start(){
            //Speak("Và giờ anh biết cuộc tình mình chẳng còn gì. Khi nắng xuân sang để lòng mình chẳng thầm thì");
        }

        public void PlayEffect(Audio.SoundFXController.SoundFXType type) => SoundFXController.PlayEffect(type);
        public void PlayEffect(AudioClip clip) => SoundFXController.PlayEffect(clip);
        public void Speak(AudioClip clip) => PlayOneShot(VoiceVolume,clip);
        private void PlayOneShot(AudioSource source,AudioClip clip){
            if(clip == null) return;

            source.PlayOneShot(clip);
        }
        private void PlaySound(AudioClip clip, AudioSource src = null){
            if(clip == null) return;
            if(src == null) src = BackgroundFX;

            src.Stop();
            src.clip = clip;
            src.loop = true;
            src.Play();
        }
        public void Speak(string speech) {
            string url = googleSTT_URL + Uri.EscapeDataString(speech);
            NetworkManager.Instance.GetAudioClip(url,(clip)=>VoiceVolume.PlayOneShot(clip));
        }

        public void SwapSoundPack(Audio.AudioPackSTO pack)
        {
            PlaySound(pack.BackgroundMusic);
            SoundFXController.GenerateTable(pack.SoundFXPacks);
        }
    }
}