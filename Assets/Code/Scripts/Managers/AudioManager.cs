using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;
using System.Collections;

namespace Project.Managers{
    public class AudioManager : MonoBehaviour{
        
        public static AudioManager Instance;

        private const string googleSTT_URL = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=vi&q=";
        public AudioSource VoiceVolume {get; private set;}
        public AudioSource BackgroundFX {get;private set;}
        private const string AudioPackPath = "Audio/DefaultAudioPack";
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
                SoundFXController = this.gameObject.AddChildWithScript<Audio.SoundFXController>("SoundFX");
            }
        }
        public void Start(){
            StartCoroutine(ResourceManager.Instance.AskForAsset<Project.Audio.AudioPackSTO>(AudioPackPath, OnReceiveDefaultPack));
        }

        private void OnReceiveDefaultPack(Project.Audio.AudioPackSTO pack){
            SwapSoundPack(pack);
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

        public Coroutine GetAudioClip(string speech, Action<AudioClip> callback) {
            string url = googleSTT_URL + Uri.EscapeDataString(speech);
            return NetworkManager.Instance.GetAudioClipAsync(url, callback);
        }

        public IEnumerator SpeakAndWait(string speech){
            string url = googleSTT_URL + Uri.EscapeDataString(speech);
            AudioClip audioClip = null;

            bool isCallbackReturned = false;
            NetworkManager.Instance.GetAudioClip(url, (clip)=> {
                audioClip = clip;
                isCallbackReturned = true;
            });
            yield return new WaitUntil(() => isCallbackReturned == true);

            yield return SpeakAndWait(audioClip);
        }
        public IEnumerator SpeakAndWait(AudioClip clip){
            if(clip == null) yield break;

            VoiceVolume.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }

        public void SwapSoundPack(Audio.AudioPackSTO pack)
        {
            PlaySound(pack.BackgroundMusic);
            SoundFXController.GenerateTable(pack.SoundFXPacks);
        }
    }
}