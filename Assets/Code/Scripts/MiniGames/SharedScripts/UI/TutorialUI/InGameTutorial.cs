using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Project.MiniGames;

namespace Project.MiniGames.UI{
    public class InGameTutorial : MonoBehaviour{
        [SerializeField] private string videoUrl;
        [SerializeField] private VideoPlayerBehaviour videoPlayer;

        [SerializeField] private Button startGameButton;
        
        void OnDisable(){
            StartCoroutine(DisableVideoPlayer());
            startGameButton?.onClick.RemoveListener(StartGame);
        }
        void OnEnable(){
            StartCoroutine(EnableVideoPlayer());
            startGameButton?.onClick.AddListener(StartGame);
        }

        private IEnumerator EnableVideoPlayer(){
            if(videoUrl == "" || videoPlayer == null) {
                yield break;
            }
            yield return videoPlayer.PrepareVideoUrl(videoUrl);
            videoPlayer.PauseVideo();
        }
        private IEnumerator DisableVideoPlayer(){
            yield return videoPlayer?.Unload();
        }
        private void StartGame(){
            BaseGameEventManager.Instance.RaiseEvent(BaseGameEventManager.StartGameEventName);
        }
    }
}