using System.Collections;
using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public class SpeakCommand : ITutorialCommand
    {
        private string m_text;
        private AudioClip m_clip;
        public SpeakCommand(string text){
            m_text = text;
        }
        public IEnumerator Execute(ICommander commander)
        {
            if(m_clip != null){
                yield return Managers.AudioManager.Instance.SpeakAndWait(m_clip);
            }
            else{
                yield return Managers.AudioManager.Instance.SpeakAndWait(m_text);
            }
        }
        public void AddClip(AudioClip clip){
            m_clip = clip;
        }
    }
}