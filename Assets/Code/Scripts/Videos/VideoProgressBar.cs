using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Project.UI{

    public class VideoProgressBar : MonoBehaviour, IDragHandler,IBeginDragHandler, IEndDragHandler, 
    IPointerDownHandler, IPointerUpHandler
    {
        private Slider progressBar;
        private RectTransform progressTransform;
        private VideoPlayerBehaviour videoPlayer;

        private bool isDragging;

        void Awake(){
            progressBar = GetComponent<Slider>();
            progressTransform = this.GetComponent<RectTransform>();
            progressBar.maxValue = 1;
            progressBar.minValue = 0;
            progressBar.direction = Slider.Direction.LeftToRight;
        }
        void  Start(){
            enabled = videoPlayer != null;
        }

        public void SetVideoPlayerBehaviour(VideoPlayerBehaviour player){
            if(this.videoPlayer != null){
                this.videoPlayer.OnSeekCompleted -= PlayVideo;
            }
            this.videoPlayer = player;
            videoPlayer.OnSeekCompleted += PlayVideo;
            enabled = true;
        }
        void Update(){
            if(!isDragging && videoPlayer.FrameCount > 0){
                float percentage = (float)videoPlayer.Frame / videoPlayer.FrameCount;
                progressBar.value = percentage;
            }
        }

        void UpdateVideoSeek(float percentage){
            float frame = videoPlayer.FrameCount * percentage;
            videoPlayer.Frame = (long)frame;
        }

        void PlayVideo(VideoPlayer player){
            // if(!isDragging){
            //     this.videoPlayer.PlayVideo();
            // }
        }

        public void OnDrag(PointerEventData eventData)
        {
            TrySkip(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            TrySkip(eventData);
        }

        private void TrySkip(PointerEventData eventData){
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(progressTransform, eventData.position, Managers.GameManager.MainGameCam, out Vector2 localPoint))
            {
                this.videoPlayer.Mute(true);
                float percentage = Mathf.InverseLerp(progressTransform.rect.xMin, progressTransform.rect.xMax, localPoint.x);
                UpdateVideoSeek(percentage);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            progressBar.onValueChanged.RemoveListener(UpdateVideoSeek);
            videoPlayer.Mute(false);
            videoPlayer.PlayVideo();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            progressBar.onValueChanged.AddListener(UpdateVideoSeek);
            this.videoPlayer.Mute(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;
             videoPlayer.Mute(false);
            videoPlayer.PlayVideo();
        }
    }
}