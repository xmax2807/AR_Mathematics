using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Project.MiniGames;
using Project.Managers;

namespace Project.MiniGames.UI{
    public class InGameTutorial : MonoBehaviour{
        [SerializeField] private string videoUrl;
        [SerializeField] private VideoPlayerBehaviour videoPlayer;
        [SerializeField] private Gameframe.GUI.PanelSystem.AnimatedPanelView view;
        [SerializeField] private Button startGameButton;

        private Canvas canvas;

        private void Awake(){
            canvas = GetComponent<Canvas>();
        }
        
        void OnDisable(){
            TimeCoroutineManager.Instance.StartCoroutine(DisableVideoPlayer());
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
        private async void StartGame(){
            await view?.HideAsync();
            canvas.enabled = false;
            BaseGameEventManager.Instance?.RaiseEvent(BaseGameEventManager.StartGameEventName);
        }
    }
}