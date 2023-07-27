using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;

namespace Project.Audio{
    public interface IAudioState{
        void EnterState();
        void ExitState();
    }

    public class SnapshotAudioState : IAudioState
    {
        private AudioMixer _mixer;
        private AudioMixerSnapshot _snapshot;
        private float _weight;
        private float _timeToReach;

        public SnapshotAudioState(AudioMixer mixer, AudioMixerSnapshot snapshot, float weight = 1, float timeToReach = 0.5f){
            _mixer = mixer;
            _snapshot = snapshot;
            _weight = weight;
            _timeToReach = timeToReach;
        }
        public void EnterState()
        {
            _mixer.TransitionToSnapshots(new AudioMixerSnapshot[]{_snapshot}, new float[]{_weight}, _timeToReach);
        }

        public void ExitState()
        {
            //_mixer.TransitionToSnapshots(new AudioMixerSnapshot[]{_snapshot}, new float[]{0}, _timeToReach);
        }
    }

    public class SceneAudioState : IAudioState
    {

        private IAudioState m_wrappee;
        private AudioPackSTO _audioPack;

        public SceneAudioState(IAudioState wrappee, AudioPackSTO audioPack = null){
            m_wrappee = wrappee;
            _audioPack = audioPack;
        }
        
        protected virtual void SelfEnterState(){
            Managers.AudioManager.Instance.SwapSoundPack(_audioPack);
        }
        protected virtual void SelfExitState(){}
        public void EnterState()
        {
            SelfEnterState();
            m_wrappee?.EnterState();
        }

        public void ExitState()
        {
            SelfExitState();
            m_wrappee?.ExitState();
        }

        public void SetWrappee(IAudioState wrappee){
            if(wrappee == null) return;

            m_wrappee?.ExitState();
            m_wrappee = wrappee;
            m_wrappee.EnterState();
        }
    }
}