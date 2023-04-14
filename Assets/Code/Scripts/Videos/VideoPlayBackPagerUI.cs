using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Panel
{
    public class VideoPlayBackPagerUI : ViewPagerUI<PreloadableVideoPanelView>
    {

        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private Canvas pausingCanvas;
        [SerializeField] private Canvas stoppedCanvas;
        [SerializeField] private Button pauseButton;
        private VideoPlayerBehaviour playerBehaviour;
        private VideoPlayerBehaviour.VideoState currentState;


        protected override void InitT(PreloadableVideoPanelView view)
        {
            base.InitT(view);

            if (playerBehaviour != null)
            {
                playerBehaviour.OnVideoStateChanged -= UpdateUI;
            }

            playerBehaviour = currentPanelView.VideoPlayerBehaviour;

            if (playerBehaviour != null)
            {
                playerBehaviour.OnVideoStateChanged += UpdateUI;
            }
            else
            {
                DisableAllCanvas();
            }
        }

        void OnEnable()
        {
            DisableAllCanvas();
            loadingCanvas.enabled = true;
        }

        private void UpdateUI(VideoPlayerBehaviour.VideoState state)
        {
            if (currentState == state) return;

            currentState = state;
            DisableAllCanvas();
            switch (state)
            {
                case VideoPlayerBehaviour.VideoState.Pausing:
                    pausingCanvas.enabled = true;
                    return;
                case VideoPlayerBehaviour.VideoState.Preparing:
                    loadingCanvas.enabled = true;
                    return;
                case VideoPlayerBehaviour.VideoState.Playing:
                    pauseButton.enabled = true;
                    return;
                case VideoPlayerBehaviour.VideoState.Stopped:
                    stoppedCanvas.enabled = true;
                    return;
            }
        }
        private void DisableAllCanvas()
        {
            loadingCanvas.enabled = false;
            pausingCanvas.enabled = false;
            stoppedCanvas.enabled = false;
            pauseButton.enabled = false;
        }
        public void PlayOrPause()
        {
            if (currentState == VideoPlayerBehaviour.VideoState.Playing)
            {
                playerBehaviour.PauseVideo();
            }
            else if (currentState == VideoPlayerBehaviour.VideoState.Pausing)
            {
                playerBehaviour.PlayVideo();
            }
            else if (currentState == VideoPlayerBehaviour.VideoState.Stopped)
            {
                playerBehaviour.ReplayVideo();
            }

        }
    }
}
