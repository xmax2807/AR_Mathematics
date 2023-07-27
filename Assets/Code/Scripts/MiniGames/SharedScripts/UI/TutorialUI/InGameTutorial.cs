using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Project.MiniGames;
using Project.Managers;
using UnityEngine.Video;
using Project.Audio;

namespace Project.MiniGames.UI
{
    public class InGameTutorial : MonoBehaviour
    {
        [SerializeField] private string videoUrl;
        [SerializeField] private bool isFromResource;
        [SerializeField] private VideoPlayerBehaviour videoPlayer;
        [SerializeField] private Gameframe.GUI.PanelSystem.AnimatedPanelView view;
        [SerializeField] private Button startGameButton;

        private VideoClip clip;

        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        void OnDisable()
        {
            GameManager.Instance.OnGameFinishLoading -= PlayVideo;
            TimeCoroutineManager.Instance?.StartCoroutine(DisableVideoPlayer());
            startGameButton?.onClick.RemoveListener(StartGame);
        }
        void OnEnable()
        {
            GameManager.Instance.OnGameFinishLoading += PlayVideo;
            StartCoroutine(EnableVideoPlayer());
            startGameButton?.onClick.AddListener(StartGame);
        }

        private IEnumerator EnableVideoPlayer()
        {
            if (videoUrl == "" || videoPlayer == null)
            {
                Debug.Log($"GameTutorial: VideoURL is empty");
                yield break;
            }
            if (isFromResource)
            {
                yield return ResourceManager.Instance.GetLocalFile<VideoClip>(videoUrl, (result) => {
                    Debug.Log("got clip");
                    clip = result;
                });
                yield return new WaitForEndOfFrame();
                if (clip == null)
                {
                    Debug.Log($"GameTutorial: Video not found at {videoUrl}");
                    yield break;
                }
                yield return videoPlayer.PrepareVideoClip(clip);
            }
            else
            {
                yield return videoPlayer.PrepareVideoUrl(videoUrl);
            }

            //videoPlayer.PauseVideo();
            
        }
        private void PlayVideo(){
            AudioManager.Instance.ChangeSnapshot(AudioStateManagement.VoiceSpeak);
            videoPlayer.PlayVideo();
        }
        private IEnumerator DisableVideoPlayer()
        {
            yield return videoPlayer?.Unload();
            AudioManager.Instance.ChangeSnapshot(AudioStateManagement.Default);
        }
        private async void StartGame()
        {
            await view?.HideAsync();
            canvas.enabled = false;
            BaseGameEventManager.Instance?.RaiseEvent(BaseGameEventManager.StartGameEventName);
        }
    }
}