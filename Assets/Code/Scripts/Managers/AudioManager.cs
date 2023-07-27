using UnityEngine;
using Project.Utils.ExtensionMethods;
using System;
using System.Collections;
using UnityEngine.Audio;
using Project.Audio;
using UnityEngine.SceneManagement;

namespace Project.Managers{
    public class AudioManager : MonoBehaviour{
        
        public static AudioManager Instance;

        private const string googleSTT_URL = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=vi&q=";
        public AudioSource VoiceVolume {get; private set;}
        public AudioSource BackgroundFX {get;private set;}
        private SoundFXController SoundFXController;

        #region Audio Mixer Group
        private AudioMixer mainMixer;
        private const string MainAudioMixer = "Audio/MainMixer";
        private const string VoiceVolumeMixer = "VoiceMixerGroup";
        private const string BackgroundMixer = "BackgroundMixerGroup";
        private const string SoundFXMixer = "SoundFXMixerGroup";
        #endregion
        
        private const string AudioPackPath = "Audio/DefaultAudioPack";
        private AudioPackSTO m_defaultAudioPack;
        public AudioPackSTO DefaultAudioPack => m_defaultAudioPack;

        private AudioStateManagement m_audioStateManagement;
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

            // ask for asset resource for Audio mixer from 3 paths
            StartCoroutine(ResourceManager.Instance.AskForAsset<AudioMixer>(MainAudioMixer, OnReceiveMixer));
        }

        private void OnReceiveMixer(AudioMixer mixer) {
            mainMixer = mixer;
            m_audioStateManagement = new AudioStateManagement(mainMixer);

            AudioMixerGroup[] voiceGroup = mainMixer.FindMatchingGroups(VoiceVolumeMixer);
            if(voiceGroup != null && voiceGroup.Length > 0){
                VoiceVolume.outputAudioMixerGroup = voiceGroup[0];
            }

            AudioMixerGroup[] backgroundGroup = mainMixer.FindMatchingGroups(BackgroundMixer);
            if(backgroundGroup != null && backgroundGroup.Length > 0){
                BackgroundFX.outputAudioMixerGroup = backgroundGroup[0];
            }

            AudioMixerGroup[] soundFXGroup = mainMixer.FindMatchingGroups(SoundFXMixer);
            if(soundFXGroup != null && soundFXGroup.Length > 0){
                SoundFXController.AssignMixerGroup(soundFXGroup[0]);
            }

            m_audioStateManagement.ChangeSnapshotAudioState(AudioStateManagement.Default);
        }

        private void OnReceiveDefaultPack(Project.Audio.AudioPackSTO pack){
            m_defaultAudioPack = pack;
            SwapSoundPack(pack);
        }

        public void PlayEffect(Audio.SoundFXController.SoundFXType type) => SoundFXController.PlayEffect(type);
        public void PlayEffect(AudioClip clip) => SoundFXController.PlayEffect(clip);
        private void PlayOneShot(AudioSource source,AudioClip clip){
            if(clip == null) return;

            source.PlayOneShot(clip);
        }
        private void PlaySound(AudioClip clip, AudioSource src = null, bool shouldLoop = true, Action onDonePlaying = null){
            if(clip == null) {
                onDonePlaying?.Invoke();
                return;
            }
            if(src == null) src = BackgroundFX;

            src.Stop();
            src.clip = clip;
            src.loop = shouldLoop;
            src.Play();

            if(onDonePlaying != null){
                TimeCoroutineManager.Instance.WaitUntil(() => src.isPlaying == false, onDonePlaying);
            }
        }
        public void Speak(AudioClip clip) {
            if(clip == null) return;
            ChangeSnapshot(AudioStateManagement.VoiceSpeak);
            PlaySound(clip, VoiceVolume, shouldLoop: false, onDonePlaying: () => ChangeSnapshot(AudioStateManagement.Default));
        }
        public void Speak(string speech, Action<AudioClip> callback = null) {
            //Check if string speech is empty or only spaces
            if(string.IsNullOrEmpty(speech) || string.IsNullOrWhiteSpace(speech)) return;

            string url = googleSTT_URL + Uri.EscapeDataString(speech);
            NetworkManager.Instance.GetAudioClip(url,(clip)=>{
                callback?.Invoke(clip);
                Speak(clip);
            });
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
            ChangeSnapshot(AudioStateManagement.VoiceSpeak);
            PlaySound(clip, VoiceVolume, false);
            yield return new WaitForSeconds(clip.length);
            ChangeSnapshot(AudioStateManagement.Default);
        }

        public void SwapSoundPack(Audio.AudioPackSTO pack)
        {
            if(pack == null) return;

            if(pack.BackgroundMusic != null){
                PlaySound(pack.BackgroundMusic, BackgroundFX, shouldLoop: true);
            }
            SoundFXController.GenerateTable(pack.SoundFXPacks);
        }

        public void ChangeSnapshot(string snapshotName){
            m_audioStateManagement.ChangeSnapshotAudioState(snapshotName);
        }
    }
}