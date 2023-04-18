using UnityEngine;
using System.Linq;

namespace Project.Audio
{
    [CreateAssetMenu(fileName = "AudioPack", menuName = "STO/Audio/Pack")]
    public class AudioPackSTO : ScriptableObject
    {
        public AudioClip BackgroundMusic;
        public SoundFXController.SoundFXPack[] SoundFXPacks =
            System.Enum.GetValues(typeof(SoundFXController.SoundFXType))
            .Cast<SoundFXController.SoundFXType>()
            .Select((val) => new SoundFXController.SoundFXPack() { Type = val })
            .ToArray();
    }
}