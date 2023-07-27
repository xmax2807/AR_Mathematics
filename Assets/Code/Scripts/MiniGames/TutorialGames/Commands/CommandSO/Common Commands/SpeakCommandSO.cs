using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/SpeakCommand", fileName = "SpeakCommandSO")]
    public class SpeakCommandSO : CommandSO{
        [SerializeField] private string m_text;
        private AudioClip cache;
        void OnEnable(){
            // subscribe OnSceneUnload
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += DeleteCache;
            
        }

        private void DeleteCache(Scene arg0)
        {
            cache = null;
        }

        void OnDisable(){
            // unsubscribe OnSceneUnload
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= DeleteCache;
            cache = null;
        }
        public override ITutorialCommand BuildCommand()
        {
            SpeakCommand command = new(m_text);
            if(cache == null){
                Managers.AudioManager.Instance.GetAudioClip(m_text, (clip)=> {
                    cache = clip;
                    command.AddClip(clip);
                });
            }
            else{
                command.AddClip(cache);
            }
            return command;
        }
    }
}