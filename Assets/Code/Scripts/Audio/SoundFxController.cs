using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

namespace Project.Audio
{
    public class SoundFXController : MonoBehaviour
    {
        public enum SoundFXType
        {
            OnClick, OnCorrect, OnRewarded, OnError, OnPopUp
        }
        [System.Serializable]
        public struct SoundFXPack
        {
            public SoundFXType Type;
            public AudioClip Clip;
        }
        private AudioSource source;
        private Dictionary<SoundFXType, AudioClip> soundTable;
        private void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
            GenerateTable(null);
        }
        public void GenerateTable(SoundFXPack[] pack, bool clearBeforeGen = false)
        {
            if (clearBeforeGen || soundTable == null)
            {
                var values = System.Enum.GetValues(typeof(SoundFXType)).Cast<SoundFXType>();
                soundTable = new Dictionary<SoundFXType, AudioClip>(values.Count());

                foreach (var val in values)
                {
                    soundTable.Add(val, null);
                }
            }
            if(pack == null) return;
            foreach (var sound in pack)
            {
                soundTable[sound.Type] = sound.Clip;
            }
        }
        public void AssignMixerGroup(AudioMixerGroup mixer)
        {
            source.outputAudioMixerGroup = mixer;
        }
        public void PlayEffect(SoundFXType type)
        {
            if(soundTable[type] == null) return;

            source.PlayOneShot(soundTable[type]);
        }
        public void PlayEffect(AudioClip clip) => source.PlayOneShot(clip);
    }
}