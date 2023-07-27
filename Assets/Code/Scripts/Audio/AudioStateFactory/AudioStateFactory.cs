using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Project.Audio{
    public class AudioStateFactory{
        private AudioMixer _mixer;
        private Dictionary<string, SnapshotAudioState> _snapshotStates;
        private Dictionary<string, SceneAudioState> _sceneStates;
        internal AudioStateFactory(AudioMixer mixer){
            this._mixer = mixer;
            InitializeSnapshotStates();
            InitializeSceneStates();
        }

        private void InitializeSnapshotStates(){
            _snapshotStates= new();
            AddSnapshot(AudioStateManagement.Default, timeToReach: 2f);
            AddSnapshot(AudioStateManagement.VoiceSpeak);
            AddSnapshot(AudioStateManagement.WatchingVideo, timeToReach: 1f);
            AddSnapshot(AudioStateManagement.MuteBackground);
        }

        internal void AddSnapshot(string name, float weight = 1f, float timeToReach = 0.5f){
            //ensure mixer found the snapshot before adding to map

            AudioMixerSnapshot snapshot = _mixer.FindSnapshot(name);
            if(snapshot == null){
                UnityEngine.Debug.Log("No snapshot named " + name);
                return;
            }
            _snapshotStates.Add(name, new SnapshotAudioState(_mixer, snapshot, weight, timeToReach));
        }
        internal void AddCustomSnapshot(string name, AudioMixerSnapshot snapshot){
            _snapshotStates.Add(name, new SnapshotAudioState(_mixer, snapshot));
        }
        public SnapshotAudioState FindSnapshotState(string snapshotName){
            return _snapshotStates.TryGetValue(snapshotName, out var state) ? state : null;
        }


        private void InitializeSceneStates(){
            _sceneStates = new();
            _sceneStates.Add("MainMenuScene", new SceneAudioState(_snapshotStates["Default"], Managers.AudioManager.Instance.DefaultAudioPack));
            _sceneStates.Add("LessonVideoScene", new LessonAudioState(_snapshotStates[AudioStateManagement.MuteBackground]));
            _sceneStates.Add("FinalTestScene", new SceneAudioState(_snapshotStates[AudioStateManagement.MuteBackground]));
            _sceneStates.Add("TutorialScene", new SceneAudioState(_snapshotStates[AudioStateManagement.MuteBackground]));
        }

        internal IAudioState FindSceneAudioState(Scene scene)
        {
            return _sceneStates.TryGetValue(scene.name, out var state) ? state : null;
        }
    }
}