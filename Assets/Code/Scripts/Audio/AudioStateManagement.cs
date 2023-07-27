using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Project.Audio
{
    public class AudioStateManagement
    {
        #region Available Snapshot States
        public const string Default = "Default";
        public const string VoiceSpeak = "VoiceSpeak";
        public const string WatchingVideo = "WatchingVideo";
        public const string MuteBackground = "MuteBG";

        private AudioStateFactory _factory;
        #endregion
        public readonly AudioMixer MainMixer;
        public IAudioState CurrentAudioState {get;private set;}
        public AudioStateManagement(AudioMixer mixer)
        {
            MainMixer = mixer;
            _factory = new AudioStateFactory(mixer);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            UnityEngine.Debug.Log("Scene name: " + scene.name);
            ChangeSceneAudioState(scene);
        }

        public void ChangeAudioState(IAudioState state){
            if(state == null ){
                return;
            }
            // if(CurrentAudioState == state){
            //     return;
            // }
            CurrentAudioState?.ExitState();
            CurrentAudioState = state;
            state.EnterState();
        }

        internal void ChangeSceneAudioState(Scene scene)
        {
            ChangeAudioState(_factory.FindSceneAudioState(scene));
        }

        internal void ChangeSnapshotAudioState(string snapshotName)
        {
            if(CurrentAudioState is SceneAudioState sceneAudioState){
                sceneAudioState.SetWrappee(_factory.FindSnapshotState(snapshotName));
                return;
            }
            ChangeAudioState(_factory.FindSnapshotState(snapshotName));
        }
    }
}